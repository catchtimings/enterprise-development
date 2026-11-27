using FoodDelivery.Generator.Application;
using FoodDelivery.Generator.Domain;
using FoodDelivery.Generator.Infrastructure;
using FoodDelivery.Generator.Presentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FoodDelivery.Generator;

/// <summary>
/// Расширения для регистрации компонентов генератора
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует службы генератора контрактов
    /// </summary>
    /// <typeparam name="TProducer">Реализация IProducerService</typeparam>
    /// <param name="services">Коллекция служб</param>
    /// <returns>Коллекция служб</returns>
    public static IServiceCollection AddGeneratorServices<TProducer>(this IServiceCollection services)
        where TProducer : class, IProducerService
    {
        services.AddSingleton<BogusOrderGenerator>();
        services.AddScoped<IProducerService, TProducer>();
        services.AddHostedService<OrderGeneratorService>();
        services.AddControllers().AddApplicationPart(typeof(OrdersGeneratorController).Assembly);
        return services;
    }

    /// <summary>
    /// Настраивает конвейер обработки запросов генератора
    /// </summary>
    /// <param name="app">Веб-приложение</param>
    /// <returns>Веб-приложение</returns>
    public static WebApplication UseGeneratorServices(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}