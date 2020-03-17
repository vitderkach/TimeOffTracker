using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Utility.AutoMapper.Exstensions
{
    public static class AutoMapperExtensions
    {
        public static TResult MergeInto<TResult>(this IMapper mapper, object item1, object item2)
        {
            return mapper.Map(item2, mapper.Map<TResult>(item1));
        }
    }
}
