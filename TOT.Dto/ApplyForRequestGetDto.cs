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
    public class ApplyForRequestGetDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string SelectedTimeOffType { get; set; }
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
        public string Notes { get; set; }
        public IList<int> SelectedManager { get; set; }
        public SelectList MyManagers { get; set; }
        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }
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
