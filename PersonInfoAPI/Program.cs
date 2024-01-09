using Microsoft.EntityFrameworkCore;
using PersonInfo.Data;
using PersonInfo.Data.Infrastructure;
using PersonInfo.Data.Repositories;
using PersonInfo.Service;
using PersonInfo.Service.Interfaces;
using PersonInfo.Service.Mapping;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration
                           .GetConnectionString("PersonInfoDbConnection")
                           ?? throw new InvalidOperationException("Connection string 'PersonInfoDbConnection' not found."); ;

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//DbContext
builder.Services.AddDbContext<PersonInfoContext>(options =>
{
    options.UseSqlServer(connString);
});


//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//builder.Services.AddAutoMapper(typeof(MappingProfile).GetType().Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMvcCore();


// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DbContext, PersonInfoContext>();
builder.Services.AddScoped<IDbFactory, DbFactory>();


builder.Services.AddScoped<IPersonRelatedPeopleRepository, PersonRelatedPeopleRepository>();
builder.Services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();




var app = builder.Build();

// later for migration

//using (var scope = app.Services.CreateScope())
//{
//    using var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
//    dbContext.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Person}/{action=Index}/{id?}");

app.Run();
