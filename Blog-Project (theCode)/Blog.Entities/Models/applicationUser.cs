using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities.Models
{
    public class applicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        DateTime? CreatedAt { get; set; } = DateTime.Now;
        public List<Post>? Posts { get; set; }
        public List<Comment>? comments { get; set; }
    }
}
