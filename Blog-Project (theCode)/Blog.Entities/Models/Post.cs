using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        [ValidateNever]
        public string Img { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(Category))]
        [DisplayName("Category")]
        [Range(1, int.MaxValue, ErrorMessage = "The Category Not Valid")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        [DisplayName("author")]
        [ForeignKey(nameof(applicationUser))]
        [ValidateNever]
        public string applicationUserId { get; set; }
        [ValidateNever]
        public applicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public List<Comment>? comments { get; set; }
    }

}
