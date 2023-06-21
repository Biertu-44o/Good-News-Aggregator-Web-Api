namespace MVC.Middlware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404)
            {
                String html =
                    "<html>\r\n<head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <style>\r\n        body, html {\r\n            height: 100%;\r\n            margin: 0;\r\n        }\r\n\r\n        .bg {\r\n            background-image: url(\"/images/404.jpg\");\r\n            height: 100%;\r\n            background-position: center;\r\n            background-repeat: no-repeat;\r\n            background-size: cover;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div  class=\"bg\"></div>\r\n</body>\r\n</html><script>\r\n  document.title = \"404\";\r\n</script>";
                await context.Response.WriteAsync(html);
            }

            if (context.Response.StatusCode == 500)
            {
                String html =
                    "<html>\r\n<head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <style>\r\n        body, html {\r\n            height: 100%;\r\n            margin: 0;\r\n        }\r\n\r\n        .bg {\r\n            background-image: url(\"/images/500.gif\");\r\n            height: 100%;\r\n            background-position: center;\r\n            background-repeat: no-repeat;\r\n            background-size: cover;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div  class=\"bg\"></div>\r\n</body>\r\n</html><script>\r\n  document.title = \"500\";\r\n</script>";
                await context.Response.WriteAsync(html);
            }
        }
    }

}
