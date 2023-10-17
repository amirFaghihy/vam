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

        #region Loans

        public DbSet<CharityLoan> CharityLoan { get; set; }
        public DbSet<CharityLoanInstallments> CharityLoanInstallments { get; set; }
        public DbSet<Guarantee> Guarantee { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<UserAccountDepositWithdrawal> UserAccountDepositWithdrawal { get; set; }

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

            
            builder.Entity<BlogArticle>()
               .HasOne(c => c.Writer)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BlogArticleBlogCategory>()
              .HasOne(c => c.BlogCategory)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

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
