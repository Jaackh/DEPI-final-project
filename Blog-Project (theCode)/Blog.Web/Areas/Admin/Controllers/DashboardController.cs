using Blog.Entities.Repositories;
using Blog.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.EditorRole}")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            // Fetch the counts for posts, comments, users, and categories
            ViewBag.PostCount = _unitOfWork.Post.GetAll().Count();
            ViewBag.CommentCount = _unitOfWork.Comment.GetAll().Count();
            ViewBag.UserCount = _unitOfWork.ApplicationUser.GetAll().Count();
            ViewBag.CategoryCount = _unitOfWork.Category.GetAll().Count();

            // Get the last 4 comments
            var lastComments = _unitOfWork.Comment.GetAll(Includeword: "applicationUser,Post")
                .OrderByDescending(c => c.DateTime)
                .Take(4)
                .ToList();

            return View(lastComments);
        }
    }
}
