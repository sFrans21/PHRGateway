using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan konfigurasi ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Tambahkan layanan Ocelot ke dalam container
//builder.Services.AddOcelot();

builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();


app.UseDefaultFiles();
app.UseStaticFiles();

// Gunakan middleware Ocelot
await app.UseOcelot();

//app.MapGet("/", () => new { Status = "PHR Gateway API is running", Timestamp = DateTime.UtcNow });
app.Run();