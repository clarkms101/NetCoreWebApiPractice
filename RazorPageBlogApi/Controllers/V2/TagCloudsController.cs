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
    public class TagCloudsController : ControllerBase
    {
        private readonly RazorPageBlogDbContext _blogDb;

        public TagCloudsController(RazorPageBlogDbContext blogDb)
        {
            this._blogDb = blogDb;
        }

        /// <summary>
        /// 取得所有的Tag
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<TagCloud> Get()
        {
            return _blogDb.TagClouds.ToList();
        }

        /// <summary>
        /// 取得指定的Tag
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:guid}")]
        [HttpGet]
        public TagCloud Get(Guid id)
        {
            return _blogDb.TagClouds.Single(s => s.Id == id);
        }
    }
}
