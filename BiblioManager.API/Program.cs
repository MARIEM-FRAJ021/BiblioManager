using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Repository;
using BiblioManager.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repos
builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
builder.Services.AddScoped<ILivreRepository, LivreRepository>();
builder.Services.AddScoped<IAuteurRepository, AuteurRepository>();
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<IAdherentRepository, AdherentRepository>();
builder.Services.AddScoped<IPaimentRepository, PaiementRepository>();

//Services
builder.Services.AddScoped<IAdherentService, AdherentService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BiblothequeDbContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BiblothequeDbContext>();
    SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
