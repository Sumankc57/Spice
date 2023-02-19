using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spice.Data;
using Spice.Models.TableModel;
using Spice.Utility;
using Stripe;
using System.Linq;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class TableBookingController : Controller
    {
        private readonly ApplicationDbContext db;

        public TableBookingController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Pendings()
        {
            var model =db.TablesBooking.Where(x => x.Status == "pending");
            return View(model);
        }
        [HttpGet]
        public IActionResult CheckedList()
        {
            var model = db.TablesBooking.Where(x => x.Status == "Checked");
            return View(model);
        }
        [HttpGet]
        public IActionResult DoneList()
        {
            var model = db.TablesBooking.Where(x => x.Status == "Done");
            return View(model);
        }
        [HttpGet]
        public IActionResult FinishBooking(int id)
        {
            var model = db.TablesBooking.Find(id);
            return View(model); 
        }
        [HttpPost]
        public IActionResult FinishBooking(TableBooking model)
        {
           var modelDb= db.TablesBooking.Find(model.Id);
            modelDb.Status = "Done";
            db.TablesBooking.Update(modelDb);
           var myType= db.TableTypes.Find(modelDb.TypeID);
            myType.IsBusy = false;
            db.TableTypes.Update(myType);
            db.SaveChanges();
            return View("Home");
        }
        [HttpGet]
        public IActionResult CancelBooking(int Id)
        {
            var model = db.TablesBooking.Find(Id);
            return View(model);
        }

        [HttpPost]
        public IActionResult CancelBooking(TableBooking model)
        {
        
            db.TablesBooking.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Home");
        }
        [HttpGet]
        public IActionResult CheckedBooking(int Id)
        {
            var model = db.TablesBooking.Find(Id);
            return View(model);
        }




        [HttpPost]
        public IActionResult CheckedBooking(TableBooking model )
        {
            var modelDb=db.TablesBooking.Find(model.Id);
            modelDb.Status = "Checked";
            db.TablesBooking.Update(modelDb);
            db.SaveChanges();
            return View("Home", "TableBooking");
        }
    }
}
