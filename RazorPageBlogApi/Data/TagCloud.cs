using System;
using System.ComponentModel.DataAnnotations;

namespace RazorPageBlogApi.Data
{
    public class TagCloud
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int Amount { get;  set; }
    }
}