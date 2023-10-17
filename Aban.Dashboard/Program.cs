using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Interfaces.Generics;
using Aban.DataLayer.Interfaces.MessageSender;
using Aban.DataLayer.Repositories;
using Aban.DataLayer.Repositories.Generics;
using Aban.DataLayer.Repositories.MessageSender;
using Aban.Domain.Configuration;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Aban.Service.IServices.Generic;
using Aban.Service.Services;
using Aban.Service.Services.Generic;
using Aban.Common.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersianTranslation.Identity;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddRazorRuntimeCompilation();

#region Injected Services

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

//builder.Services.AddScoped<IBlogArticleRepository, BlogArticleRepository>();
builder.Services.AddScoped<IBlogArticleService, BlogArticleService>();
//builder.Services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
builder.Services.AddScoped<IBlogCategoryService, BlogCategoryService>();

builder.Services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();
builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();

builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();


#region Loan

//builder.Services.AddScoped<ICharityAccountService, CharityAccountService>();

#endregion

#endregion

var mvcBuilder = builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}


#region ValidateAntiForgeryToken 
builder.Services.AddAntiforgery(options =>
{
    // Set Cookie properties using CookieBuilder properties†.
    options.FormFieldName = "__RequestVerificationToken";
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
});
#endregion

builder.Services.AddDbContextPool<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));



#region AddJsonFile

builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddJsonFile("Configuration.json", false, true);

#endregion


#region Json Configuration

builder.Services.Configure<IdentityConfiguration>(builder.Configuration.GetSection("Identity"));
builder.Services.Configure<PathsConfiguration>(builder.Configuration.GetSection("Paths"));

#endregion



Enumeration.ExpireTimeType defaultLockoutTimeSpanType = GenericEnumList.Parse<Enumeration.ExpireTimeType>(builder.Configuration.GetSection("Identity")["DefaultLockoutTimeSpanType"]);
int expiredefaultLockoutTimeSpan = Convert.ToInt32(builder.Configuration.GetSection("Identity")["ExpiredefaultLockoutTimeSpan"]);

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
builder.Services.AddIdentity<Aban.Domain.Entities.UserIdentity, IdentityRole>(options =>
{
    options.Password.RequireDigit = bool.Parse(builder.Configuration.GetSection("Identity")["RequireDigit"]);
    options.Password.RequireNonAlphanumeric = bool.Parse(builder.Configuration.GetSection("Identity")["RequireNonAlphanumeric"]);
    options.Password.RequireUppercase = bool.Parse(builder.Configuration.GetSection("Identity")["RequireUppercase"]);
    options.Password.RequireLowercase = bool.Parse(builder.Configuration.GetSection("Identity")["RequireLowercase"]);

    options.User.RequireUniqueEmail = bool.Parse(builder.Configuration.GetSection("Identity")["RequireUniqueEmail"]);
    options.User.AllowedUserNameCharacters = builder.Configuration.GetSection("Identity")["AllowedUserNameCharacters"];

    options.SignIn.RequireConfirmedAccount = bool.Parse(builder.Configuration.GetSection("Identity")["RequireConfirmedAccount"]);
    options.SignIn.RequireConfirmedEmail = bool.Parse(builder.Configuration.GetSection("Identity")["RequireConfirmedEmail"]);
    options.SignIn.RequireConfirmedPhoneNumber = bool.Parse(builder.Configuration.GetSection("Identity")["RequireConfirmedPhoneNumber"]);

    options.Lockout.DefaultLockoutTimeSpan = defaultLockoutTimeSpanType == Enumeration.ExpireTimeType.Years
        ? TimeSpan.FromDays(expiredefaultLockoutTimeSpan * 365)
        : defaultLockoutTimeSpanType == Enumeration.ExpireTimeType.Months
            ? TimeSpan.FromDays(expiredefaultLockoutTimeSpan * 30)
            :
            defaultLockoutTimeSpanType == Enumeration.ExpireTimeType.Weeks
                ? TimeSpan.FromDays(expiredefaultLockoutTimeSpan * 7)
                : defaultLockoutTimeSpanType == Enumeration.ExpireTimeType.Days
                    ? TimeSpan.FromDays(expiredefaultLockoutTimeSpan)
                    :
                    defaultLockoutTimeSpanType == Enumeration.ExpireTimeType.Hours
                        ? TimeSpan.FromHours(expiredefaultLockoutTimeSpan)
                        :
                        defaultLockoutTimeSpanType == Enumeration.ExpireTimeType.Minutes
                            ? TimeSpan.FromMinutes(expiredefaultLockoutTimeSpan)
                            :
                            TimeSpan.FromSeconds(expiredefaultLockoutTimeSpan);
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<PersianIdentityErrorDescriber>()
    ;

Enumeration.ExpireTimeType expireTimeType = GenericEnumList.Parse<Enumeration.ExpireTimeType>(builder.Configuration.GetSection("Identity")["ExpireTimeType"]);
int expireTime = Convert.ToInt32(builder.Configuration.GetSection("Identity")["ExpireTimeSpan"]);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();



builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = builder.Configuration.GetSection("Identity")["AccessDeniedPath"];
    options.Cookie.Name = builder.Configuration.GetSection("Identity")["CookieName"];
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan =
        expireTimeType == Enumeration.ExpireTimeType.Years
        ? TimeSpan.FromDays(expireTime * 365)
        : expireTimeType == Enumeration.ExpireTimeType.Months
            ? TimeSpan.FromDays(expireTime * 30)
            :
            expireTimeType == Enumeration.ExpireTimeType.Weeks
                ? TimeSpan.FromDays(expireTime * 7)
                : expireTimeType == Enumeration.ExpireTimeType.Days
                    ? TimeSpan.FromDays(expireTime)
                    :
                    expireTimeType == Enumeration.ExpireTimeType.Hours
                        ? TimeSpan.FromHours(expireTime)
                        :
                        expireTimeType == Enumeration.ExpireTimeType.Minutes
                            ? TimeSpan.FromMinutes(expireTime)
                            :
                            TimeSpan.FromSeconds(expireTime);

    options.LoginPath = builder.Configuration.GetSection("Identity")["LoginPath"];
    options.LogoutPath = builder.Configuration.GetSection("Identity")["LogOutPath"];
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = Convert.ToBoolean(builder.Configuration.GetSection("Identity")["SlidingExpiration"]);
});



builder.Services.AddOptions<NetworkCredential>().Bind(builder.Configuration.GetSection("EmailSetting"));

builder.Services.AddScoped<IMessageSender, MessageSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


#region Routeing

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Loans",
        areaName: "Loans",
        pattern: "Loans/{controller=Home}/{action}/{id?}");
    endpoints.MapRazorPages();

    endpoints.MapAreaControllerRoute(
        name: "Blog",
        areaName: "Blog",
        pattern: "Blog/{controller=Home}/{action}/{id?}");
    endpoints.MapRazorPages();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "User",
        areaName: "User",
        pattern: "User/{controller=UserIdentity}/{action}");
    endpoints.MapRazorPages();
});



#endregion


#region Default
// همیشه باید آخرین روتینگ باشد بخاطر
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});
#endregion

app.Run();
