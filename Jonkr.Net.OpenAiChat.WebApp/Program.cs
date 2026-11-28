using Jonkr.Net.OpenAiChat.WebApp.Components;
using Jonkr.Net.OpenAiChat.WebApp.Services;
using Serilog;

namespace Jonkr.Net.OpenAiChat.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Serilog logging configuration
            builder.Host.UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration)
            );

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.Configure<OpenAiOptions>(builder.Configuration.GetSection("OpenAI"));
            builder.Services.AddSingleton<OpenAiChatService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
