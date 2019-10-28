using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TOT.Utility.AutoMapper
{
    public class TOTAutoMapper : TOT.Interfaces.IMapper
    {
        private readonly IMapper mapper;
        public TOTAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainTo>();
                cfg.AddProfile<DTOTo>();
            }) ;
            mapper = config.CreateMapper();
        }
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}
