using System.Diagnostics;
using ApexCharts;
using Conservices.Status.Components;
using Conservices.Status.Repositories.Implementations;
using Conservices.Status.Repositories.Interfaces;
using Conservices.Status.Services.Implementation;
using Conservices.Status.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;
using Index = Conservices.Status.Ui.Modules.Index.Index;

if (!Directory.Exists("./logs/"))
	Directory.CreateDirectory("./logs/");

Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.Console()
			.WriteTo.Debug()
			.WriteTo.Trace()
			.WriteTo.File(
				"./logs/",
				rollingInterval: RollingInterval.Day,
				restrictedToMinimumLevel: LogEventLevel.Information
			)
			.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
	.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddApexCharts(
	e =>
	{
		e.GlobalOptions = new ApexChartBaseOptions()
		{
			Annotations = new Annotations()
			{
				Yaxis =
				[
					new AnnotationsYAxis()
					{
						Y = 0,
					},
				],
			},
			Chart = new Chart()
			{
				Height = "100%",
				Selection = new Selection()
				{
					Enabled = false,
				},
				Toolbar = new Toolbar()
				{
					Show = false,
					Tools = new Tools()
					{
						Selection = false,
						Zoom = false,
						Zoomin = false,
						Zoomout = false,
						Pan = false,
					},
				},
			},
			Tooltip = new Tooltip()
			{
				Enabled = false,
			},
			Stroke = new Stroke()
			{
				Width = 3,
				Curve = Curve.Smooth,
			},
		};
	}
);

builder.Services.AddKeyedSingleton<IPingRepository, ConservicesApiPingRepository>("conservices.de");
builder.Services.AddKeyedSingleton<IPingRepository, ConnectPingRepository>("connect.conservices.de");
builder.Services.AddKeyedSingleton<IServerStatusRepository, ConservicesServerStatusRepository>("conservices-server");

builder.Services.AddSingleton<IApiResponseService, ApiResponseService>();
builder.Services.AddSingleton<IServerUptimeService, ServerUptimeService>();

if (!Directory.Exists("./keys/"))
	Directory.CreateDirectory("./keys/");

builder
	.Services.AddDataProtection()
	.PersistKeysToFileSystem(new DirectoryInfo("./keys/"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app
	.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddAdditionalAssemblies(typeof(Index).Assembly);

var timerService = app.Services.GetRequiredService<IApiResponseService>();
var statusService = app.Services.GetRequiredService<IServerUptimeService>();

Log.Debug("Starting Api Timer");
Task.Run(timerService.ApiResponseTimer);
Task.Run(statusService.ServerResponseTimer);

await app.RunAsync();