﻿using BORISEVICH.Domain.Entities;
using BORISEVICH.UI.Extension;
using BORISEVICH.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BORISEVICH.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private Cart _cart;
        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: CartController
        public ActionResult Index()
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            return View(_cart.CartItems);
        }
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Success)
            {
                _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
                _cart.AddToCart(data.Data);
                HttpContext.Session.Set<Cart>("cart", _cart);
            }
            return Redirect(returnUrl);
        }
        [Route("[controller]/remove/{id:int}")]
        public ActionResult Remove(int id)
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            _cart.RemoveItems(id);
            HttpContext.Session.Set<Cart>("cart", _cart);
            return RedirectToAction("index");
        }
    }
}
