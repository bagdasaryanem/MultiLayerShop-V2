using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiLayerApp.DataAccess.Data;
using MultiLayerApp.Utility;

namespace MultiLayerApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.RoleAdmin + "," + StaticDetails.RoleEmployee)]
    public class UserController : Controller
    {
        private readonly ShopDbContext _db;

        public UserController(ShopDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }


        //API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ShopUsers.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new { data = userList });
        }
    }
}
