using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]

    [Area("Admin")]
    public class MenuItemController : Controller
    {
        //dependency injection
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        //constructor 
        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Category = _db.Category,
                MenuItem = new Models.MenuItem()
            };
        }
        public async Task<IActionResult> Index()
        {
            var menuItems = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();

            return View(menuItems);
        }

        //GET create
        public IActionResult Create()
        {
            return View(MenuItemVM);
        }

        //POST create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            _db.MenuItem.Add(MenuItemVM.MenuItem);
            await _db.SaveChangesAsync();

            //image saving section 
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if(files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "Images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDb.Image = @"\Images\" + MenuItemVM.MenuItem.Id + extension;
            }
            else
            {
                //no file wa uploaded. Use default instead 
                var uploads = Path.Combine(webRootPath, @"Images\" + SD.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"\Images\" + MenuItemVM.MenuItem.Id + ".png");
                menuItemFromDb.Image = @"\Images\" + MenuItemVM.MenuItem.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        // GET - edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            MenuItemVM.SubCategory = await _db.SubCategory
                .Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId)
                .ToListAsync();

            if(MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }


            return View(MenuItemVM);
        }

        //POST edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {

            if(id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                MenuItemVM.SubCategory = await _db.SubCategory
                    .Where(s => s.CategoryId == MenuItemVM
                    .MenuItem.CategoryId).ToListAsync();

                return View(MenuItemVM);
            }


            //image saving section 
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count > 0)
            {
                //new img has been uploaded
                var uploads = Path.Combine(webRootPath, "Images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //delete the original file
                var imagePath = Path.Combine(webRootPath, menuItemFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDb.Image = @"\Images\" + MenuItemVM.MenuItem.Id + extension_new;
            }

            menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDb.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDb.CategoryId = MenuItemVM.MenuItem.CategoryId;
            menuItemFromDb.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;



            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        //GET details 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            MenuItemVM.SubCategory = await _db.SubCategory
                .Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId)
                .ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }


            return View(MenuItemVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            MenuItemVM.SubCategory = await _db.SubCategory
                .Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId)
                .ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }


            return View(MenuItemVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            MenuItem menuItem = await _db.MenuItem.FindAsync(id);

            if (menuItem != null)
            {
                var imagePath = Path.Combine(webRootPath, menuItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _db.MenuItem.Remove(menuItem);
                await _db.SaveChangesAsync();
                
            }

            return RedirectToAction(nameof(Index));
        }

    }
}