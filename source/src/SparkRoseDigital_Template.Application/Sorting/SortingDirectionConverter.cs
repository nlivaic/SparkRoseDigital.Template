using System;
using System.ComponentModel;
using SparkRoseDigital_Template.Application.Sorting.Models;
using SparkRoseDigital_Template.Common.Extensions;

namespace SparkRoseDigital_Template.Application.Sorting;

public class SortingDirectionConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    {
        if (value is string source)
        {
            var orderByCriteriaWithDirection = source.Split(' ');
            var orderByDirection = SortDirection.Asc;     // Default value.
            if (orderByCriteriaWithDirection.Length > 1
                && !Enum.TryParse(
                    orderByCriteriaWithDirection[1].CapitalizeFirstLetter(),
                    out orderByDirection))
            {
                throw new InvalidPropertyMappingException($"Unknown ordering direction: {orderByCriteriaWithDirection[1]}");
            }
            return new SortCriteria
            {
                SortByCriteria = orderByCriteriaWithDirection[0],
                SortDirection = orderByDirection
            };
        }
        else
        {
            return base.ConvertFrom(context, culture, value);
        }
    }
}
