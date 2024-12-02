using FeatureHubSDK;
using Measurement.Context;
using Measurement.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IClientContext>(sp =>
{
    var featureHubConfig = new EdgeFeatureHubConfig("http://localhost:8085", "20f56c80-0a65-4ca4-a0ac-af86d8646ff6/kOth0DoMNfI2eFCrBgsFaWQLtlJ3SyYmZ2Bf7ykc");
    return featureHubConfig.NewContext();
});

builder.Services.AddTransient<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddDbContext<MeasurementDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MeasurementDb")));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

app.Run();
