using System.Collections.Generic;
using SparkRoseDigital_Template.Application.Sorting.Models;

namespace SparkRoseDigital_Template.Application.Tests.Helpers;

public class ResourceParameters1
    : BaseSortable<MappingSourceModel1>
{
    public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
}
