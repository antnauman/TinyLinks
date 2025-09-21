using FluentValidation.AspNetCore;
using FluentValidation;
using LinkService.Application;
using LinkService.Infrastructure;
using LinkService.Grpc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddGrpc();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateLinkValidator>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<LinkDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
// {
//     o.Authority = builder.Configuration["Jwt:Authority"];
//     o.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateAudience = true,
//         ValidAudience = builder.Configuration["Jwt:Audience"]
//     };
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<LinkResolverService>();

app.MapGet("/healthz", () => Results.Ok("ok"));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LinkDbContext>();
    db.Database.Migrate();
}

app.Run();
