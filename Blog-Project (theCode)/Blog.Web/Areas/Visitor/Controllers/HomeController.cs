using Blog.DataAccess.Implementation;
using Blog.Entities.Repositories;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Blog.Web.Areas.Visitor.Controllers
{
    [Area("Visitor")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? page)
        {
            var PageNumber = page ?? 1;
            int PageSize = 6;

            var products = _unitOfWork.Post
                .GetAll(Includeword: "Category,ApplicationUser")
                .OrderByDescending(x => x.CreatedAt) 
                .ToPagedList(PageNumber, PageSize);

            return View(products);
        }


        public IActionResult GetPost(int? id)
        {
            if(id == null || id <= 0)
            {
                return NotFound();
            }
            var categories = _unitOfWork.Category.GetAll().OrderBy(c => c.Name).ToList();
            ViewBag.Categories = categories;

            var post = _unitOfWork.Post.GetFirstorDefault(x=>x.Id == id , Includeword: "Category,ApplicationUser,comments,comments.applicationUser");
            if(post == null) 
                return NotFound();
            return View(post);
        }
    }
}
