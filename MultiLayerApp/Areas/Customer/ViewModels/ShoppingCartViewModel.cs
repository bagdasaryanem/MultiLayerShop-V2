using System.Collections.Generic;
using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.Areas.Customer.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
