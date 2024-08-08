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

// Регистрация HttpClient с базовым адресом
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7057") });

// Регистрация CustomAuthStateProvider и AuthenticationStateProvider
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

// Добавление службы авторизации
builder.Services.AddAuthorizationCore();

// Регистрация IProductService и ProductService
//builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, HttpCategoryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, HttpProductService>();

await builder.Build().RunAsync();
