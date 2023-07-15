using BigBook.DataAccess.Repository.IRepository;
using BigBook.Models;
using BigBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BigBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {

            Company company = new();

            if (id == null || id == 0)
            {
                //Create    
                return View(company);
            }
            else
            {    // Update product
                company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);

                return View(company);

            }



        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(Company obj)
        {
            if (ModelState.IsValid)
            {
                
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult DELETE(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful"});
        }
        #endregion
    }
}

