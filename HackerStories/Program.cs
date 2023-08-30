using HackerStories.Interfaces;
using HackerStories.Mappers;
using HackerStories.Model;
using HackerStories.Services;
using Microsoft.Extensions.Configuration;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHackerStoriesService, HackerStoriesService>();
builder.Services.AddScoped<IHackerNewsClient, HackerNewsClient>();
builder.Services.AddScoped<IMapper<HackerStory, StoryDto>, HackerStoryToStoryDtoMapper>();
builder.Services.AddScoped<IRestClient>(provider =>
{
    var baseUrl = builder.Configuration.GetSection("HackerNews:Url").Value;
    return new RestClient(baseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
