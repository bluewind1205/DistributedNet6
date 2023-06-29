using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace DistributedNet6.Filter;

public class RequestLoggingFilter : IActionFilter
{
    private readonly Serilog.ILogger _logger; //注入serilog
    private Stopwatch _stopwatch; //统计程序耗时

    public RequestLoggingFilter(Serilog.ILogger logger)
    {
        _logger = logger;
        _stopwatch = Stopwatch.StartNew();
    }

    public async void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch.Stop();
        var request = context.HttpContext.Request;
        var response = context.HttpContext.Response;
        _logger
            .ForContext("RequestJson",  await GetBody(request.Body)) //请求字符串
            .Information("Request {Method} {Path} responded {StatusCode} in {Elapsed:0.0000} ms", //message
                request.Method,
                request.Path,
                response.StatusCode,
                _stopwatch.Elapsed.TotalMilliseconds);
    }

    private async Task<string> GetBody(Stream stream)
    {
        stream.Position = 0;
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        string content = await reader.ReadToEndAsync();
        stream.Seek(0,SeekOrigin.Begin);
        return content;
    }

    public async void OnActionExecuted(ActionExecutedContext context)
    {
    }
}