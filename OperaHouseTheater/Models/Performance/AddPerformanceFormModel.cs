
namespace OperaHouseTheater.Models.Performance
{
    using OperaHouseTheater.Services.Performances.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Performance;

    public class AddPerformanceFormModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = "Title length must be between {2} & {1} symbols.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(ComposerNameMaxLength,
            MinimumLength = ComposerNameMinLength,
            ErrorMessage = "Composer name must be between {2} & {1} symbols.")]
        public string Composer { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(SynopsisMaxLength,
            MinimumLength = SynopsisMinLength,
            ErrorMessage = "The synopsis must be at least {2} symbols.")]
        public string Synopsis { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Url]
        public string ImageUrl { get; set; }

        [Display(Name = "Тип")]
        public int PerformanceTypeId { get; set; }

        public IEnumerable<PerformanceTypeServiceModel> PerformanceTypes { get; set; }
    }
}
