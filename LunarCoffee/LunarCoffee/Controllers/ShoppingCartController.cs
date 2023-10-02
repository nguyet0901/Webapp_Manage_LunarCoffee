using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LunarCoffee.Models;
using LunarCoffee.Models.EntityFramework;

namespace LunarCoffee.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null)
            {
                return View(cart.Items);
            }    
            return View();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart1 = (ShoppingCart)Session["Cart"];
            if (cart1 != null && cart1.Items.Any())
            {
                ViewBag.CheckCart = cart1;
                
            }
            //try
            //{
            //    if(Session["UserName"]!=null)
            //    {
            //        string username = "";
            //        username = Session["UserName"].ToString();
            //        var _user = db.Users.Where(t=>t.UserName.Equals(username)).FirstOrDefault();
            //        if (_user != null)
            //        {
            //            OrderViewModel _orderViewModel = new OrderViewModel();
            //            _orderViewModel.CustomerName = (string.IsNullOrEmpty(_user.FullName)?"Khách Hàng": _user.FullName);
            //            _orderViewModel.Phone = (string.IsNullOrEmpty(_user.Phone) ? "098*******" : _user.Phone);
            //            _orderViewModel.Email = _user.Email;
            //            _orderViewModel.TypePayment = 1;

            //            //thanh toan
            //            var code = new { Success = false, Code = -1 };
            //                ShoppingCart cart = (ShoppingCart)Session["Cart"];
            //                if (cart != null)
            //                {
            //                    Order order = new Order();
            //                    order.CustomerName = _orderViewModel.CustomerName;
            //                    order.Phone = _orderViewModel.Phone;
            //                    order.Address = "Địa Chỉ Trống";
            //                    order.Email = _orderViewModel.Email;
            //                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
            //                    {
            //                        ProductId = x.ProductId,
            //                        Quantity = x.Quantity,
            //                        Price = x.Price
            //                    }));
            //                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
            //                    order.TypePayment = _orderViewModel.TypePayment;
            //                    order.CreatedDate = DateTime.Now;
            //                    order.ModifiedDate = DateTime.Now;
            //                    order.CreatedBy = _orderViewModel.Phone;
            //                    Random rd = new Random();
            //                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            //                    //order.E = req.CustomerName;
            //                    db.Orders.Add(order);
            //                    db.SaveChanges();
            //                    //send mail cho khachs hang
            //                    var strSanPham = "";
            //                    var thanhtien = decimal.Zero;
            //                    var TongTien = decimal.Zero;
            //                    foreach (var sp in cart.Items)
            //                    {
            //                        strSanPham += "<tr>";
            //                        strSanPham += "<td>" + sp.ProductName + "</td>";
            //                        strSanPham += "<td>" + sp.Quantity + "</td>";
            //                        strSanPham += "<td>" + LunarCoffee.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
            //                        strSanPham += "</tr>";
            //                        thanhtien += sp.Price * sp.Quantity;
            //                    }
            //                    TongTien = thanhtien;
            //                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
            //                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
            //                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            //                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            //                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
            //                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
            //                    contentCustomer = contentCustomer.Replace("{{Email}}", _orderViewModel.Email);
            //                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
            //                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", LunarCoffee.Common.Common.FormatNumber(thanhtien, 0));
            //                    contentCustomer = contentCustomer.Replace("{{TongTien}}", LunarCoffee.Common.Common.FormatNumber(TongTien, 0));
            //                    LunarCoffee.Common.Common.SendMail("La Fleur De Marie", "Đơn hàng #" + order.Code, contentCustomer.ToString(), _orderViewModel.Email);

            //                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
            //                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
            //                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
            //                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            //                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
            //                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
            //                    contentAdmin = contentAdmin.Replace("{{Email}}", _orderViewModel.Email);
            //                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
            //                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", LunarCoffee.Common.Common.FormatNumber(thanhtien, 0));
            //                    contentAdmin = contentAdmin.Replace("{{TongTien}}", LunarCoffee.Common.Common.FormatNumber(TongTien, 0));
            //                    LunarCoffee.Common.Common.SendMail("La Fleur De Marie", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
            //                    cart.ClearCart();
            //                    return RedirectToAction("CheckOutSuccess");
                            
            //            }
            //            return Json(code);
            //        }
            //    }

            //}
            //catch(Exception e)
            //{

            //}
            return View();
        }

        public ActionResult CheckOutSuccess()
        {
            return View();
        }

        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partial_CheckOut()
        {
            if (Session["UserName"] != null)
            {
                string UserName = Session["UserName"].ToString();
                var _user = db.Users.Where(t => t.UserName.Equals(UserName)).FirstOrDefault();
                if (_user != null)
                {
                    OrderViewModel model = new OrderViewModel();
                    model.CustomerName = _user.FullName;
                    model.Phone= _user.Phone;
                    model.Address=_user.Address;
                    model.Email= _user.Email;
                    return PartialView(model);
                }
            }
                return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    order.Email = req.Email;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = req.Phone;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    order.DeliveryDate=req.DeliveryDate;
                    order.DeliveryHour=req.DeliveryHour;
                    order.DeliveryMinute=req.DeliveryMinute;
                    //order.E = req.CustomerName;
                    db.Orders.Add(order);
                    db.SaveChanges();

                    Export export = new Export();
                    export.WareName = "Kho 1";
                    export.Quantity = cart.Items.Sum(x => (x.Quantity)); ;
                    export.IsExport = 0;

                    cart.Items.ForEach(x => export.ExportDetails.Add(new ExportDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitID = db.Products.Find(x.ProductId).UnitID
                    }));
                    export.CreatedDate = DateTime.Now;
                    export.ModifiedDate = DateTime.Now;
                    export.CreatedBy = req.Phone;
                    Random rd1 = new Random();
                    export.Code = "PX" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    //order.E = req.CustomerName;
                    db.Exports.Add(export);
                    db.SaveChanges();
                    //send mail cho khachs hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + LunarCoffee.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address); 
                    contentCustomer = contentCustomer.Replace("{{ThoiGianNhanHang}}", order.DeliveryDate.Value.ToString("dd/MM/yyy") + " " + order.DeliveryHour + ":" + order.DeliveryMinute);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", LunarCoffee.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", LunarCoffee.Common.Common.FormatNumber(TongTien, 0));
                    LunarCoffee.Common.Common.SendMail("La Fleur De Marie", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{ThoiGianNhanHang}}", order.DeliveryDate.Value.ToString("dd/MM/yyy") + " " + order.DeliveryHour + ":" + order.DeliveryMinute);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", LunarCoffee.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", LunarCoffee.Common.Common.FormatNumber(TongTien, 0));
                    LunarCoffee.Common.Common.SendMail("La Fleur De Marie", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}