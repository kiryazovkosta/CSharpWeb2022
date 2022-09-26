using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

const string HtmlForm = @"
<div><h1>Input form</h1></div>
<form action='/html' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

const string DownloadForm = @"
<div><h1>Download form</h1></div>
<form action='/content' method='POST'>
    <input type='submit' value ='Download' />
</form>";

await DownloadSitesAsTextFile("content.txt", new string[] { "https://judge.softuni.org/", "https://softuni.org/" });

var server = new HttpServer(routes => routes
    .MapGet("/", new TextResponse("Hello from the server!"))
    .MapGet("/redirect", new RedirectResponse("https://softuni.bg"))
    .MapGet("/content", new HtmlResponse(DownloadForm))
    .MapPost("/content", new TextFileResponse("content.txt"))
    .MapGet("/html", new HtmlResponse(HtmlForm))
    .MapPost("/html", new TextResponse("", AddFormDataAction))
    .MapGet("/cookies", new HtmlResponse("", AddCookiesAction))
    );
await server.Start();

static void AddFormDataAction(Request request, Response response)
{
    response.Body = "";
    foreach (var (key, value) in request.Form)
    {
        response.Body += $"{key} - {value}";
        response.Body += Environment.NewLine;
    }
}

static async Task<string> DownloadWebsiteContent(string url)
{
    var client = new HttpClient();
    using (client)
    {
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return content.Substring(0, 3000);
    }
}

static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
{
    var downloads = new List<Task<string>>();
    foreach (var url in urls)
    {
        downloads.Add(DownloadWebsiteContent(url));
    }

    var responses = await Task.WhenAll(downloads);
    var responsesString = string.Join(
        Environment.NewLine + new string('-', 100) + Environment.NewLine, 
        responses);

    await File.WriteAllTextAsync(fileName, responsesString);
}

static void AddCookiesAction(Request request, Response response)
{
    bool requestHasCookies = request.Cookies.Any();
    string bodyText = string.Empty;

    if (requestHasCookies)
    {
        var cookieText = new StringBuilder();
        cookieText.Append("<h1>Cookies</h1>");
        cookieText.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

        foreach (var cookie in request.Cookies)
        {
            cookieText.Append("<tr>");
            cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
            cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
            cookieText.Append("</tr>");
        }

        cookieText.Append("</table>");
        bodyText = cookieText.ToString();
    }
    else
    {
        bodyText = "<h1>Cookies set!</h1>";
    }

    response.Body = bodyText;

    if (!requestHasCookies)
    {
        response.Cookies.Add("My-Cookie", "My-Value");
        response.Cookies.Add("My-Second-Cookie", "My-Second-Value");

    }
}
