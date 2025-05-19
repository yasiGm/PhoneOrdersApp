using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PhoneOrdersApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        [Precision(18, 2)]
        public Decimal Price { get; set; }
    }
}
