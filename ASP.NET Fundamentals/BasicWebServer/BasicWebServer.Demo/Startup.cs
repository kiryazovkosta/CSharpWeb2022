using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

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
    .MapPost("/html", new TextResponse("", AddFormDataAction)));
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
