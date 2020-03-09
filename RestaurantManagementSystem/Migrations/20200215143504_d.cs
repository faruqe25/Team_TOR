using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagementSystem.Migrations
{
    public partial class d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomersId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomersName = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    PaymentMobileNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomersId);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    IngredientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.IngredientId);
                });

            migrationBuilder.CreateTable(
                name: "MealHour",
                columns: table => new
                {
                    MealHourId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealHourTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealHour", x => x.MealHourId);
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    OfferId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Coupon = table.Column<string>(nullable: true),
                    ValidatyStart = table.Column<DateTime>(nullable: false),
                    ValidatyTo = table.Column<DateTime>(nullable: false),
                    Discount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.OfferId);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    TableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableNumber = table.Column<string>(nullable: true),
                    TableCapacity = table.Column<int>(nullable: false),
                    BookingPrice = table.Column<int>(nullable: false),
                    BookedStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.TableId);
                });

            migrationBuilder.CreateTable(
                name: "StockDetails",
                columns: table => new
                {
                    StockDetailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    StockInDate = table.Column<DateTime>(nullable: false),
                    AvailableStock = table.Column<DateTime>(nullable: false),
                    IngredientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDetails", x => x.StockDetailsId);
                    table.ForeignKey(
                        name: "FK_StockDetails_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    FoodItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    MealHourId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.FoodItemId);
                    table.ForeignKey(
                        name: "FK_FoodItems_MealHour_MealHourId",
                        column: x => x.MealHourId,
                        principalTable: "MealHour",
                        principalColumn: "MealHourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderedTable",
                columns: table => new
                {
                    CustomerOrderedTableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookTimeFrom = table.Column<DateTime>(nullable: false),
                    BookTimeTo = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ConfirmStatus = table.Column<bool>(nullable: false),
                    CustomersId = table.Column<int>(nullable: false),
                    TableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderedTable", x => x.CustomerOrderedTableId);
                    table.ForeignKey(
                        name: "FK_CustomerOrderedTable_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "CustomersId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderedTable_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequiredMaterial",
                columns: table => new
                {
                    RequiredMaterialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityInGram = table.Column<int>(nullable: false),
                    IngredientId = table.Column<int>(nullable: false),
                    FoodItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredMaterial", x => x.RequiredMaterialId);
                    table.ForeignKey(
                        name: "FK_RequiredMaterial_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequiredMaterial_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderDetails",
                columns: table => new
                {
                    CustomerOrderDetailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    OnlineStatus = table.Column<bool>(nullable: false),
                    FoodItemNo = table.Column<int>(nullable: false),
                    FoodItemId = table.Column<int>(nullable: true),
                    DiscountId = table.Column<int>(nullable: false),
                    OfferId = table.Column<int>(nullable: true),
                    CustomerOrderedTableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderDetails", x => x.CustomerOrderDetailsId);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetails_CustomerOrderedTable_CustomerOrderedTableId",
                        column: x => x.CustomerOrderedTableId,
                        principalTable: "CustomerOrderedTable",
                        principalColumn: "CustomerOrderedTableId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetails_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetails_Offer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offer",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_CustomerOrderedTableId",
                table: "CustomerOrderDetails",
                column: "CustomerOrderedTableId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_FoodItemId",
                table: "CustomerOrderDetails",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_OfferId",
                table: "CustomerOrderDetails",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderedTable_CustomersId",
                table: "CustomerOrderedTable",
                column: "CustomersId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderedTable_TableId",
                table: "CustomerOrderedTable",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_MealHourId",
                table: "FoodItems",
                column: "MealHourId");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredMaterial_FoodItemId",
                table: "RequiredMaterial",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredMaterial_IngredientId",
                table: "RequiredMaterial",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_IngredientId",
                table: "StockDetails",
                column: "IngredientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerOrderDetails");

            migrationBuilder.DropTable(
                name: "RequiredMaterial");

            migrationBuilder.DropTable(
                name: "StockDetails");

            migrationBuilder.DropTable(
                name: "CustomerOrderedTable");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "MealHour");
        }
    }
}
