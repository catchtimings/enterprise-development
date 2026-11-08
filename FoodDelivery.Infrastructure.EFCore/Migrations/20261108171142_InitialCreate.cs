using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDelivery.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    full_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    delivery_address = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dish_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dish_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "restaurants",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rating = table.Column<double>(type: "double", nullable: false),
                    opening_time = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    closing_time = table.Column<TimeOnly>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restaurants", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dishes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    weight = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    CategoryId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dishes", x => x.id);
                    table.ForeignKey(
                        name: "FK_dishes_dish_categories_CategoryId1",
                        column: x => x.CategoryId1,
                        principalTable: "dish_categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_dishes_dish_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "dish_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    ClientId1 = table.Column<int>(type: "int", nullable: true),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    RestaurantId1 = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    delivered_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    total_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_clients_ClientId1",
                        column: x => x.ClientId1,
                        principalTable: "clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_orders_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_restaurants_RestaurantId1",
                        column: x => x.RestaurantId1,
                        principalTable: "restaurants",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_orders_restaurants_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "restaurants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    dish_id = table.Column<int>(type: "int", nullable: false),
                    DishId1 = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price_at_order = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_items_dishes_DishId1",
                        column: x => x.DishId1,
                        principalTable: "dishes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_items_dishes_dish_id",
                        column: x => x.dish_id,
                        principalTable: "dishes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "id", "delivery_address", "full_name", "phone_number" },
                values: new object[,]
                {
                    { 1, "Ул. Пушкина, 1, д. Колотушкина 11, кв. 101", "Аннова Анна Ивановна", "+79990000000" },
                    { 2, "Ул. Пушкина, 2, д. Колотушкина 12, кв. 102", "Алексеев Алексей Алексеевич", "+79990000001" },
                    { 3, "Ул. Пушкина, 3, д. Колотушкина 13, кв. 103", "Васильев Василий Васильевич", "+79990000002" },
                    { 4, "Ул. Пушкина, 4, д. Колотушкина 14, кв. 104", "Дмитриев Дмитрий Дмитриевич", "+79990000003" },
                    { 5, "Ул. Пушкина, 5, д. Колотушкина 15, кв. 105", "Иванов Иван Иванович", "+79990000004" },
                    { 6, "Ул. Пушкина, 6, д. Колотушкина 16, кв. 106", "Ольгина Ольга Олеговна", "+79990000005" },
                    { 7, "Ул. Пушкина, 7, д. Колотушкина 17, кв. 107", "Павлов Павел Павлович", "+79990000006" },
                    { 8, "Ул. Пушкина, 8, д. Колотушкина 18, кв. 108", "Петров Петр Петрович", "+79990000007" },
                    { 9, "Ул. Пушкина, 9, д. Колотушкина 19, кв. 109", "Сергеев Сергей Сергеевич", "+79990000008" },
                    { 10, "Ул. Пушкина, 10, д. Колотушкина 20, кв. 110", "Сидоров Сидор Сидорович", "+79990000009" },
                    { 11, "Ул. Пушкина, 11, д. Колотушкина 21, кв. 111", "Смирнов Станислав Сергеевич", "+79990000010" }
                });

            migrationBuilder.InsertData(
                table: "dish_categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Пицца" },
                    { 2, "Суши" },
                    { 3, "Бургеры" },
                    { 4, "Супы" },
                    { 5, "Салаты" },
                    { 6, "Десерты" },
                    { 7, "Напитки" },
                    { 8, "Закуски" },
                    { 9, "Паста" },
                    { 10, "Гриль" }
                });

            migrationBuilder.InsertData(
                table: "restaurants",
                columns: new[] { "id", "address", "closing_time", "name", "opening_time", "rating" },
                values: new object[,]
                {
                    { 1, "Ул. Ленина, д. 1", new TimeOnly(23, 0, 0), "Ресторан №1", new TimeOnly(9, 0, 0), 4.2000000000000002 },
                    { 2, "Ул. Ленина, д. 2", new TimeOnly(23, 30, 0), "Ресторан №2", new TimeOnly(9, 30, 0), 4.4000000000000004 },
                    { 3, "Ул. Ленина, д. 3", new TimeOnly(23, 0, 0), "Ресторан №3", new TimeOnly(9, 0, 0), 4.5999999999999996 },
                    { 4, "Ул. Ленина, д. 4", new TimeOnly(23, 30, 0), "Ресторан №4", new TimeOnly(9, 30, 0), 4.7999999999999998 },
                    { 5, "Ул. Ленина, д. 5", new TimeOnly(23, 0, 0), "Ресторан №5", new TimeOnly(9, 0, 0), 4.0 },
                    { 6, "Ул. Ленина, д. 6", new TimeOnly(23, 30, 0), "Ресторан №6", new TimeOnly(9, 30, 0), 4.2000000000000002 },
                    { 7, "Ул. Ленина, д. 7", new TimeOnly(23, 0, 0), "Ресторан №7", new TimeOnly(9, 0, 0), 4.4000000000000004 },
                    { 8, "Ул. Ленина, д. 8", new TimeOnly(23, 30, 0), "Ресторан №8", new TimeOnly(9, 30, 0), 4.5999999999999996 },
                    { 9, "Ул. Ленина, д. 9", new TimeOnly(23, 0, 0), "Ресторан №9", new TimeOnly(9, 0, 0), 4.7999999999999998 },
                    { 10, "Ул. Ленина, д. 10", new TimeOnly(23, 30, 0), "Ресторан №10", new TimeOnly(9, 30, 0), 4.0 }
                });

            migrationBuilder.InsertData(
                table: "dishes",
                columns: new[] { "id", "category_id", "CategoryId1", "name", "price", "weight" },
                values: new object[,]
                {
                    { 1, 1, null, "Пепперони", 150m, 200 },
                    { 2, 2, null, "Филадельфия", 250m, 250 },
                    { 3, 3, null, "Чизбургер", 350m, 300 },
                    { 4, 4, null, "Борщ", 450m, 350 },
                    { 5, 5, null, "Цезарь", 550m, 400 },
                    { 6, 6, null, "Чизкейк", 650m, 450 },
                    { 7, 7, null, "Кола", 750m, 500 },
                    { 8, 8, null, "Картофель фри", 850m, 550 },
                    { 9, 9, null, "Карбонара", 950m, 600 },
                    { 10, 10, null, "Стейк рибай", 1050m, 650 }
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "client_id", "ClientId1", "created_at", "delivered_at", "restaurant_id", "RestaurantId1", "total_amount" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2026, 9, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 14, 15, 0, 0, DateTimeKind.Unspecified), 1, null, 150m },
                    { 2, 2, null, new DateTime(2026, 9, 10, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 16, 15, 0, 0, DateTimeKind.Unspecified), 2, null, 500m },
                    { 3, 3, null, new DateTime(2026, 9, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 18, 35, 0, 0, DateTimeKind.Unspecified), 3, null, 1050m },
                    { 4, 4, null, new DateTime(2026, 9, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 20, 40, 0, 0, DateTimeKind.Unspecified), 4, null, 1800m },
                    { 5, 5, null, new DateTime(2026, 9, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 14, 45, 0, 0, DateTimeKind.Unspecified), 5, null, 2750m },
                    { 6, 6, null, new DateTime(2026, 9, 11, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 16, 50, 0, 0, DateTimeKind.Unspecified), 6, null, 3900m },
                    { 7, 7, null, new DateTime(2026, 9, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 18, 55, 0, 0, DateTimeKind.Unspecified), 7, null, 5250m },
                    { 8, 8, null, new DateTime(2026, 9, 11, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 21, 0, 0, 0, DateTimeKind.Unspecified), 8, null, 6800m },
                    { 9, 9, null, new DateTime(2026, 9, 12, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 15, 5, 0, 0, DateTimeKind.Unspecified), 9, null, 8550m },
                    { 10, 10, null, new DateTime(2026, 9, 12, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 17, 10, 0, 0, DateTimeKind.Unspecified), 10, null, 10500m },
                    { 11, 11, null, new DateTime(2026, 9, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 19, 10, 0, 0, DateTimeKind.Unspecified), 10, null, 10500m }
                });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "dish_id", "DishId1", "order_id", "price_at_order", "quantity" },
                values: new object[,]
                {
                    { 1, 1, null, 1, 150m, 1 },
                    { 2, 2, null, 2, 250m, 2 },
                    { 3, 3, null, 3, 350m, 3 },
                    { 4, 4, null, 4, 450m, 4 },
                    { 5, 5, null, 5, 550m, 5 },
                    { 6, 6, null, 6, 650m, 6 },
                    { 7, 7, null, 7, 750m, 7 },
                    { 8, 8, null, 8, 850m, 8 },
                    { 9, 9, null, 9, 950m, 9 },
                    { 10, 10, null, 10, 1050m, 10 },
                    { 11, 10, null, 11, 1050m, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_dishes_category_id",
                table: "dishes",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_dishes_CategoryId1",
                table: "dishes",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_dish_id",
                table: "order_items",
                column: "dish_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_DishId1",
                table: "order_items",
                column: "DishId1");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_client_id",
                table: "orders",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_ClientId1",
                table: "orders",
                column: "ClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_orders_restaurant_id",
                table: "orders",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_RestaurantId1",
                table: "orders",
                column: "RestaurantId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "dishes");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "dish_categories");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "restaurants");
        }
    }
}
