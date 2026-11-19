using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class RestrictDishDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dishes_dish_categories_CategoryId1",
                table: "dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_dishes_DishId1",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_dishes_dish_id",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_clients_ClientId1",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_restaurants_RestaurantId1",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_ClientId1",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_RestaurantId1",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_order_items_DishId1",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "IX_dishes_CategoryId1",
                table: "dishes");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "RestaurantId1",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DishId1",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "dishes");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_dishes_dish_id",
                table: "order_items",
                column: "dish_id",
                principalTable: "dishes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_items_dishes_dish_id",
                table: "order_items");

            migrationBuilder.AddColumn<int>(
                name: "ClientId1",
                table: "orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId1",
                table: "orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DishId1",
                table: "order_items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "dishes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 1,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 2,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 3,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 4,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 5,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 6,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 7,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 8,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 9,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "dishes",
                keyColumn: "id",
                keyValue: 10,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 1,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 2,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 3,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 4,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 5,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 6,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 7,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 8,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 9,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 10,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 11,
                column: "DishId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "ClientId1", "RestaurantId1" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_orders_ClientId1",
                table: "orders",
                column: "ClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_orders_RestaurantId1",
                table: "orders",
                column: "RestaurantId1");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_DishId1",
                table: "order_items",
                column: "DishId1");

            migrationBuilder.CreateIndex(
                name: "IX_dishes_CategoryId1",
                table: "dishes",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_dishes_dish_categories_CategoryId1",
                table: "dishes",
                column: "CategoryId1",
                principalTable: "dish_categories",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_dishes_DishId1",
                table: "order_items",
                column: "DishId1",
                principalTable: "dishes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_dishes_dish_id",
                table: "order_items",
                column: "dish_id",
                principalTable: "dishes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_clients_ClientId1",
                table: "orders",
                column: "ClientId1",
                principalTable: "clients",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_restaurants_RestaurantId1",
                table: "orders",
                column: "RestaurantId1",
                principalTable: "restaurants",
                principalColumn: "id");
        }
    }
}
