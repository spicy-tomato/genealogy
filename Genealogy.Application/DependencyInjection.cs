using FluentValidation;
using Genealogy.Application.UseCases.People.Commands.Create;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Genealogy.Application;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return builder;
    }

    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CreatePersonCommandValidator>();

        return builder;
    }
}