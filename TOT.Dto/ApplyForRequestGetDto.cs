using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class ApplyForRequestGetDto {
        public ApplyForRequestGetDto()
        {
            RequiredManagers = new List<int>();
            ExsistingManagers = new Dictionary<string, string>();
        }
        public int Id { get; set; }

        [Display(Name = "Vacation type")]
        [Required(ErrorMessage = "Required")]
        public TimeOffType  TimeOffType { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Vacation types")]
        public string SelectedTimeOffType { get; set; }

        [Display(Name = "Vacation types")]
        public SelectList VacationTypes { 
            get
            {
                var type = typeof(TimeOffType);
                var enumNames = type.GetEnumValues().Cast<TimeOffType>();
                ICollection<SelectListItem> res = new Collection<SelectListItem>();
                foreach (var name in enumNames)
                {
                    res.Add(new SelectListItem()
                    {
                        Text = name.GetDescription(),
                        Value = name.ToString()
                    });
                }
                SelectList list = new SelectList(res, "Value", "Text");
                return list;
            } 
        }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Managers")]
        [Required(ErrorMessage = "Required")]
        public IEnumerable<int> RequiredManagers { get; set; }
        public IEnumerable<KeyValuePair<string, string>> ExsistingManagers { get; set; }
        public int UserId { get; set; }
    }
    public static class ExtensionMethods
    {
        static public string GetDescription(this TimeOffType This)
        {
            var type = typeof(TimeOffType);
            var memInfo = type.GetMember(This.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
