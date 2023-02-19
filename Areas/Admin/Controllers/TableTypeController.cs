using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spice.Data;
using Spice.Models.TableModel;
using Spice.Utility;
using System.Linq;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class TableTypeController : Controller
    {
        private readonly ApplicationDbContext db;

        public TableTypeController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Index() 
        {
            var models =db.TableTypes.ToList();
            return View(models);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TableType model)
        {
            if (ModelState.IsValid)
            {
                db.TableTypes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "TableType");
            }

            return View();  
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var model = db.TableTypes.SingleOrDefault(x=>x.Id==Id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(TableType model)
        {
            if (ModelState.IsValid)
            {
                var type = db.TableTypes.First(x => x.Id == model.Id);
                type.Name = model.Name;
                type.Description = model.Description;
                type.Price = model.Price;
                type.IsBusy = model.IsBusy;
                db.SaveChanges();
                return RedirectToAction("Index", "TableType");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var model = db.TableTypes.SingleOrDefault(x => x.Id == Id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(TableType model)
        {
           
            if (model!=null)
            {
                db.Remove(model);
                db.SaveChanges();
                return RedirectToAction("Index", "TableType");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            return View (db.TableTypes.SingleOrDefault(x=>x.Id== Id));
        }

    }
}
