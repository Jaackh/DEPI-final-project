using Blog.Entities.Models;
using Blog.Entities.Repositories;
using Blog.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Web.Areas.Visitor.Controllers
{
    [Area("Visitor")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult AddComment(int postId, string text)
        {

            string userId = null;

            if (User?.Identity is ClaimsIdentity claimsIdentity)
            {
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    userId = claim.Value;
                }
            }

            var newComment = new Comment
            {
                PostId = postId,
                Contant = text,
                applicationUserId = userId, 
                DateTime = DateTime.Now
            };

            _unitOfWork.Comment.Add(newComment);
            _unitOfWork.Complete();

            return Ok();
        }




        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            var comment = _unitOfWork.Comment.GetFirstorDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            if (comment.applicationUserId != userId)
            {
                return Unauthorized();
            }

            _unitOfWork.Comment.Remove(comment);
            _unitOfWork.Complete();

            return Ok();
        }

    }
}
