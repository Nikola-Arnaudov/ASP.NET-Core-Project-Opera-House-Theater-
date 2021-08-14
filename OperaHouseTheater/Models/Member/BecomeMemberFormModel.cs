namespace OperaHouseTheater.Models.Member
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static Data.DataConstants;

    public class BecomeMemberFormModel
    {
        [Required]
        [StringLength(MemberNameMaxLength,MinimumLength = MemberNameMinLength)]
        [Display(Name = "Name")]
        public string MemberName { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength,MinimumLength = PhoneNumberMinLength,ErrorMessage = "The phone number must start with zero and be exactly 10 digits long.")]
        [RegularExpression("^[0][0-9]{9}$" ,ErrorMessage ="The phone number must be in that format: 0888888888")]
        public string PhoneNumber { get; set; }

        public string   Message { get; set; }
    }
}
