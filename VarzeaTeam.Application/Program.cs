using VarzeaLeague.Application.Extension;
using VarzeamTeam.Infra.CrossCutting.Ioc;

var builder = WebApplication.CreateBuilder(args);

DependencyInjection.ConfigureService(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ElasticsearchMiddleware>();

// Configure the HTTP request pipeline.
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