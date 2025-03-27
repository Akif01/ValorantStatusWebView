using Microsoft.AspNetCore.DataProtection;
using ValorantStatusWebView.API;
using ValorantStatusWebView.Components;

namespace ValorantStatusWebView
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add console logging
            builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.AddHttpClient<IApiService, ApiService>();
            builder.Services.AddSingleton<ConfigurationService>();
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("/app/keys")) // Persistent storage
                .SetApplicationName("ValorantStatusWebView"); // Shared app name
            
            builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
