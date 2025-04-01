using System;
using AutoMapper;
using FluentValidation;
using SparkRoseDigital_Template.Application.Foos.Commands;

namespace SparkRoseDigital_Template.Api.Models;

public class UpdateFooRequest
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public byte[] RowVersion { get; set; }
}

public class UpdateFooRequestValidator : AbstractValidator<UpdateFooRequest>
{
    public UpdateFooRequestValidator()
    {
        RuleFor(x => x.Text).MinimumLength(5);
    }
}

public class UpdateFooRequestProfile : Profile
{
    public UpdateFooRequestProfile()
    {
        CreateMap<UpdateFooRequest, UpdateFooCommand>();
    }
}
