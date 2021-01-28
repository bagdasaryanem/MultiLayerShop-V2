using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
