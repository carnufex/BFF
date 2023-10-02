using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("token")
				.AddJwtBearer("token", options =>
				{
					options.Authority = "https://demo.duendesoftware.com";
					options.Audience = "api";

					options.MapInboundClaims = false;
				});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("ApiCaller", policy =>
	{
		policy.RequireClaim("scope", "api");
	});

	options.AddPolicy("RequireInteractiveUser", policy =>
	{
		policy.RequireClaim("sub");
	});
});

var app = builder.Build();
//
//IdentityModelEventSource.ShowPII = true;

app.UseAuthentication();
app.UseAuthorization();
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
.RequireAuthorization("ApiCaller");

app.MapGet("/weatherforecastt", () =>
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
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
