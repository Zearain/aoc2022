using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using AwesomeSolver.Blazor;
using AwesomeSolver.Blazor.Services;
using AwesomeSolver.Core;
using AwesomeSolver.Core.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAdventOfCodeSolvers();
builder.Services.AddMudServices();

builder.Services.AddScoped<IInputProvider, HttpInputProvider>();

await builder.Build().RunAsync();
