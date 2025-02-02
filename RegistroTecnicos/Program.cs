using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;

using RegistroTecnicos.Components;
using RegistroTecnicos.DAL;
using RegistroTecnicos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddSingleton<Radzen.NotificationService>();
builder.Services.AddBlazorBootstrap();

//Obtenemos el ConStr para usarlo en el contexto
var ConStr = builder.Configuration.GetConnectionString("SqlConStr");

//Agregamos el contexto al builder con el ConStr
builder.Services.AddDbContextFactory<Contexto>(o => o.UseSqlServer(ConStr));

//Inyeccion del service
builder.Services.AddScoped<TecnicoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<TicketService>();


builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
