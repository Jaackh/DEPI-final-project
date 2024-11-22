using Blog.DataAccess.Implementation;
using Blog.Entities.Models;
using Blog.Entities.Repositories;
using Blog.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.EditorRole}")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View("Create",category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id <= 0 || id == null)
            {
                return NotFound();
            }

            Category category = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int? id)
        {
            if(id <= 0 || id == null)
            {
                return NotFound();
            }
            Category category = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }



        public IActionResult Edit(int? id)
        {
            if(id <= 0 ||id == null)
            {
                return NotFound();
            }
            Category category = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            return View(category);
        }

        public IActionResult ConfirmEdit(Category category)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
