using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Contant { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; }

        [ForeignKey(nameof(applicationUser))]
        [ValidateNever]
        public string applicationUserId { get; set;}
        [ValidateNever]
        public applicationUser applicationUser { get; set; }    
    }
}
