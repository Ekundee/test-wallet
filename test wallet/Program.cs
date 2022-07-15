using Microsoft.EntityFrameworkCore;
using System.Reflection;
using test_wallet.DbContexts;
using test_wallet.Repositories;
using test_wallet.HelperFunctions;
using test_wallet.HelperFunctions.Implementation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IWalletRepository, WalletRepository>();
builder.Services.AddSingleton<ICustomPasswordHasher, CustomPasswordHasher>();   
builder.Services.AddScoped<ITokenAuthorization, TokenAuthorization>();


builder.Services.AddControllers();
string mySqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection");

// Db contexts
builder.Services.AddDbContext<WalletDbContext>(options => options.UseSqlServer(mySqlConnectionStr));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();

// Cors
builder.Services.AddCors(p => p.AddPolicy("corsPolicy", build =>
 {
     build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
 }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("corsPolicy");

app.Run();