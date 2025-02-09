using BPNCart.Application.Behaviors;
using BPNCart.Application.Commands;
using BPNCart.Application.ExternalServices;
using BPNCart.Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BPNCart.Application;
public static class ServiceRegistration
{
    public static void AddApplicationService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(cfg => {
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingAndLogging<,>));
            cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly);
        });

        serviceCollection.AddScoped<IValidator<AddProductCommand>, AddProductCommandValidator>();
    }
}
