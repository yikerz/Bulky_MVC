using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Add(Category dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                DisplayOrder = dto.DisplayOrder
            };
            _db.Categories.Add(category);
            _db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("get")]
        public IActionResult Get()
        {
            var category = _db.Categories.ToList();
            return Ok(category);
        }
    }
}