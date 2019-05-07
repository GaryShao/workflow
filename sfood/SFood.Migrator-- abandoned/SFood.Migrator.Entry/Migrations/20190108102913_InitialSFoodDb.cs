using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.Migrator.Entry.Migrations
{
    public partial class InitialSFoodDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "OrderInfo");

            migrationBuilder.EnsureSchema(
                name: "Dish");

            migrationBuilder.EnsureSchema(
                name: "Restaurant");

            migrationBuilder.EnsureSchema(
                name: "HawkerCenter");

            migrationBuilder.EnsureSchema(
                name: "IdentitySchema");

            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.EnsureSchema(
                name: "RelationShip");

            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomizationCategories",
                schema: "Dish",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    MaxOptions = table.Column<byte>(nullable: false),
                    DishId = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizationCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasicInfos",
                schema: "HawkerCenter",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    DetailAddress = table.Column<string>(maxLength: 500, nullable: true),
                    District = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<string>(maxLength: 30, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "IdentitySchema",
                columns: table => new
                {
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Id = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "IdentitySchema",
                columns: table => new
                {
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Id = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                schema: "OrderInfo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    OrderId = table.Column<string>(maxLength: 32, nullable: true),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    PendingToCooking = table.Column<DateTime>(nullable: true),
                    CookingToDeliveOrTaking = table.Column<DateTime>(nullable: true),
                    DeliveOrTakingToDone = table.Column<DateTime>(nullable: true),
                    AnyToClosed = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Icon = table.Column<string>(maxLength: 500, nullable: true),
                    SelectedIcon = table.Column<string>(maxLength: 500, nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    Entry = table.Column<byte>(nullable: false),
                    Value = table.Column<string>(maxLength: 500, nullable: true),
                    IsQualified = table.Column<bool>(nullable: true),
                    Reason = table.Column<string>(maxLength: 500, nullable: true),
                    EditorId = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                schema: "HawkerCenter",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    Capacity = table.Column<int>(nullable: false),
                    FundFlow = table.Column<long>(nullable: false),
                    AmountOfRestaurants = table.Column<int>(nullable: false),
                    ContactName = table.Column<string>(maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(maxLength: 20, nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_BasicInfos_CenterId",
                        column: x => x.CenterId,
                        principalSchema: "HawkerCenter",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                schema: "HawkerCenter",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_BasicInfos_CenterId",
                        column: x => x.CenterId,
                        principalSchema: "HawkerCenter",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasicInfos",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Logo = table.Column<string>(maxLength: 500, nullable: true),
                    Announcement = table.Column<string>(maxLength: 500, nullable: true),
                    IsOpened = table.Column<bool>(nullable: false),
                    IsDeliverySupport = table.Column<bool>(nullable: false),
                    OrderResponseTime = table.Column<byte>(nullable: true),
                    SortWeight = table.Column<int>(nullable: false, defaultValue: 0),
                    SalesVolumeMonth = table.Column<int>(nullable: false, defaultValue: 0),
                    SalesVolumeAnnual = table.Column<int>(nullable: false, defaultValue: 0),
                    SalesVolumeManual = table.Column<int>(nullable: false, defaultValue: 0),
                    Status = table.Column<string>(nullable: false, defaultValue: "Running"),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicInfos_BasicInfos_CenterId",
                        column: x => x.CenterId,
                        principalSchema: "HawkerCenter",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "IdentitySchema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "IdentitySchema",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "IdentitySchema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentitySchema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "IdentitySchema",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentitySchema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "IdentitySchema",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 32, nullable: false),
                    RoleId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "IdentitySchema",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentitySchema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "IdentitySchema",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentitySchema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    RestaurantImageCategory = table.Column<string>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_BasicInfos_CenterId",
                        column: x => x.CenterId,
                        principalSchema: "HawkerCenter",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Images_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserExtensions",
                schema: "IdentitySchema",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    UserId = table.Column<string>(maxLength: 32, nullable: true),
                    StaffStatus = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(maxLength: 100, nullable: true),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExtensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExtensions_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExtensions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentitySchema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dealings",
                schema: "OrderInfo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Status = table.Column<string>(nullable: false),
                    OrderNumber = table.Column<string>(maxLength: 20, nullable: true),
                    DeliveryType = table.Column<string>(nullable: false),
                    PaymentType = table.Column<byte>(nullable: false),
                    IsDishPacked = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    Dishes = table.Column<string>(maxLength: 2000, nullable: true),
                    SeatId = table.Column<string>(maxLength: 32, nullable: true),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    ContactPhone = table.Column<string>(maxLength: 15, nullable: true),
                    UserId = table.Column<string>(maxLength: 32, nullable: true),
                    FetchNumber = table.Column<string>(maxLength: 10, nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dealings_BasicInfos_CenterId",
                        column: x => x.CenterId,
                        principalSchema: "HawkerCenter",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dealings_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dealings_Seats_SeatId",
                        column: x => x.SeatId,
                        principalSchema: "HawkerCenter",
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant&Categories",
                schema: "RelationShip",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    RestaurantCategoryId = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant&Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurant&Categories_Categories_RestaurantCategoryId",
                        column: x => x.RestaurantCategoryId,
                        principalSchema: "Restaurant",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Restaurant&Categories_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    RegistrationStatus = table.Column<string>(maxLength: 30, nullable: true),
                    QualifiedTime = table.Column<DateTime>(nullable: true),
                    ApplicationType = table.Column<string>(nullable: false),
                    RestaurantNo = table.Column<string>(maxLength: 20, nullable: true),
                    OpenedAt = table.Column<short>(nullable: true),
                    ClosedAt = table.Column<short>(nullable: true),
                    IsReceivingAuto = table.Column<bool>(nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: true),
                    Introduction = table.Column<string>(maxLength: 2000, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    SalesVolumeMonth = table.Column<int>(nullable: false),
                    SalesVolumeAnnual = table.Column<int>(nullable: false),
                    SalesVolumeManual = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(maxLength: 500, nullable: true),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_BasicInfos_CenterId",
                        column: x => x.CenterId,
                        principalSchema: "HawkerCenter",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dishes_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    BeginTime = table.Column<short>(nullable: false),
                    EndTime = table.Column<short>(nullable: false),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customizations",
                schema: "Dish",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    DishId = table.Column<string>(maxLength: 32, nullable: true),
                    CategoryId = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customizations_CustomizationCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Dish",
                        principalTable: "CustomizationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customizations_Dishes_DishId",
                        column: x => x.DishId,
                        principalSchema: "Restaurant",
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DishCategories",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Index = table.Column<byte>(nullable: false),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    MenuId = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishCategories_Menus_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "Restaurant",
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DishCategories_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dishes&Categories",
                schema: "RelationShip",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Index = table.Column<byte>(nullable: false),
                    IsOnShelf = table.Column<bool>(nullable: false),
                    DishId = table.Column<string>(maxLength: 32, nullable: true),
                    DishCategoryId = table.Column<string>(maxLength: 32, nullable: true),
                    MenuId = table.Column<string>(maxLength: 32, nullable: true),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes&Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes&Categories_DishCategories_DishCategoryId",
                        column: x => x.DishCategoryId,
                        principalSchema: "Restaurant",
                        principalTable: "DishCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dishes&Categories_Dishes_DishId",
                        column: x => x.DishId,
                        principalSchema: "Restaurant",
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDishes&Customizations",
                schema: "RelationShip",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    OrderId = table.Column<string>(maxLength: 32, nullable: true),
                    RestaurantId = table.Column<string>(nullable: true),
                    CustomizationUnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    CustomizationName = table.Column<string>(maxLength: 20, nullable: true),
                    OrderDishId = table.Column<string>(maxLength: 32, nullable: true),
                    CustomizationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDishes&Customizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDishes&Customizations_Customizations_CustomizationId",
                        column: x => x.CustomizationId,
                        principalSchema: "Dish",
                        principalTable: "Customizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Archives",
                schema: "OrderInfo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Status = table.Column<string>(nullable: false),
                    OrderNumber = table.Column<string>(maxLength: 20, nullable: true),
                    IsMerchantCanceled = table.Column<bool>(nullable: false),
                    DeliveryType = table.Column<string>(nullable: false),
                    PaymentType = table.Column<byte>(nullable: false),
                    IsDishPacked = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    SeatId = table.Column<string>(maxLength: 32, nullable: true),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    BillId = table.Column<string>(maxLength: 32, nullable: true),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    ContactPhone = table.Column<string>(maxLength: 15, nullable: true),
                    UserId = table.Column<string>(maxLength: 32, nullable: true),
                    FetchNumber = table.Column<string>(maxLength: 10, nullable: true),
                    RefusedReason = table.Column<string>(maxLength: 100, nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Archives_BasicInfos_CenterId",
                        column: x => x.CenterId,
                        principalSchema: "HawkerCenter",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Archives_BasicInfos_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "Restaurant",
                        principalTable: "BasicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Archives_Seats_SeatId",
                        column: x => x.SeatId,
                        principalSchema: "HawkerCenter",
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                schema: "OrderInfo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    OrderId = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_Archives_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "OrderInfo",
                        principalTable: "Archives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders&Dishes",
                schema: "RelationShip",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    DishId = table.Column<string>(maxLength: 32, nullable: true),
                    DishUnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    DishName = table.Column<string>(maxLength: 100, nullable: true),
                    OrderId = table.Column<string>(maxLength: 32, nullable: true),
                    RestaurantId = table.Column<string>(maxLength: 32, nullable: true),
                    Amount = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders&Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders&Dishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalSchema: "Restaurant",
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders&Dishes_Archives_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "OrderInfo",
                        principalTable: "Archives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CenterId",
                schema: "Common",
                table: "Images",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_RestaurantId",
                schema: "Common",
                table: "Images",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_CategoryId",
                schema: "Dish",
                table: "Customizations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_DishId",
                schema: "Dish",
                table: "Customizations",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_CenterId",
                schema: "HawkerCenter",
                table: "Details",
                column: "CenterId",
                unique: true,
                filter: "[CenterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CenterId",
                schema: "HawkerCenter",
                table: "Seats",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "IdentitySchema",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "IdentitySchema",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "IdentitySchema",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExtensions_RestaurantId",
                schema: "IdentitySchema",
                table: "UserExtensions",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExtensions_UserId",
                schema: "IdentitySchema",
                table: "UserExtensions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "IdentitySchema",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "IdentitySchema",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "IdentitySchema",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "IdentitySchema",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Archives_BillId",
                schema: "OrderInfo",
                table: "Archives",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Archives_CenterId",
                schema: "OrderInfo",
                table: "Archives",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Archives_RestaurantId",
                schema: "OrderInfo",
                table: "Archives",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Archives_SeatId",
                schema: "OrderInfo",
                table: "Archives",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_OrderId",
                schema: "OrderInfo",
                table: "Bills",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Dealings_CenterId",
                schema: "OrderInfo",
                table: "Dealings",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealings_RestaurantId",
                schema: "OrderInfo",
                table: "Dealings",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealings_SeatId",
                schema: "OrderInfo",
                table: "Dealings",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_OrderId",
                schema: "OrderInfo",
                table: "Details",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes&Categories_DishCategoryId",
                schema: "RelationShip",
                table: "Dishes&Categories",
                column: "DishCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes&Categories_DishId",
                schema: "RelationShip",
                table: "Dishes&Categories",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDishes&Customizations_CustomizationId",
                schema: "RelationShip",
                table: "OrderDishes&Customizations",
                column: "CustomizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDishes&Customizations_OrderDishId",
                schema: "RelationShip",
                table: "OrderDishes&Customizations",
                column: "OrderDishId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders&Dishes_DishId",
                schema: "RelationShip",
                table: "Orders&Dishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders&Dishes_OrderId",
                schema: "RelationShip",
                table: "Orders&Dishes",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant&Categories_RestaurantCategoryId",
                schema: "RelationShip",
                table: "Restaurant&Categories",
                column: "RestaurantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant&Categories_RestaurantId",
                schema: "RelationShip",
                table: "Restaurant&Categories",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicInfos_CenterId",
                schema: "Restaurant",
                table: "BasicInfos",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_RestaurantId",
                schema: "Restaurant",
                table: "Details",
                column: "RestaurantId",
                unique: true,
                filter: "[RestaurantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DishCategories_MenuId",
                schema: "Restaurant",
                table: "DishCategories",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_DishCategories_RestaurantId",
                schema: "Restaurant",
                table: "DishCategories",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CenterId",
                schema: "Restaurant",
                table: "Dishes",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_RestaurantId",
                schema: "Restaurant",
                table: "Dishes",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RestaurantId",
                schema: "Restaurant",
                table: "Menus",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDishes&Customizations_Orders&Dishes_OrderDishId",
                schema: "RelationShip",
                table: "OrderDishes&Customizations",
                column: "OrderDishId",
                principalSchema: "RelationShip",
                principalTable: "Orders&Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Archives_Bills_BillId",
                schema: "OrderInfo",
                table: "Archives",
                column: "BillId",
                principalSchema: "OrderInfo",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_BasicInfos_CenterId",
                schema: "HawkerCenter",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Archives_BasicInfos_CenterId",
                schema: "OrderInfo",
                table: "Archives");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicInfos_BasicInfos_CenterId",
                schema: "Restaurant",
                table: "BasicInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Archives_BasicInfos_RestaurantId",
                schema: "OrderInfo",
                table: "Archives");

            migrationBuilder.DropForeignKey(
                name: "FK_Archives_Bills_BillId",
                schema: "OrderInfo",
                table: "Archives");

            migrationBuilder.DropTable(
                name: "Images",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "VerificationCodes",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Details",
                schema: "HawkerCenter");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "UserExtensions",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "Dealings",
                schema: "OrderInfo");

            migrationBuilder.DropTable(
                name: "Details",
                schema: "OrderInfo");

            migrationBuilder.DropTable(
                name: "Dishes&Categories",
                schema: "RelationShip");

            migrationBuilder.DropTable(
                name: "OrderDishes&Customizations",
                schema: "RelationShip");

            migrationBuilder.DropTable(
                name: "Restaurant&Categories",
                schema: "RelationShip");

            migrationBuilder.DropTable(
                name: "Details",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "Qualifications",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "IdentitySchema");

            migrationBuilder.DropTable(
                name: "DishCategories",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "Customizations",
                schema: "Dish");

            migrationBuilder.DropTable(
                name: "Orders&Dishes",
                schema: "RelationShip");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "CustomizationCategories",
                schema: "Dish");

            migrationBuilder.DropTable(
                name: "Dishes",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "BasicInfos",
                schema: "HawkerCenter");

            migrationBuilder.DropTable(
                name: "BasicInfos",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "Bills",
                schema: "OrderInfo");

            migrationBuilder.DropTable(
                name: "Archives",
                schema: "OrderInfo");

            migrationBuilder.DropTable(
                name: "Seats",
                schema: "HawkerCenter");
        }
    }
}
