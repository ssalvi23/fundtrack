using FundTrack.Api.Configuration;
using FundTrack.Api.Health;
using FundTrack.Application;
using FundTrack.Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

// Fail at startup, not on the first browser request, when the section is missing.
builder.Services.AddOptions<CorsSettings>()
    .Bind(builder.Configuration.GetSection(CorsSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddCors();
builder.Services.AddOptions<CorsOptions>()
    .Configure<IOptions<CorsSettings>>((cors, settings) =>
        cors.AddPolicy(CorsSettings.PolicyName, policy => policy
            .WithOrigins(settings.Value.AllowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()));

var app = builder.Build();

// Outermost, so it catches exceptions from everything below and returns ProblemDetails.
app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Skipped locally: a redirect on a CORS preflight fails in the browser.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseCors(CorsSettings.PolicyName);
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthResponseWriter.WriteAsync,
});

app.Run();

// Lets WebApplicationFactory<Program> see the top-level Program class from the test project.
public partial class Program;
