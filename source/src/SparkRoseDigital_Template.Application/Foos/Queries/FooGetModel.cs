using System;
using AutoMapper;
using SparkRoseDigital_Template.Core.Entities;

namespace SparkRoseDigital_Template.Application.Foos.Queries;

public class FooGetModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public byte[] RowVersion { get; set; }

    public class FooGetModelProfile : Profile
    {
        public FooGetModelProfile()
        {
            CreateMap<Foo, FooGetModel>();
        }
    }
}
