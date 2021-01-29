using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using MultiLayerApp.DataAccess.Domain;
using MultiLayerApp.DataAccess.Repository.IRepository;
using MultiLayerApp.Utility;

namespace MultiLayerApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchString, int? category, int? priceFrom, int? priceTo, int pageNumber = 1, int pageSize = 8)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            if (!String.IsNullOrEmpty(searchString))
                productList = productList.Where(x => x.Name.ToLower().Contains(searchString));

            var categoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            ViewBag.categoryList = categoryList;

            if (category.HasValue)
                productList = productList.Where(x => x.Category.Id == category);

            if (priceFrom.HasValue)
                productList = productList.Where(x => x.Price >= priceFrom);

            if (priceTo.HasValue)
                productList = productList.Where(x => x.Price <= priceTo);

            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
            ViewBag.searchString = searchString;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart
                    .GetAll(c => c.UserId == claim.Value)
                    .ToList().Count();

                HttpContext.Session.SetInt32(StaticDetails.ShoppingCartSession, count);
            }

            var excludeRecords = (pageSize * pageNumber) - pageSize;
            var phoneCount = productList.Count();
            productList = productList.Skip(excludeRecords)
                                     .Take(pageSize);

            var result = new PagedResult<Product>
            {
                Data = productList.ToList(),
                TotalItems = phoneCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var productFromDb = _unitOfWork.Product.
                GetFirstOrDefault(u => u.Id == id, includeProperties: "Category");
            ShoppingCart cartObj = new ShoppingCart()
            {
                Product = productFromDb,
                ProductId = productFromDb.Id
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart cartObject)
        {
            cartObject.Id = 0;
            if (ModelState.IsValid)
            {
                //then we will add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartObject.UserId = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.UserId == cartObject.UserId && u.ProductId == cartObject.ProductId
                    , includeProperties: "Product"
                    );

                if (cartFromDb == null)
                {
                    //no records exists in database for that product for that user
                    _unitOfWork.ShoppingCart.Add(cartObject);
                }
                else
                {
                    cartFromDb.Count += cartObject.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                }
                _unitOfWork.Save();


                var count = _unitOfWork.ShoppingCart
                    .GetAll(c => c.UserId == cartObject.UserId)
                    .ToList().Count();

                HttpContext.Session.SetInt32(StaticDetails.ShoppingCartSession, count);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productFromDb = _unitOfWork.Product.
                        GetFirstOrDefault(u => u.Id == cartObject.ProductId, includeProperties: "Category");
                ShoppingCart cartObj = new ShoppingCart()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.Id
                };
                _logger.LogInformation("Unable to add product to the cart, as ModelState is not valid");
                return View(cartObj);
            }


        }
    }
}
