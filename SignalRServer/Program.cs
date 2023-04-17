using MessagePack;
using Microsoft.AspNetCore.Http.Connections;
using SignalRServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR()
    .AddMessagePackProtocol(opt =>
    {
        opt.SerializerOptions = MessagePackSerializerOptions.Standard
            .WithOmitAssemblyVersion(true);
    })
    // run multiple instances with different urls to see redis backplane in action
    // dotnet run --urls "http://localhost:5000"
    .AddStackExchangeRedis("localhost:6379");

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<LearningHub>("/learning-hub", opts =>
opts.Transports = HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling);

app.Run();
