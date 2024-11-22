using Blog.Entities.Models;
using Blog.Entities.Repositories;
using Blog.Entities.ViewModel;
using Blog.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;


namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.EditorRole}")]
    public class PostController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PostController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var post = _unitOfWork.Post.GetAll(Includeword: "Category,ApplicationUser")
                .OrderByDescending(x => x.CreatedAt);

            return View(post);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModelForPost = new PostCategories
            {
                post = new Post(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };
            return View(viewModelForPost);
        }

        [HttpPost]
        public IActionResult Save(PostCategories postcategories, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Posts");
                    var ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    postcategories.post.Img = @"Images\Posts\" + filename + ext;
                }

                //Add auther

                string userId = null;

                if (User?.Identity is ClaimsIdentity claimsIdentity)
                {
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        userId = claim.Value;
                    }
                }
                postcategories.post.applicationUserId = userId;

                //  return Content(postcategories.post.applicationUserId + "postcategories.post.Img");

                _unitOfWork.Post.Add(postcategories.post);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View("Create", postcategories);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id <= 0 || id == null)
            {
                return NotFound();
            }
            var viewModelForPost = new PostCategories
            {
                post = _unitOfWork.Post.GetFirstorDefault(x=> x.Id == id) ,
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };
            return View(viewModelForPost);  
        }

        [HttpPost]
        public IActionResult Edit(PostCategories postcategories, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var UploadPath = Path.Combine(RootPath, @"Images\Posts");
                    var ext = Path.GetExtension(file.FileName);

                    if (!string.IsNullOrEmpty(postcategories.post.Img))
                    {
                        var oldImgPath = Path.Combine(RootPath, postcategories.post.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }


                    using (var filestream = new FileStream(Path.Combine(UploadPath, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }


                    postcategories.post.Img = @"Images\Posts\" + filename + ext;
                }

                _unitOfWork.Post.Update(postcategories.post);
                _unitOfWork.Complete();

                return RedirectToAction("Index");
            }

            return View(postcategories);
        }

        public IActionResult Delete(int id)
        {
            var post = _unitOfWork.Post.GetFirstorDefault(x => x.Id == id , Includeword: "Category,ApplicationUser");
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _unitOfWork.Post.GetFirstorDefault(x => x.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            string RootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(post.Img))
            {
                var imgPath = Path.Combine(RootPath, post.Img.TrimStart('\\'));
                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }

            _unitOfWork.Post.Remove(post);
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

    }
}
