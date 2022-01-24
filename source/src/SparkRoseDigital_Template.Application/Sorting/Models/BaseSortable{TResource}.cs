using System;

namespace SparkRoseDigital_Template.Application.Sorting.Models
{
    public abstract class BaseSortable<TResource> : BaseSortable
    {
        public override Type ResourceType { get; } = typeof(TResource);
    }
}
