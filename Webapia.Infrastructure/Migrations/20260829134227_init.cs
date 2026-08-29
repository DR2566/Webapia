using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Webapia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CreationTimestamp = table.Column<int>(type: "int", nullable: false, defaultValueSql: "DATEDIFF(SECOND, '1970-01-01', SYSUTCDATETIME())"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ImgUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreationTimestamp", "Description", "ImgUri", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("01a04c51-6d80-732b-a63a-425edc702774"), 1750000000, "27-inch 4K Display", "https://img.alza.cz/m1.jpg", "Monitor", 299.99m },
                    { new Guid("01a04c51-7168-7a2f-801d-56bcc4917746"), 1750000010, "Mechanical Keyboard", "https://img.alza.cz/k1.jpg", "Keyboard", 89.99m },
                    { new Guid("01a04c51-7550-7e53-afc7-a1c969e6176c"), 1750000020, "Wireless RGB Gaming Mouse", "https://img.alza.cz/mouse1.jpg", "Gaming Mouse", 59.99m },
                    { new Guid("01a04c51-7938-7b3b-9494-b2dfb5f11928"), 1750000030, "15-inch Performance Laptop", "https://img.alza.cz/laptop1.jpg", "Laptop", 1299.99m },
                    { new Guid("01a04c51-7d20-7910-bb53-ca101a408cec"), 1750000040, "Wireless Noise Cancelling Headphones", "https://img.alza.cz/headphones1.jpg", "Headphones", 149.99m },
                    { new Guid("01a04c51-8108-76b8-b0b7-bfc4e5259d1d"), 1750000050, "Full HD USB Webcam", "https://img.alza.cz/webcam1.jpg", "Webcam", 79.99m },
                    { new Guid("01a04c51-84f0-7822-b7a7-5d3bbb201c28"), 1750000060, "7-in-1 USB-C Hub", "https://img.alza.cz/hub1.jpg", "USB-C Hub", 49.99m },
                    { new Guid("01a04c51-88d8-7634-b067-d1e0935b1005"), 1750000070, "1TB Portable SSD", "https://img.alza.cz/ssd1.jpg", "External SSD", 119.99m },
                    { new Guid("01a04c51-8cc0-7d77-8198-ec1cd446c538"), 1750000080, "Ergonomic Gaming Chair", "https://img.alza.cz/chair1.jpg", "Gaming Chair", 249.99m },
                    { new Guid("01a04c51-90a8-7b30-b95b-1948e4692bcb"), 1750000090, "LED Desk Lamp", "https://img.alza.cz/lamp1.jpg", "Desk Lamp", 39.99m },
                    { new Guid("01a04c51-9490-757c-953c-ad77c495bc14"), 1750000100, "128GB 5G Smartphone", "https://img.alza.cz/phone1.jpg", "Smartphone", 899.99m },
                    { new Guid("01a04c51-9878-71bc-9148-22ad0dc5df84"), 1750000110, "11-inch Tablet", "https://img.alza.cz/tablet1.jpg", "Tablet", 599.99m },
                    { new Guid("01a04c51-9c60-7e4d-ac92-d1a700cedc55"), 1750000120, "Fitness and Health Tracking", "https://img.alza.cz/watch1.jpg", "Smart Watch", 229.99m },
                    { new Guid("01a04c51-a048-7f61-8b5a-d4ab0b923845"), 1750000130, "Portable Bluetooth Speaker", "https://img.alza.cz/speaker1.jpg", "Bluetooth Speaker", 99.99m },
                    { new Guid("01a04c51-a430-7f0f-b622-c2be8e9947e9"), 1750000140, "USB Condenser Microphone", "https://img.alza.cz/microphone1.jpg", "Microphone", 129.99m },
                    { new Guid("01a04c51-a818-7a75-8c77-8f7d76aafcda"), 1750000150, "High Performance Graphics Card", "https://img.alza.cz/gpu1.jpg", "Graphics Card", 699.99m },
                    { new Guid("01a04c51-ac00-7f69-b711-a64c0c7638b1"), 1750000160, "8-Core Desktop Processor", "https://img.alza.cz/cpu1.jpg", "Processor", 349.99m },
                    { new Guid("01a04c51-afe8-7451-baa4-d7240366440d"), 1750000170, "32GB DDR5 Memory Kit", "https://img.alza.cz/ram1.jpg", "RAM", 159.99m },
                    { new Guid("01a04c51-b3d0-7ec4-8c9c-7ae4b141ae6c"), 1750000180, "ATX Gaming Motherboard", "https://img.alza.cz/motherboard1.jpg", "Motherboard", 219.99m },
                    { new Guid("01a04c51-b7b8-70c7-ace9-1781ae65ec84"), 1750000190, "750W Modular Power Supply", "https://img.alza.cz/psu1.jpg", "Power Supply", 109.99m },
                    { new Guid("01a04c51-bba0-7fc4-b146-9ac145fab374"), 1750000200, "Tempered Glass ATX Case", "https://img.alza.cz/case1.jpg", "PC Case", 94.99m },
                    { new Guid("01a04c51-bf88-7671-b161-d56ed2220741"), 1750000210, "Wi-Fi 6 Router", "https://img.alza.cz/router1.jpg", "Router", 139.99m },
                    { new Guid("01a04c51-c370-708e-9be1-e7922d4e09c1"), 1750000220, "Wireless Color Printer", "https://img.alza.cz/printer1.jpg", "Printer", 189.99m },
                    { new Guid("01a04c51-c758-7c36-b501-267d1c97ba9b"), 1750000230, "Full HD Home Projector", "https://img.alza.cz/projector1.jpg", "Projector", 499.99m },
                    { new Guid("01a04c51-cb40-7338-839e-307fab222384"), 1750000240, "Virtual Reality Headset", "https://img.alza.cz/vr1.jpg", "VR Headset", 449.99m },
                    { new Guid("01a04c51-cf28-7889-ad48-b74da2898f9f"), 1750000250, "Wireless Game Controller", "https://img.alza.cz/controller1.jpg", "Game Controller", 69.99m },
                    { new Guid("01a04c51-d310-713e-bd2f-6951020f5515"), 1750000260, "Programmable Mechanical Keypad", "https://img.alza.cz/keypad1.jpg", "Mechanical Keypad", 49.99m },
                    { new Guid("01a04c51-d6f8-7a30-85a3-692e8858e7ff"), 1750000270, "Adjustable Aluminum Laptop Stand", "https://img.alza.cz/stand1.jpg", "Laptop Stand", 34.99m },
                    { new Guid("01a04c51-dae0-7411-9749-67c73d2cec66"), 1750000280, "Fast Wireless Charging Pad", "https://img.alza.cz/charger1.jpg", "Wireless Charger", 29.99m },
                    { new Guid("01a04c51-dec8-77da-92fe-f30057578d28"), 1750000290, "128GB USB 3.2 Flash Drive", "https://img.alza.cz/usb1.jpg", "USB Flash Drive", 19.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreationTimestamp",
                table: "Products",
                column: "CreationTimestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
