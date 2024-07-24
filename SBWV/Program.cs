using Microsoft.Extensions.FileProviders;
using SBWV.Service;

namespace SBWV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SwapBookDbContext>();
            builder.Services.AddTransient<Repository>();
            builder.Services.AddTransient<Transliteration>();

            // Adding Session

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });




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


            app.UseRouting();

            app.UseAuthorization();

        

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseSession();

            app.Run();
        }
    }
}
