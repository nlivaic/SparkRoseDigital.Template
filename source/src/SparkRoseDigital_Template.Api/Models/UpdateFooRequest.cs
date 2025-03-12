using System;
using AutoMapper;
using FluentValidation;
using SparkRoseDigital_Template.Application.Foos.Commands;

namespace SparkRoseDigital_Template.Api.Models;

public record UpdateFooRequest(Guid Id, string Text);

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
