using BigBook.DataAccess.Repository.IRepository;
using BigBook.Models;
using BigBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BigBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, "Product"),
                OrderHeader = new OrderHeader()
            };

            foreach(var cart in ShoppingCartViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(
                    cart.Count,
                    cart.Product.Price,
                    cart.Product.Price50,
                    cart.Product.Price100);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartViewModel);
        }
		public IActionResult Summary()
		{
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, "Product"),
                OrderHeader = new OrderHeader()
            };
            ShoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                .GetFirstOrDefault(u => u.Id == claim.Value);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.StreetAdress = ShoppingCartViewModel.OrderHeader.StreetAdress;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.City;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.State;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.PostalCode;

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(
                    cart.Count,
                    cart.Product.Price,
                    cart.Product.Price50,
                    cart.Product.Price100);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartViewModel);
		}

		private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else if (quantity <= 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }

        public IActionResult Plus(int cartId)
        {
            var cart  = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id==cartId);

            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
		public IActionResult Minus(int cartId)
		{
			var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

            if (cart.Count <= 1)
            {
				_unitOfWork.ShoppingCart.Remove(cart);
			}
            else
            {
				_unitOfWork.ShoppingCart.DecrementCount(cart, 1);
			}			

			_unitOfWork.Save();

			return RedirectToAction(nameof(Index));
		}

        public IActionResult Remove(int cartId)
        {
			var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

			_unitOfWork.ShoppingCart.Remove(cart);
			_unitOfWork.Save();

			return RedirectToAction(nameof(Index));
		}
	}
}
