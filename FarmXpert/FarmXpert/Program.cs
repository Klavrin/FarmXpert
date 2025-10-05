using Microsoft.AspNetCore.Identity;
using FarmXpert.Components;
using FarmXpert.Components.Account;
using FarmXpert.Data;
using AspNetCore.Identity.Mongo;
using FarmXpert.Application.Todo.Queries.GetAllTodos;
using FarmXpert.Domain.Interfaces;
using FarmXpert.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using MongoDB.Driver;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddControllers();
builder.Services.AddSingleton<MongodDbService>();
builder.Services.AddHttpClient();
builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityMongoDbProvider<ApplicationUser>(identityOptions =>
{
    identityOptions.Password.RequireDigit = true;
    identityOptions.Password.RequiredLength = 6;
}, mongoIdentityOptions =>
{
    mongoIdentityOptions.ConnectionString = builder.Configuration.GetConnectionString("MongoDb");
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetAllTodosQueryHandler>();
});

builder.Services.AddAuthorization();

// builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoService = sp.GetRequiredService<MongodDbService>();
    return mongoService.Database;
});

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<IPersonalDocumentRepository, PersonalDocumentRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageServiceRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FarmXpert.Client._Imports).Assembly);

app.MapControllers();
app.MapAdditionalIdentityEndpoints();

app.Run();
