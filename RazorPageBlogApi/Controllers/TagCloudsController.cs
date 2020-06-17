using Microsoft.AspNetCore.Mvc;
using RazorPageBlogApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorPageBlogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagCloudsController : ControllerBase
    {
        private readonly RazorPageBlogDbContext _blogDb;

        public TagCloudsController(RazorPageBlogDbContext blogDb)
        {
            this._blogDb = blogDb;
        }

        [HttpGet]
        public IEnumerable<TagCloud> Get()
        {
            return _blogDb.TagClouds.ToList();
        }

        [Route("{id:guid}")]
        [HttpGet]
        public TagCloud Get(Guid id)
        {
            return _blogDb.TagClouds.Single(s => s.Id == id);
        }
    }
}
