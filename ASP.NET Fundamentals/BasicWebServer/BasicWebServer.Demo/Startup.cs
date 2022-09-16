using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;

const string HtmlForm = @"
<div><h1>Input form</h1></div>
<form action='/html' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

var server = new HttpServer(routes => routes
    .MapGet("/", new TextResponse("Hello from the server!"))
    .MapGet("/redirect", new RedirectResponse("https://softuni.bg"))
    .MapGet("/html", new HtmlResponse(HtmlForm))
    .MapPost("/html", new TextResponse("", AddFormDataAction)));
server.Start();

static void AddFormDataAction(Request request, Response response)
{
    response.Body = "";
    foreach (var (key, value) in request.Form)
    {
        response.Body += $"{key} - {value}";
        response.Body += Environment.NewLine;
    }
}
