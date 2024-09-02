using System.Reflection;
using VarzeaLeague.Application.Extension;
using VarzeamTeam.Infra.CrossCutting.Ioc;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    DependencyInjection.ConfigureService(builder.Services, builder.Configuration, xmlFilename);
}

var app = builder.Build();

app.UseMiddleware<ElasticsearchMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();
