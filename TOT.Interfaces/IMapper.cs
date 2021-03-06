﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Interfaces
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
        TResult MergeInto<TResult>(object item1, object item2);
    }
}
