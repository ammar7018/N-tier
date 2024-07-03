
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using N_tier.Data;
using N_tier.Data.Repository;
using N_tier.Data.Repository.IRepository;
using N_tier.Models;
using N_tier.Models.Models;
using N_tier.Utility;

namespace N_tier.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {    
        

        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                return View(CompanyObj);
            }
        }

        #region All API

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company? obj = _unitOfWork.Company.Get(x => x.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            // TempData["success"] = "product deleted Successfully";

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
