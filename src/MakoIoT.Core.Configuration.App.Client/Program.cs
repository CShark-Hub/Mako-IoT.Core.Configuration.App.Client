using Blazored.Modal;
using MakoIoT.Core.Configuration.App.Client;
using MakoIoT.Core.Configuration.App.Client.Services;
using MakoIoT.Core.Configuration.App.Client.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("Device");

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Device"));

builder.Services.AddScoped<IDeviceConfigService, DeviceConfigService>();

//view models
builder.Services.AddSingleton<MessageViewModel>();
builder.Services.AddTransient<IMessage>(sp => sp.GetService<MessageViewModel>());
builder.Services.AddTransient<DeviceViewModel>();
builder.Services.AddSingleton<TimezonesViewModel>();

builder.Services.AddBlazoredModal();

await builder.Build().RunAsync();
