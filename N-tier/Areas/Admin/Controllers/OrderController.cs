using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_tier.Data.Repository.IRepository;
using N_tier.Models.Models;
using N_tier.Models.ViewModels;
using N_tier.Utility;
using Stripe;
using Stripe.Checkout;
using Stripe.FinancialConnections;
using System.Security.Claims;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;


namespace N_tier.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProp: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == id, includeProp: "Product")
            };

            return View(OrderVM);
        }

        [HttpPost]
        [ActionName("Detail")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrderDetail() {

            OrderHeader OrderHeaderFromDB = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            OrderHeaderFromDB.Name = OrderVM.OrderHeader.Name;
            OrderHeaderFromDB.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            OrderHeaderFromDB.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            OrderHeaderFromDB.City = OrderVM.OrderHeader.City;
            OrderHeaderFromDB.State = OrderVM.OrderHeader.State;
            OrderHeaderFromDB.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                OrderHeaderFromDB.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                OrderHeaderFromDB.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(OrderHeaderFromDB);
            _unitOfWork.Save();

            TempData["Success"] = "Order Detail Updated Successfully";

            return RedirectToAction(nameof(Detail), new { id = OrderHeaderFromDB.Id });
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Saccessfully";

            return RedirectToAction(nameof(Detail), new { id = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {

            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier= OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus= SD.StatusShipped;
            orderHeader.ShippingDate= DateTime.Now;

            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate=DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();

            TempData["Success"] = "Order Shipped Saccessfully";

            return RedirectToAction(nameof(Detail), new { id = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder()
        {
            var orderHeaderFromDB = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            if (orderHeaderFromDB.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions()
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDB.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDB.Id, SD.StatusCancelled, SD.StatusRefunded);

            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDB.Id, SD.StatusCancelled, SD.StatusCancelled);
            }

            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully.";

            return RedirectToAction(nameof(Detail), new { id = OrderVM.OrderHeader.Id });
        }

        [ActionName("Detail")]
        [HttpPost]
        public IActionResult Details_PAY_NOW()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeader
                .Get(u => u.Id == OrderVM.OrderHeader.Id, includeProp: "ApplicationUser");
            OrderVM.OrderDetail = _unitOfWork.OrderDetail
                .GetAll(u => u.OrderHeaderId == OrderVM.OrderHeader.Id, includeProp: "Product");

            //stripe logic
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in OrderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult PaymentConfirmation(int orderHeaderId)
        {

            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderHeaderId);
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                //this is an order by company

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }


            }


            return View(orderHeaderId);
        }

        #region All API

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderList;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                objOrderList = _unitOfWork.OrderHeader.GetAll(includeProp: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objOrderList = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserID== userId, includeProp: "ApplicationUser").ToList();
            }

            if (status!="All")
                objOrderList = objOrderList.Where(u => u.PaymentStatus == status);

			return Json(new { data = objOrderList });
		}

        # endregion
	}
}
