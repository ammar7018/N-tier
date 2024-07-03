using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using N_tier.Data.Repository.IRepository;
using N_tier.Models;
using N_tier.Models.Models;
using N_tier.Models.ViewModels;
using N_tier.Utility;
using System.Collections.Generic;

namespace N_tier.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.product.GetAll(includeProp: "Category").ToList();
            return View(products);
        }

/*        public IActionResult Delete(int? id) {

            if (id == null||id<=0)
            {
                return NotFound();
            }

            Product? obj = _unitOfWork.product.Get(x=>x.Id==id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);        
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.product.Get(x => x.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "product deleted Successfully";

            return RedirectToAction("Index");
        }
*/
        /*public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _unitOfWork.product.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _unitOfWork.product.Update(obj);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View();
        }*/

        public IActionResult Upsert(int ? id)
        {
            ProductVM vm;
            if (id == null || id == 0)
            {
                vm = new()
                {

                    CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }),
                    product = new Product()

                };
            }
            else
            {
                vm = new()
                {
                    CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }),
                    product = _unitOfWork.product.Get(x => x.Id == id)

                };
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            //obj.product.Category = _unitOfWork.Category.Get(x => obj.product.CategoryId == x.ID); 
            
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath= Path.Combine(wwwRootPath, @"image\product");

                    if (!string.IsNullOrEmpty(productVM.product.imageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.product.imageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.product.imageUrl = @"\image\product\"+fileName;
                    
                }

                if (productVM.product.Id == 0)
                {
                    _unitOfWork.product.Add(productVM.product);
                }
                else
                {
                    _unitOfWork.product.Update(productVM.product);
                }


                _unitOfWork.Save();
                TempData["success"] = "product Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
                return View(productVM);
            }

        }

        #region All API

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.product.GetAll(includeProp:"Category").ToList();

            /*foreach(var obj in objProductList)
            {
                obj.Category.products=null;
                  
            }*/

            return Json(new { data=objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product? obj = _unitOfWork.product.Get(x => x.Id == id);

            if (obj == null)
            {   
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (obj.imageUrl != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.imageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }


            _unitOfWork.product.Remove(obj);
            _unitOfWork.Save();
            // TempData["success"] = "product deleted Successfully";

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
