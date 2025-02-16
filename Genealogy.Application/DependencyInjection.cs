﻿using FluentValidation;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Application.Services.Implementations;
using Genealogy.Application.UseCases.People.Create;
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

    public static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CreatePersonCommandValidator>();
        builder.Services.AddScoped(typeof(IRequestValidator<>), typeof(RequestValidator<>));

        return builder;
    }
}