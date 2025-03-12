using AutoMapper;
using FluentValidation;
using SparkRoseDigital_Template.Application.Foos.Commands;

namespace SparkRoseDigital_Template.Api.Models;

public record CreateFooRequest(string Text);

public class CreateFooRequestValidator : AbstractValidator<CreateFooRequest>
{
    public CreateFooRequestValidator()
    {
        RuleFor(x => x.Text).MinimumLength(5);
    }
}

public class CreateFooRequestProfile : Profile
{
    public CreateFooRequestProfile()
    {
        CreateMap<CreateFooRequest, CreateFooCommand>();
    }
}
