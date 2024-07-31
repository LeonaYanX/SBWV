using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;


namespace SBWV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SwapBookDbContext>();
            builder.Services.AddTransient<Repository>();
            

            // Adding Session

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            // подключение стандартной авторизации
            // cookie-authentication

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/account/login"); //если пользователь не авторизован, перенаправляет
                                                                             //на страницу входа
            builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            app.UseSession();
            app.UseAuthentication();// подключение стандартной авторизации
            app.UseRouting();

            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

           

            app.Run();
        }
    }
}
