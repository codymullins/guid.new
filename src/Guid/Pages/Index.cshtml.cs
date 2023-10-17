using Guid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace Guid.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? Count { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var header = Request.Headers.Accept;
            Request.Query.TryGetValue("accept", out var format);
            var resp = GetResponseFormat(format);

            if (resp == ResponseFormat.None && header != "application/json")
            {
                return Page();
            }

            if (header == "application/json")
            {
                resp = ResponseFormat.Json;
            }

            switch (Count)
            {
                case > 100:
                    return BadRequest(new Sorry("The max number allowed is 100"));
                case 0:
                    return Many(0, resp);
                case null:
                    return One(resp);
                case 1:
                    return One(resp);
            }

            return Many(Count.Value, resp);
        }

        private ResponseFormat GetResponseFormat(StringValues format)
        {
            ResponseFormat resp;

            switch (format)
            {
                case "plain":
                    resp = ResponseFormat.Plain;
                    break;
                case "json":
                    resp = ResponseFormat.Json;
                    break;
                default:
                    resp = ResponseFormat.None;
                    break;
            }

            return resp;
        }

        private IActionResult One(ResponseFormat format)
        {
            var value = System.Guid.NewGuid();
            var result = new SingleResult(value);
            return format == ResponseFormat.Plain ? new ContentResult { Content = result?.Value.ToString() } : new JsonResult(result);
        }

        private IActionResult Many(int count, ResponseFormat format)
        {
            var data = new List<System.Guid>();
            var i = 1;
            while (i <= count)
            {
                data.Add(System.Guid.NewGuid());
                i++;
            }

            var result = new ManyResult(data.Select(p => new SingleResult(p)));
            return format == ResponseFormat.Plain ? new ContentResult
            {
                Content = result?.Items?.Select(p => p.Value.ToString()).Aggregate((working, next) => working + "\n" + next)
            } : new JsonResult(result);
        }

    }

    public enum ResponseFormat
    {
        None,
        Plain,
        Json
    }
}