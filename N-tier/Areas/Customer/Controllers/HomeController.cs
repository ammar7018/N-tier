using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N_tier.Data.Repository.IRepository;
using N_tier.Models;
using N_tier.Models.Models;
using N_tier.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace N_tier.Areas.Customer.Controllers
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

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim!=null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }

            IEnumerable<Product> productList = _unitOfWork.product.GetAll(includeProp: "Category");
            return View(productList);

        }

        public IActionResult Details(int productId)
        {
            if (productId == 0)
            {
                return NotFound();
            }

            ShoppingCart shoppingCart = new()
            {
                Product = _unitOfWork.product.Get((x => (x.Id == productId)), includeProp: "Category"),
                Count = 1,
                ProductId = productId

            };

            
            return View(shoppingCart);

        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimIdentity=(ClaimsIdentity)User.Identity;

            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDB = _unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId==userId
                                                                    && u.ProductId == shoppingCart.ProductId);
            if (cartFromDB != null)
            {
                cartFromDB.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
                _unitOfWork.Save();

            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();

                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }

            TempData["success"] = "Cart Updated successfully";

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}