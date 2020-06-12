using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPageBlogApi.Data
{
    public class Article
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        [StringLength(250)]
        public string CoverPhoto { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        public string Tags { get; set; }
    }
}