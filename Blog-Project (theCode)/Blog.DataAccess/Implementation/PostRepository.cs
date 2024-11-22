
using Blog.DataAccess.Data;
using Blog.Entities.Models;
using Blog.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Implementation
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly AppDbContext _context;
        public PostRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Post post)
        {
            var postInDb = _context.posts.FirstOrDefault(x => x.Id == post.Id);
            if (postInDb != null)
            {
                postInDb.Title = post.Title;
                postInDb.CategoryId = post.CategoryId;
                postInDb.Content = post.Content;
                postInDb.Img = post.Img;
            }
        }
    }
}
