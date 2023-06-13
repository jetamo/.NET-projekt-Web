using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETprojektWEB.Models
{
    public class BookListItemForm
    {
        [Required(ErrorMessage = "Book title field is requierd")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Author field is requierd")]
        [Display(Name = "Author")]
        public string Author { get; set; }

    }
}
