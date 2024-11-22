using Blog.Entities.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Areas.Visitor.Controllers
{
    [Area("Visitor")]

    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }


        public IActionResult GetCategory(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.GetFirstorDefault(
                x => x.Id == id,
                Includeword: "Posts"
            );

            if (category == null)
                return NotFound();

            return View(category);
        }

    }
}
