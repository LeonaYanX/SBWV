using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using SBWV.Service;
using log4net;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using log4net.Config;
using System.IO;
using SBWV.Models;
using Microsoft.EntityFrameworkCore;
using SBWV.Abstractions;


namespace SBWV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // устанавливаем файл для логгирования
            builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            // настройка логгирования с помошью свойства Logging идет до 
            // создания объекта WebApplication

            builder.Services.AddDbContext<SwapBookDbContext>();
            builder.Services.AddTransient<IRepository,Repository>();
            builder.Services.AddTransient<MailSender>();
            
            builder.Services.AddControllersWithViews();
            

            builder.Services.AddDistributedMemoryCache();

           /* builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });*/


            // подключение стандартной авторизации
            // cookie-authentication

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/account/login"); //если пользователь не авторизован, перенаправляет
                                                                             //на страницу входа
            builder.Services.AddAuthorization();



            var app = builder.Build();

        


            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error"); // Перенаправление на метод в контроллере при ошибке
                app.UseHsts();
            }

            /* // Configure the HTTP request pipeline.
             if (!app.Environment.IsDevelopment())
             {
                 app.UseExceptionHandler("/Home/Error");
                 // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                 app.UseHsts();
             }*/

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "dist")),
                RequestPath = "/dist"
            });

          //  app.UseSession();
            app.UseAuthentication();// подключение стандартной авторизации
            app.UseRouting();

            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                     name: "ConfirmEmail",
                     pattern : "Account/ConfirmEmail/{email}/{code}",
              defaults: new { controller = "Account", action = "ConfirmEmail" }
            );

           

            app.Run();
        }

        
    }
}











