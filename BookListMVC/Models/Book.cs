using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BookListMVC.Models
{
    public class Book
    {
        [Key]
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        //[Display(Name = "Name")]
        public string Name { get; set; }
        //[Display(Name = "Author")]
        public string Author { get; set; }
        //[Display(Name = "ISBN")]
        public string ISBN { get; set; }
    }
    public class BooksViewModel
    {
        public IEnumerable<Book> Books { get; set; }
    }
}
