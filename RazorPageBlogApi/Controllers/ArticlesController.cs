using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RazorPageBlogApi.Data;

namespace RazorPageBlogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly RazorPageBlogDbContext _blogDb;

        public ArticlesController(RazorPageBlogDbContext blogDb)
        {
            this._blogDb = blogDb;
        }

        [HttpGet]
        public IEnumerable<Article> Get()
        {
            return _blogDb.Articles.ToList();
        }

        [Route("{id:guid}")]
        [HttpGet]
        public Article Get(Guid id)
        {
            return _blogDb.Articles.Single(s => s.Id == id);
        }
    }
}
