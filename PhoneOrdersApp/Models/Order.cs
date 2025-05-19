using System.ComponentModel.DataAnnotations;

namespace PhoneOrdersApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();


        [Required]
        public int CreatedByEmployeeId { get; set; }
        public Employee CreatedByEmployee { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

    }
}
