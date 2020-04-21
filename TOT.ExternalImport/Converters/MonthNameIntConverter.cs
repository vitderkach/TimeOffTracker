using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TOT.DataImport.Converters
{
    public class MonthIntConverter
    {
        private readonly CultureInfo _cultureInfo;
        public MonthIntConverter(string cultureName)
        {
            CultureInfo cultureInfo = new CultureInfo("ru");
            _cultureInfo = cultureInfo;
        }

        public string ConvertFromInt(int monthNumber)
        {
            if (monthNumber < 1 || monthNumber > 12)
            {
                throw new ArgumentException("The argument monthNumber should be smaller than 1 or biiger than 12.");
            }
            return _cultureInfo.DateTimeFormat.GetMonthName(monthNumber);
        }

        public int ConvertFromString(string month)
        {
            int result = Array.FindIndex(_cultureInfo.DateTimeFormat.MonthNames, mn => mn == month) + 1;
            if (result == 0)
            {
                throw new ArgumentException("The argument month should be valid month name.");
            }
            return result;
        }
    }
}
