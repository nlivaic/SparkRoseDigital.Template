using System.Collections.Generic;
using AutoMapper;
using SparkRoseDigital_Template.Application.Sorting.Models;

namespace SparkRoseDigital_Template.Application.Sorting
{
    public class SortCriteriaResolver<TSource, TTarget>
        : IValueResolver<TSource, TTarget, IEnumerable<SortCriteria>>
        where TSource : BaseSortable
        where TTarget : BaseSortable
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public SortCriteriaResolver(IPropertyMappingService propertyMappingService)
        {
            _propertyMappingService = propertyMappingService;
        }

        public IEnumerable<SortCriteria> Resolve(
            TSource source,
            TTarget target,
            IEnumerable<SortCriteria> destMember,
            ResolutionContext context) =>
            _propertyMappingService.Resolve(source, target);
    }
}
