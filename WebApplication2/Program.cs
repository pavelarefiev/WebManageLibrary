using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=books.db"));

builder.Services.AddRazorPages();

// Добавляем логирование
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFilter("Microsoft", LogLevel.Warning);

// Добавляем IWebHostEnvironment
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>(builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync($"Global Exception Handler - An error occurred: {ex.Message}\n{ex.StackTrace}");
    }
});

app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.HasFormContentType)
    {
        var form = await context.Request.ReadFormAsync();
        if (form.Files.Count > 0)
        {
            foreach (var file in form.Files)
            {
                System.Diagnostics.Debug.WriteLine($"FileName = {file.FileName}, Length = {file.Length}, ContentType = {file.ContentType}");
            }

        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Form files is empty");
        }
    }
    await next();
});

app.UseAuthorization();
app.MapRazorPages();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error != null)
        {
            var exception = exceptionHandlerPathFeature.Error;
            await context.Response.WriteAsync($"An error occurred: {exception.Message}\n{exception.StackTrace}");
        }
    });
});
// Создание базы данных
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();