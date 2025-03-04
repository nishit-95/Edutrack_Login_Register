using Edutrack;
using Edutrack.Interfaces;
using Edutrack.Repositories.Implementations;
using Npgsql;
using Repositories.Implementations;
using Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IRegisterInterface, RegisterRepository>();
builder.Services.AddScoped<ILoginInterface, LoginRepository>();
builder.Services.AddScoped<IStudentInterface, StudentRepository>();
builder.Services.AddScoped<ITeacherInterface, TeacherRepository>();

builder.Services.AddScoped<NpgsqlConnection>((parameter) =>
{
    var ConnectionString = parameter.GetRequiredService<IConfiguration>().GetConnectionString("pgconn");
    return new NpgsqlConnection(ConnectionString);
});


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5190")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()); // Add this to allow credentials
});

builder.Services.AddDistributedMemoryCache(); // Add this before AddSession

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
app.UseCors("AllowSpecificOrigin");
app.UseSession();
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
