using System.ComponentModel.DataAnnotations;

namespace PhoneOrdersApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
    public String Note { get; set; }
    }
}
