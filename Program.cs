using Microsoft.EntityFrameworkCore;
using Handshakes_Carpark.Data;
using Handshakes_Carpark.Repository;
using Handshakes_Carpark.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<BatchJob>();
builder.Services.AddScoped<ICarParkRepository, CarParkRepository>();
builder.Services.AddScoped<IFileProcessor, FileProcessor>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var batchJob = services.GetRequiredService<BatchJob>();

    string filePath = "hdb-carpark-info.xlsx";
    await batchJob.ProcessBatchFileAsync(filePath);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});

app.UseHttpsRedirection();


app.Run();
