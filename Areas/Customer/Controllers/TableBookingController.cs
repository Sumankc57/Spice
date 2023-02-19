using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Spice.Data;
using Spice.Models.TableModel;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Spice.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TableBookingController : Controller
    {
        private readonly UserManager<IdentityUser> usermanger;
        private readonly ApplicationDbContext db;

        public TableBookingController( UserManager<IdentityUser> usermanger
                                     , ApplicationDbContext db)
        {
            this.usermanger = usermanger;
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var thisUser =await usermanger.FindByNameAsync(User.Identity.Name);
            var model = db.TablesBooking.Where(x => x.UserId == thisUser.Id);
          
            return View(model);
        }


        [HttpGet]
        public IActionResult CreateBooking()
        {

            var tableState = db.TableTypes.Where(x => x.IsBusy == false);
            ViewBag.DroD = new SelectList(tableState, "Id", "Name");
            if (tableState == null)
            {
                return View("BookingError");
            }
            // var TypeOfTable = db.TableTypes.Where(x => x.IsBusy == false);
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(TableBooking model)
        {
            var tableState = db.TableTypes.Where(x => x.IsBusy == false);
            if (ModelState.IsValid)
            {
                
                if (tableState!=null)
                {
                    var user = await usermanger.FindByNameAsync(User.Identity.Name);
                    model.UserId = user.Id;
                    model.UserName = user.UserName;

                    var tableTypeChange = db.TableTypes.Find(model.TypeID);
                    tableTypeChange.IsBusy = true;

                    db.TablesBooking.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("BookingError");
                }

            }
            if (model.TypeID ==0)
            {
                return View("BookingError");

            }
            return View(model);
        }


       
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var model =db.TablesBooking.Find(Id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(TableBooking model)
        {
            db.TablesBooking.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // this action for show the available tables 
        [HttpGet]
        public IActionResult TabelsAvailable()
        {
           var model= db.TableTypes.Where(x => x.IsBusy == false);
            return View(model);
        }

    }
}
