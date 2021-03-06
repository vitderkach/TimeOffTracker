﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TOT.Entities;

namespace TOT.Data.Extensions
{
    public static class TymeOffTypeExtensions
    {
        public static string GetDescription(this TimeOffType This)
        {
            var type = typeof(TimeOffType);
            var memInfo = type.GetMember(This.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
