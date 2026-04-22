using BEShop.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace BEShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(10000);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddDbContext<EshopContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"), o => o.EnableRetryOnFailure()));
            builder.Services.AddScoped<CommentService>();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
    public class CommentService
    {
        public string AiResponce(string[] messages)
        {
            StringBuilder sb = new();
            sb.Append("return small review based on next text, do not write greetings. \n\r\n\r");
            foreach(var message in messages)
            {
                sb.Append(message);
            }
            string userMessage = sb.ToString();
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
               HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using var client = new HttpClient(handler);

            var payload = new
            {
                model = "gemma3:27b",
                messages = new[]
                {
                new { role = "user", content = userMessage }
            }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}");

            var response = client.PostAsync(
                $"{Environment.GetEnvironmentVariable("OPENAI_BASE_URL")}/chat/completions",
                content
            ).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            using var doc = JsonDocument.Parse(responseString);

            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString();
        }
    }
}
