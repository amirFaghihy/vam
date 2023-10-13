using Aban.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;

namespace Aban.DataLayer.Context
{
    public class AppDbContext : IdentityDbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AppDbContext(DbContextOptions dbContextOptions)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(dbContextOptions)
        {

        }

        #region Charity

        public DbSet<CharityAccount> CharityAccount { get; set; }
        public DbSet<CharityUserIdentityCharityHelper> CharityUserIdentityCharityHelper { get; set; }
        public DbSet<CharityAddition> CharityAddition { get; set; }
        public DbSet<CharityBankRecord> CharityBankRecord { get; set; }
        public DbSet<CharityDeducation> CharityDeducation { get; set; }
        public DbSet<CharityDeposit> CharityDeposit { get; set; }
        public DbSet<CharityLoan> CharityLoan { get; set; }
        public DbSet<CharityLoanInstallments> CharityLoanInstallments { get; set; }
        public DbSet<CharityWage> CharityWage { get; set; }
        public DbSet<CharityWageCharityAddition> CharityWageCharityAddition { get; set; }
        public DbSet<CharityWageCharityDeduction> CharityWageCharityDeduction { get; set; }
        public DbSet<CharityWageCharityDeposit> CharityWageCharityDeposit { get; set; }
        public DbSet<CharityWageCharityLoanInstallment> CharityWageCharityLoanInstallment { get; set; }

        #endregion



        #region Blog

        public DbSet<BlogArticle> BlogArticle { get; set; }
        public DbSet<BlogArticleBlogCategory> BlogArticleBlogCategory { get; set; }
        public DbSet<BlogCategory> BlogCategory { get; set; }

        #endregion

        #region Main

        public DbSet<City> City { get; set; }
        public DbSet<State> State { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<UserIdentity>()
               .HasOne(c => c.State)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserIdentity>()
               .HasOne(c => c.UserRegistrar)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BlogArticle>()
               .HasOne(c => c.Writer)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BlogArticleBlogCategory>()
              .HasOne(c => c.BlogCategory)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityDeposit>()
              .HasOne(c => c.UserIdentity)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityDeposit>()
             .HasOne(c => c.CharityAccount)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityWageCharityAddition>()
             .HasOne(c => c.CharityAddition)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<CharityWageCharityDeduction>()
             .HasOne(c => c.CharityDeducation)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityAddition>()
             .HasOne(c => c.UserIdentity)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityDeducation>()
             .HasOne(c => c.UserIdentity)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityUserIdentityCharityHelper>()
             .HasOne(c => c.UserIdentity)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityWage>()
             .HasOne(c => c.UserIdentity)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityWage>()
             .HasOne(c => c.WageReceiver)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharityLoan>()
             .HasOne(c => c.UserIdentity)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);


            //builder.Seed(_databaseContext);
            //builder.OnModelBuilder();

        }

    }


    /// <summary>
    /// این کلاس به این دلیل ایجاد شد که زمانی که ماگریشن میزدم، خطا میداد
    /// پس از سرچ کردن بسیار زیاد، در یک ویدیو، با این تکه کد مواجه شدم
    /// پس از اضاف کردن این تکه کد به پروژه، مشکل رفع شد
    /// </summary>
    public class BloggingContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionstring = configurationRoot.GetConnectionString("MyConnectionString");
            builder.UseSqlServer(connectionstring);

            return new AppDbContext(builder.Options);
        }
    }

}
