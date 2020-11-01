using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using GoFlex.Core.Entities;

namespace GoFlex.Web.ViewModels
{
    public class EventEditViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(128, ErrorMessage = "Max length of this field is 128 characters")]
        public string Name { get; set; }

        [MaxLength(1024, ErrorMessage = "Max length of this field is 1024 characters")]
        public string Description { get; set; }

        [DisplayName("Date of the event")]
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Time")]
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        public string Photo { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [DisplayName("Location")]
        public int LocationId { get; set; }

        public Guid OrganizerId { get; set; }

        public EventPriceViewModel CurrentPrice { get; set; }

        public IEnumerable<EventPriceViewModel> Prices { get; set; }
        public IEnumerable<EventCategory> Categories { get; set; }
        public IEnumerable<Location> Locations { get; set; }
    }
}
