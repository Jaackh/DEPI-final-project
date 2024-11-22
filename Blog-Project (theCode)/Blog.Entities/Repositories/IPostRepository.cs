using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        void Update(Post post);
    }
}
