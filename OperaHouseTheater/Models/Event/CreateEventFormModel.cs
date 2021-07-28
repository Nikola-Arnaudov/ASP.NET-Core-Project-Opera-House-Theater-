namespace OperaHouseTheater.Models.Event
{
    using OperaHouseTheater.Models.Performance;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class CreateEventFormModel
    {
        [Display(Name ="Заглавие")]
        [Required(ErrorMessage ="This field is required.")]
        public int PerformanceId { get; set; }

        [Required(ErrorMessage ="This field is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage ="This field is required.")]
        public int TicketPrice { get; set; }

        public IEnumerable<PerformanceTitleViewModel> PerformanceTitles { get; set; }

    }
}
