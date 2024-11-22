using ApexCharts;
using Conservices.Status.Components;
using Conservices.Status.Repositories.Implementations;
using Conservices.Status.Repositories.Interfaces;
using Conservices.Status.Services.Implementation;
using Conservices.Status.Services.Interfaces;
using MudBlazor.Services;
using Index = Conservices.Status.Ui.Modules.Index.Index;

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

builder.Services.AddKeyedSingleton<IPingRepository, ConservicesPingRepository>("conservices.de");
builder.Services.AddKeyedSingleton<IPingRepository, ConnectPingRepository>("connect.conservices.de");

builder.Services.AddSingleton<IApiResponseService, ApiResponseService>();

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

_ = app.Services.GetRequiredService<IApiResponseService>();

await app.RunAsync();