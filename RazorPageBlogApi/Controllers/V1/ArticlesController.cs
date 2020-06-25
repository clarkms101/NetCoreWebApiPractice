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

        /// <summary>
        /// 取得所有的文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Article> Get()
        {
            return _blogDb.Articles.ToList();
        }

        /// <summary>
        /// 取得指定的文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:guid}")]
        [HttpGet]
        public Article Get(Guid id)
        {
            return _blogDb.Articles.Single(s => s.Id == id);
        }
    }
}
