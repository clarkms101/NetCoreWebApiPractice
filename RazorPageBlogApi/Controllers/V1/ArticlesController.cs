using Microsoft.AspNetCore.Mvc;
using RazorPageBlogApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorPageBlogApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)] // 將棄用的版本
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
