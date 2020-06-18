using Microsoft.AspNetCore.Mvc;
using RazorPageBlogApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorPageBlogApi.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
