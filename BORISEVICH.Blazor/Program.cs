using BORISEVICH.Blazor.Components;
using BORISEVICH.Blazor.Services;
using BORISEVICH.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddHttpClient<IProductService<Book>, ApiProductService>(c =>
c.BaseAddress = new Uri("https://localhost:7002/api/books"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
