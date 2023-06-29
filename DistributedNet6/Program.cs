using DistributedNet6;
using DistributedNet6.Filter;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 配置全局filter过滤器
builder.Services.AddControllers(option =>
{
    option.Filters.Add(typeof(RequestLoggingFilter));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.AddSerilLog();
// 微软自带日志格式
// builder.Services.AddHttpLogging(options =>
// {
//     options.LoggingFields = HttpLoggingFields.All;
//     options.RequestHeaders.Add("sec-ch-ua");
//     options.ResponseHeaders.Add("MyResponseHeader");
//     options.MediaTypeOptions.AddText("application/javascript");
//     options.RequestBodyLogLimit = 4096;
//     options.ResponseBodyLogLimit = 4096;
//
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
// 微软自带
// app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next)=> {
    context.Request.EnableBuffering();
    await next.Invoke();
});
// app.Use(async (context, next) =>
// {
//     context.Response.Headers["MyResponseHeader"] =
//         new string[] { "My Response Header Value" };
//
//     await next();
// });

app.Run();

