using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Net.Http;
using Core.Interface; 
using Data.Services;
using BlazorClient.Provider;
using BlazorClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ����������� HttpClient � ������� �������
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7057") });

// ����������� CustomAuthStateProvider � AuthenticationStateProvider
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

// ���������� ������ �����������
builder.Services.AddAuthorizationCore();

// ����������� IProductService � ProductService
//builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, HttpCategoryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, HttpProductService>();

await builder.Build().RunAsync();
