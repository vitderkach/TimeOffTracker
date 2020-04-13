using AutoMapper;
using AutoMapper.EquivalencyExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TOT.Utility.AutoMapper.Exstensions;

namespace TOT.Utility.AutoMapper
{
    public class TOTAutoMapper : TOT.Interfaces.IMapper
    {
        private readonly IMapper mapper;
        public TOTAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<DomainTo>();
                cfg.AddProfile<DTOTo>();
                cfg.AddProfile<DtoToDto>();
            }) ;
            mapper = config.CreateMapper();
        }
        public TDestination Map<TSource, TDestination>(TSource source)
            => mapper.Map<TSource, TDestination>(source);


        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
            => mapper.Map<TSource, TDestination>(source, destination);

        public TResult MergeInto<TResult>(object item1, object item2)
            => mapper.MergeInto<TResult>(item1, item2);
    }
}
