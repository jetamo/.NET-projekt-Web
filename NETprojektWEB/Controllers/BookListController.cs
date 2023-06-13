using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NETprojektWEB.Models;
using NETprojektWEB.Services;

namespace NETprojektWEB.Controllers
{
    public class BookListController : Controller
    {
        private int indexToEdit;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!this.HttpContext.Session.TryGetValue("username", out byte[] data))
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { });
            }
            base.OnActionExecuting(context);
        }

        private readonly BookListItemsService itemsService;

        public BookListController(BookListItemsService itemService)
        {
            this.itemsService = itemService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Items = await this.itemsService.GetItems();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookListItemForm form)
        {
            if (ModelState.IsValid)
            {
                await this.itemsService.Add(form);

                return RedirectToAction("Index");
            }
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteItemForm form)
        {
            await this.itemsService.DeleteItem(form.Text); 
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DeleteItemForm form)
        {
            List<BookListItemForm> books = await this.itemsService.GetItems();
            for(int i = 0; i < books.Count; i++)
            {
                if(books[i].Title == form.Text)
                {
                    TempData["IndexToEdit"] = i;
                    this.indexToEdit = i;
                    byte[] intBytes = BitConverter.GetBytes(i);
                    Array.Reverse(intBytes);
                    byte[] result = intBytes;
                    HttpContext.Session.Set("IndexToEdit", result);

                }
            }
            return RedirectToAction("EditBook");
        }

        public async Task<IActionResult> EditBook()
        {
            ViewBag.Items = await this.itemsService.GetItems();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(BookListItemForm form)
        {
            List<BookListItemForm> items = await this.itemsService.GetItems();
            byte[] index;
            HttpContext.Session.TryGetValue("IndexToEdit", out index);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(index);

            int i = BitConverter.ToInt32(index, 0);
            items[i] = form;
            this.itemsService.SaveItems(items);
            return RedirectToAction("Index");
        }
    }
}
