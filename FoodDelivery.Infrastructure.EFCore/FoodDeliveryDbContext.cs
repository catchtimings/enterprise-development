using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.EFCore;

/// <summary>
/// Контекст базы данных
/// </summary>
/// <param name="options">Опции контекста базы данных</param>
public class FoodDeliveryDbContext(DbContextOptions<FoodDeliveryDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция клиентов в базе данных
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Коллекция блюд в базе данных
    /// </summary>
    public DbSet<Dish> Dishes { get; set; }

    /// <summary>
    /// Коллекция категорий блюд в базе данных
    /// </summary>
    public DbSet<DishCategory> DishCategories { get; set; }

    /// <summary>
    /// Коллекция позиций заказов в базе данных
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }

    /// <summary>
    /// Коллекция заказов в базе данных
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Коллекция ресторанов в базе данных
    /// </summary>
    public DbSet<Restaurant> Restaurants { get; set; }

    /// <summary>
    /// Конфигурация модели базы данных
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // на всякий пожарный

        // Конфигурация сущности Client в базе данных 
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id).IsRequired();
            entity.Property(c => c.FullName).HasMaxLength(200).IsRequired();
            entity.Property(c => c.PhoneNumber).HasMaxLength(30).IsRequired();
            entity.Property(c => c.DeliveryAddress).HasMaxLength(300).IsRequired();

            entity.HasData(DataSeeder.Clients.Select(c => new
            {
                c.Id,
                c.FullName,
                c.PhoneNumber,
                c.DeliveryAddress
            }));
        });

        // Конфигурация сущности Dish в базе данных
        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Id).IsRequired();
            entity.Property(d => d.Name).HasMaxLength(200).IsRequired();

            entity.HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .IsRequired();

            entity.HasData(DataSeeder.Dishes.Select(d => new
            {
                d.Id,
                d.Name,
                d.Weight,
                d.Price,
                d.CategoryId
            }));
        });

        // Конфигурация сущности DishCategory в базе данных
        modelBuilder.Entity<DishCategory>(entity =>
        {
            entity.HasKey(dc => dc.Id);

            entity.Property(dc => dc.Id).IsRequired();
            entity.Property(dc => dc.Name).HasMaxLength(100).IsRequired();

            entity.HasData(DataSeeder.Categories.Select(c => new
            {
                c.Id,
                c.Name
            }));
        });

        // Конфигурация сущности Order в базе данных
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Id).IsRequired();
            entity.Property(o => o.ClientId).IsRequired();
            entity.Property(o => o.RestaurantId).IsRequired();
            entity.Property(o => o.CreatedAt).IsRequired();
            entity.Property(o => o.TotalAmount).IsRequired();

            entity.HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .IsRequired();

            entity.HasOne(o => o.Restaurant)
                .WithMany()
                .HasForeignKey(o => o.RestaurantId)
                .IsRequired();

            entity.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(DataSeeder.Orders.Select(o => new
            {
                o.Id,
                o.ClientId,
                o.RestaurantId,
                o.CreatedAt,
                o.DeliveredAt,
                o.TotalAmount
            }));
        });

        // Конфигурация сущности Restaurant в базе данных
        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id).IsRequired();
            entity.Property(r => r.Name).HasMaxLength(200).IsRequired();
            entity.Property(r => r.Address).HasMaxLength(300).IsRequired();

            entity.HasData(DataSeeder.Restaurants.Select(r => new
            {
                r.Id,
                r.Name,
                r.Address,
                r.Rating,
                r.OpeningTime,
                r.ClosingTime
            }));
        });

        // Конфигурация сущности OrderItem в базе данных
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).IsRequired();
            entity.Property(i => i.OrderId).IsRequired();
            entity.Property(i => i.DishId).IsRequired();
            entity.Property(i => i.Quantity).IsRequired();
            entity.Property(i => i.PriceAtOrder).HasPrecision(18, 2).IsRequired();

            entity.HasOne(i => i.Dish)
                .WithMany()
                .HasForeignKey(i => i.DishId)
                .IsRequired();

            entity.HasData(DataSeeder.Orders
                .SelectMany(o => o.Items)
                .Select(i => new
                {
                    i.Id,
                    i.OrderId,
                    i.DishId,
                    i.Quantity,
                    i.PriceAtOrder
                }));
        });
    }
}