using Microsoft.EntityFrameworkCore;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Domain;

namespace FoodDelivery.Infrastructure.EFCore;

/// <summary>
/// Контекст базы данных
/// </summary>
/// <param name="options">Опции контекста базы данных</param>
public class FoodDeliveryDbContext(DbContextOptions options) : DbContext(options)
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

            entity.HasData(DataSeeder.Clients);
        });

        // Конфигурация сущности Dish в базе данных
        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Id).IsRequired();
            entity.Property(d => d.Name).HasMaxLength(200).IsRequired();

            entity.HasOne<DishCategory>()
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .IsRequired();

            entity.HasData(DataSeeder.Dishes);
        });

        // Конфигурация сущности DishCategory в базе данных
        modelBuilder.Entity<DishCategory>(entity =>
        {
            entity.HasKey(dc => dc.Id);

            entity.Property(dc => dc.Id).IsRequired();
            entity.Property(dc => dc.Name).HasMaxLength(100).IsRequired();

            entity.HasData(DataSeeder.Categories);
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

            entity.HasOne<Client>()
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .IsRequired();

            entity.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(o => o.RestaurantId)
                .IsRequired();

            entity.HasData(DataSeeder.Orders);
        });

        // Конфигурация сущности Restaurant в базе данных
        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id).IsRequired();
            entity.Property(r => r.Name).HasMaxLength(200).IsRequired();
            entity.Property(r => r.Address).HasMaxLength(300).IsRequired();

            entity.HasData(DataSeeder.Restaurants);
        });
    }
}
