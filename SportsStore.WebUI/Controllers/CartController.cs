//using System.Linq;
//using System.Web.Mvc;
//using SportsStore.Domain.Abstract;
//using SportsStore.Domain.Entities;
//using SportsStore.WebUI.Models;

//namespace SportsStore.WebUI.Controllers
//{

//    public class CartController : Controller
//    {
//        private IProductRepository repository;
//        private IOrderProcessor orderProcessor;

//        public CartController(IProductRepository repo)
//        {
//            repository = repo;
//        }

//        //public ViewResult Index(string returnUrl)
//        //{
//        //    return View(new CartIndexViewModel
//        //    {
//        //        Cart = GetCart(),
//        //        ReturnUrl = returnUrl
//        //    });
//        //}

//        public ViewResult Index(Cart cart, string returnUrl)
//        {
//            return View(new CartIndexViewModel
//            {
//                ReturnUrl = returnUrl,
//                Cart = cart
//            });
//        }

//        //public RedirectToRouteResult AddToCart(int productId, string returnUrl)
//        //{
//        //    Product product = repository.Products
//        //        .FirstOrDefault(p => p.ProductID == productId);

//        //    if (product != null)
//        //    {
//        //        GetCart().AddItem(product, 1);
//        //    }
//        //    return RedirectToAction("Index", new { returnUrl });
//        //}

//        public RedirectToRouteResult AddToCart(Cart cart, int productId,
//        string returnUrl)
//        {
//            Product product = repository.Products
//                .FirstOrDefault(p => p.ProductID == productId);

//            if (product != null)
//            {
//                cart.AddItem(product, 1);
//            }
//            return RedirectToAction("Index", new { returnUrl });
//        }

//        //public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
//        //{
//        //    Product product = repository.Products
//        //        .FirstOrDefault(p => p.ProductID == productId);

//        //    if (product != null)
//        //    {
//        //        GetCart().RemoveLine(product);
//        //    }
//        //    return RedirectToAction("Index", new { returnUrl });
//        //}

//        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId,
//        string returnUrl)
//        {
//            Product product = repository.Products
//                .FirstOrDefault(p => p.ProductID == productId);

//            if (product != null)
//            {
//                cart.RemoveLine(product);
//            }
//            return RedirectToAction("Index", new { returnUrl });
//        }

//        public PartialViewResult Summary(Cart cart)
//        {
//            return PartialView(cart);
//        }

//        public ViewResult Checkout()
//        {
//            return View(new ShippingDetails());
//        }

//        //private Cart GetCart()
//        //{

//        //    Cart cart = (Cart)Session["Cart"];
//        //    if (cart == null)
//        //    {
//        //        cart = new Cart();
//        //        Session["Cart"] = cart;
//        //    }
//        //    return cart;
//        //}
//    }
//}


using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{

    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }


        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                ReturnUrl = returnUrl,
                Cart = cart
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId,
                string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId,
                string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

    }
}
