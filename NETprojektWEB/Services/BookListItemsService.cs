using Microsoft.AspNetCore.Hosting;
using NETprojektWEB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NETprojektWEB.Services
{
    public class BookListItemsService
    {
        private readonly IWebHostEnvironment enc;

        private string DataPath
        {
            get
            {
                return enc.ContentRootFileProvider.GetFileInfo("data.json").PhysicalPath;
            }
        }

        public BookListItemsService(IWebHostEnvironment enc)
        {
            this.enc = enc;
        }

        public async Task Add(BookListItemForm form)
        {
            List<BookListItemForm> items = await this.GetItems();

            items.Add(form);

            await this.SaveItems(items);
        }

        public async Task<List<BookListItemForm>> GetItems()
        {
            if (!File.Exists(DataPath))
            {
                return new List<BookListItemForm>();
            }


            string content = await File.ReadAllTextAsync(DataPath);
            List<BookListItemForm> items = JsonSerializer.Deserialize<List<BookListItemForm>>(content);
            return items;
        }

        public async Task SaveItems(List<BookListItemForm> items)
        {
            string content = JsonSerializer.Serialize(items);

            await File.WriteAllTextAsync(DataPath, content);
        }

        public async Task DeleteItem(string text)
        {
            List<BookListItemForm> items = await this.GetItems();
            for(int i = 0; i < items.Count; i++)
            {
                if(items[i].Title == text)
                {
                    items.RemoveAt(i);
                }
            }
            await this.SaveItems(items);

        }
    }
}