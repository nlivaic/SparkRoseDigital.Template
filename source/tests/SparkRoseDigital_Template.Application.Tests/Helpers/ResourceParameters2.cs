using System.Collections.Generic;
using SparkRoseDigital_Template.Application.Sorting.Models;

namespace SparkRoseDigital_Template.Application.Tests.Helpers;

public class ResourceParameters2
    : BaseSortable<MappingSourceModel2>
{
    public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
}
