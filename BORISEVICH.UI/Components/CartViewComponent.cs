using BORISEVICH.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using BORISEVICH.UI.Extension;

namespace BORISEVICH.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }
    }
}
