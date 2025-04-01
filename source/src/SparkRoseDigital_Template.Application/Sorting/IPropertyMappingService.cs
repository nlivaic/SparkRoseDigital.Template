using System.Collections.Generic;
using SparkRoseDigital_Template.Application.Sorting.Models;

namespace SparkRoseDigital_Template.Application.Sorting;

public interface IPropertyMappingService
{
    IEnumerable<SortCriteria> Resolve(BaseSortable sortableSource, BaseSortable sortableTarget);
}
