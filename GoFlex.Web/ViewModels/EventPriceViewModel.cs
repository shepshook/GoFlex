using System.ComponentModel.DataAnnotations;

namespace GoFlex.Web.ViewModels
{
    public class EventPriceViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Count is required")]
        [Range(1, int.MaxValue)]
        public int Total { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        [Range(0, 1_000_000)]
        public decimal Price { get; set; }

        public bool IsRemoved { get; set; }
    }
}
