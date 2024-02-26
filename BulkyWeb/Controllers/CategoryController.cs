using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        public IActionResult Index()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == "Invalid")
            {
                ModelState.AddModelError("Name","Category name cannot be 'Invalid'!");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Successfully create category!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFound = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryFound == null)
            {
                return NotFound();
            }
            return View(categoryFound);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Successfully update category!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFound = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryFound == null)
            {
                return NotFound();
            }
            return View(categoryFound);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var categoryFound = _db.Categories.FirstOrDefault(c => c.Id == id);
            _db.Categories.Remove(categoryFound);
            _db.SaveChanges();
            TempData["success"] = "Successfully delete category!";
            return RedirectToAction("Index", "Category");
        }
    }
}