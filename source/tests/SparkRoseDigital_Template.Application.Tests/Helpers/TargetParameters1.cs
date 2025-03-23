using System.Collections.Generic;
using SparkRoseDigital_Template.Application.Sorting.Models;

namespace SparkRoseDigital_Template.Application.Tests.Helpers;

public class TargetParameters1
    : BaseSortable<MappingTargetModel1>
{
    public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
}
