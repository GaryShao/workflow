using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.EFCore.Configurations;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.IdentityModels;
using SFood.DataAccess.Models.LocalizationModels;
using SFood.DataAccess.Models.ProcedureModels;
using SFood.DataAccess.Models.RelationshipModels;

namespace SFood.DataAccess.EFCore
{
    public class SFoodDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public static readonly LoggerFactory SFoodLoggerFactory =
            new LoggerFactory(new[] 
            {
                new ConsoleLoggerProvider((category, level) 
                    => category == DbLoggerCategory.Database.Command.Name &&
                        level == LogLevel.Information, true)
            });

        public SFoodDbContext()
        {
        }

        public SFoodDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IEntityMaterializerSource, DateTimeKindEntityMaterializerSource>();            

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(SFoodLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ArchivedOrder>(order =>
            {
                order.Property(od => od.Status).HasConversion<string>();
                order.Property(od => od.DeliveryType).HasConversion<string>();
                order.Property(od => od.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<OrderDetail>(orderDetail => {
                orderDetail.HasIndex(od => od.OrderId).IsUnique(true);
            });

            builder.Entity<Bill>(bill =>
            {
                bill.HasIndex(b => b.OrderId).IsUnique(true);
                bill.Property(b => b.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<DealingOrder>(order =>
            {
                order.Property(od => od.Status).HasConversion<string>();
                order.Property(od => od.DeliveryType).HasConversion<string>();
                order.Property(od => od.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<Dish>(dish =>
            {
                dish.HasIndex(d => d.RestaurantId).IsUnique(false);
                dish.Property(d => d.CreatedTime).HasDefaultValueSql("getutcdate()");
                dish.HasQueryFilter(d => !d.IsDeleted);
            });

            builder.Entity<Dish_DishCategory>(ddc => {
                ddc.HasOne(d => d.Dish)
                .WithMany(di => di.Dish_DishCategories)
                .HasForeignKey(dc => dc.DishId);

                ddc.HasOne(d => d.Category)
                .WithMany(di => di.Dish_DishCategories)
                .HasForeignKey(dc => dc.DishCategoryId);
            });

            builder.Entity<Dish_CustomizationCategory>(dcc => {
                dcc.HasIndex(d => new { d.DishId, d.CustomizationCategoryId });
            });

            builder.Entity<Customization>(dc =>
            {                
                dc.HasQueryFilter(d => !d.IsDeleted);
            });

            builder.Entity<CustomizationCategory>(cc =>
            {
                cc.HasIndex(d => d.RestaurantId);
                cc.HasQueryFilter(d => !d.IsDeleted);
            });

            builder.Entity<HawkerCenter>(center =>
            {
                center.Property(c => c.CreatedTime).HasDefaultValueSql("getutcdate()");

                center.HasOne(c => c.HawkerCenterDetail)
                    .WithOne(hcd => hcd.Center);

                center.Property(c => c.Status).HasConversion<string>();

                center.HasQueryFilter(c => !c.IsDeleted);
            });

            builder.Entity<HawkerCenterDetail>(extension =>
            {
                extension.HasIndex(e => e.CenterId).IsUnique(true);
                extension.Property(e => e.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<HawkerCenterBanner>(banner => {
                banner.HasIndex(b => b.CenterId).IsUnique(false);
                banner.Property(b=>b.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<Image>(image =>
            {
                image.Property(i => i.RestaurantImageCategory).HasConversion<string>();
                image.Property(i =>i.CreatedTime).HasDefaultValueSql("getutcdate()");
                image.HasIndex(i => i.RestaurantId).IsUnique(false);
            });

            builder.Entity<Order_Dish>(orderDish => {
                orderDish.HasOne(od => od.Dish)
                .WithMany(di => di.Order_Dishes)
                .HasForeignKey(d => d.DishId);

                orderDish.HasOne(od => od.Order)
                .WithMany(o => o.Order_Dishes)
                .HasForeignKey(oo => oo.OrderId);

            });

            builder.Entity<OrderDish_Customization>(odc => {
                odc.HasOne(od => od.OrderDish)
                .WithMany(di => di.OrderDish_Customizations)
                .HasForeignKey(d => d.OrderDishId);
            });

            builder.Entity<Restaurant>(restaurant =>
            {
                restaurant.HasIndex(r => r.CenterId).IsUnique(false);

                restaurant.HasOne(r => r.RestaurantDetail)
                    .WithOne(rr => rr.Restaurant);

                restaurant.Property(r => r.Status).HasConversion<string>();
                restaurant.Property(r => r.SortWeight).HasDefaultValue(default(int));
                restaurant.Property(r => r.SalesVolumeMonth).HasDefaultValue(default(int));
                restaurant.Property(r => r.SalesVolumeAnnual).HasDefaultValue(default(int));
                restaurant.Property(r => r.SalesVolumeManual).HasDefaultValue(default(int));

                restaurant.HasQueryFilter(r => r.Status == RestaurantStatus.Running);
            });

            builder.Entity<RestaurantCategory>(rc =>
            {
                rc.Property(r =>r.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<Restaurant_RestaurantCategory>(rrc => {
                rrc.HasOne(rc => rc.Restaurant)
                .WithMany(r => r.Restaurant_RestaurantCategories)
                .HasForeignKey(d => d.RestaurantId);

                rrc.HasOne(rc => rc.RestaurantCategory)
                .WithMany(rec => rec.Restaurant_RestaurantCategories)
                .HasForeignKey(oo => oo.RestaurantCategoryId);

            });

            builder.Entity<RestaurantDetail>(re =>
            {
                re.HasIndex(e => e.RestaurantId).IsUnique(true);
                re.Property(e => e.CreatedTime).HasDefaultValueSql("getutcdate()");
                re.Property(e => e.ApplicationType).HasConversion<string>();
                re.Property(e => e.RegistrationStatus).HasConversion<string>();
            });

            builder.Entity<Seat>(seat => {
                seat.HasIndex(s => s.CenterId).IsUnique(false);
                seat.HasIndex(s => s.SeatAreaId).IsUnique(false);
                seat.Property(s => s.CreatedTime).HasDefaultValueSql("getutcdate()");

                seat.HasQueryFilter(s => !s.IsDeleted);
            });
            builder.Entity<SeatArea>(seatArea => {
                seatArea.HasIndex(s => s.CenterId).IsUnique(false);
                seatArea.HasQueryFilter(s => !s.IsDeleted);
                seatArea.Property(e => e.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<VerificationCode>(vc => {
                vc.Property(e => e.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            builder.Entity<Qualification>(qa => {                
                qa.Property(q => q.CreatedTime).HasDefaultValueSql("getutcdate()");
            });

            #region Identity

            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<RoleClaim>().ToTable("RoleClaims");
            builder.Entity<User>().ToTable("Users");
            builder.Entity<UserClaim>().ToTable("UserClaims");
            builder.Entity<UserLogin>().ToTable("UserLogins");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<UserToken>().ToTable("UserTokens");
            builder.Entity<UserExtension>(ue => {
                ue.Property(u => u.StaffStatus).HasConversion<string>();
            });

            #endregion

            builder.Entity<Language>(lang => {
                lang.HasIndex(l => l.Code).IsUnique(true);
            });

            builder.Entity<Resource>(resource => {
                resource.HasIndex(re => new { re.LanguageId, re.Key }).IsUnique(true);
            });
        }

        public DbSet<ArchivedOrder> ArchivedOrders { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Customization> DishCustomizations { get; set; }
        public DbSet<CustomizationCategory> CustomizationCategories { get; set; }
        public DbSet<DealingOrder> DealingOrders { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishCategory> DishCategories { get; set; }        
        public DbSet<HawkerCenter> HawkerCenters { get; set; }
        public DbSet<HawkerCenterDetail> HawkerCenterDetails { get; set; }
        public DbSet<HawkerCenterBanner> Banners { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
        public DbSet<RestaurantDetail> RestaurantDetails { get; set; }        
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatArea> SeatAreas { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<UserExtension> UserExtensions { get; set; }
        public DbSet<CountryCode> CountryCodes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Resource> Resources { get; set; }


        public DbSet<Dish_DishCategory> Dish_DishCategories { get; set; }
        public DbSet<Order_Dish> Order_Dishes { get; set; }
        public DbSet<OrderDish_Customization> OrderDish_Customizations { get; set; }
        public DbSet<Restaurant_RestaurantCategory> Restaurant_RestaurantCategories { get; set; }   
        public DbSet<Dish_CustomizationCategory> Dish_CustomizationCategories { get; set; }
        
        public DbQuery<USP_DishCustomization> USP_DishCustomizations { get; set; }    
        public DbQuery<USP_DishStatistic> USP_DishStatistics { get; set; }
    }
}