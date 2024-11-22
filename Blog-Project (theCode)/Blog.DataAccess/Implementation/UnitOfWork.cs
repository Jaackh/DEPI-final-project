using Blog.DataAccess.Data;
using Blog.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public ICommentRepository Comment { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public IPostRepository Post { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ApplicationUser = new ApplicationUserRepository(context);
            Comment = new CommentRepository(context);
            Category = new CategoryRepository(context);
            Post = new PostRepository(context);    
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
  
    
}
