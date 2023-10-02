using Duende.Bff.Yarp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddBff()
	.AddRemoteApis();

// Example CORS configuration in DuendeBFF
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp", builder =>
	{
		builder.WithOrigins("https://localhost:4200")
			   .AllowAnyHeader()
			   .AllowAnyMethod();
	});
});

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = "cookie";
	options.DefaultChallengeScheme = "oidc";
	options.DefaultSignOutScheme = "oidc";
}).AddCookie("cookie", options =>
{
	options.Cookie.Name = "__Host-bff";
	options.Cookie.SameSite = SameSiteMode.Strict;
}).AddOpenIdConnect("oidc", options =>
{
	options.Authority = "https://demo.duendesoftware.com";
	options.ClientId = "interactive.confidential";
	options.ClientSecret = "secret";
	options.ResponseType = "code";
	options.ResponseMode = "query";


	options.GetClaimsFromUserInfoEndpoint = true;
	options.MapInboundClaims = false;
	options.SaveTokens = true;

	options.Scope.Clear();
	options.Scope.Add("openid");
	options.Scope.Add("profile");
	options.Scope.Add("api");
	options.Scope.Add("offline_access");

	options.TokenValidationParameters = new()
	{
		NameClaimType = "name",
		RoleClaimType = "role"
	};
});

var app = builder.Build();



// In your startup.cs, use the CORS policy
app.UseCors("AllowAngularApp");


app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseBff();
app.MapBffManagementEndpoints();
app.UseAuthorization();


//app.MapControllers()
//	.RequireAuthorization()
//	.AsBffApiEndpoint();

app.MapRemoteBffApiEndpoint("/weatherforecastt", "https://localhost:7291/WeatherForecastt");
app.MapRemoteBffApiEndpoint("/weatherforecast", "https://localhost:7291/WeatherForecast").RequireAccessToken(Duende.Bff.TokenType.User);


app.MapFallbackToFile("index.html"); ;

app.Run();