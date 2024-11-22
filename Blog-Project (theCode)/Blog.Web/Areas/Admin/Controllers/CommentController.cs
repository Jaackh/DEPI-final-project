using Blog.Entities.Repositories;
using Blog.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.EditorRole}")]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var comments = _unitOfWork.Comment.GetAll(Includeword: "applicationUser,Post");
            return View(comments);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var comment = _unitOfWork.Comment.GetFirstorDefault(c => c.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            _unitOfWork.Comment.Remove(comment);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

    }
}
