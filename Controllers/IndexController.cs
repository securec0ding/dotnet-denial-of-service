using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Backend.Controllers
{
    public class IndexController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public ContentResult GetChart()
        {
            var html = System.IO.File.ReadAllText("Chart/index.html");
            return Content(html, "text/html", Encoding.UTF8);
        }

        [HttpGet]
        [Route("/Chart/js/{fileName}")]
        public ContentResult GetJs(string fileName)
        {
            var js = System.IO.File.ReadAllText($"Chart/js/{fileName}");
            return Content(js, "text/javascript", Encoding.UTF8);
        }

        [HttpGet]
        [Route("/Chart/css/{fileName}")]
        public ContentResult GetCss(string fileName)
        {
            var css = System.IO.File.ReadAllText($"Chart/css/{fileName}");
            return Content(css, "text/css", Encoding.UTF8);
        }
    }
}