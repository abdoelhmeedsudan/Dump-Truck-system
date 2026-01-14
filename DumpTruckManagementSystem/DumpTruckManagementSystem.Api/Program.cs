using DumpTruckManagementSystem.Application.Features.DriverFeature.Queries.Query;
using DumpTruckManagementSystem.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddMediatRService();
builder.Services.AddAutoMapperProfile();
builder.Services.AddSwaggerOpenAPI();
builder.Services.AddMediatRService((typeof(GetAllDriverQuery).Assembly));


var app = builder.Build();

app.ConfigureSwagger();

app.UseHttpsRedirection();

app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();