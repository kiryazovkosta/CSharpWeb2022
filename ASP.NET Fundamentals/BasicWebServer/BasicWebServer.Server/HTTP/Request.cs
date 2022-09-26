using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BasicWebServer.Server.HTTP
{
    public class Request
    {
        public Method Method { get; private set; }
        public string Url { get; private set; } = null!;
        public HeaderCollection Headers { get; private set; } = null!;
        public CookieCollection Cookies { get; private set; } = null!;
        public string Body { get; private set; } = null!;
        public Session Session { get; private set; } = null!;
        public IReadOnlyDictionary<string, string> Form { get; private set; } = null!;

        private static Dictionary<string, Session> Sessions = new();

        public static Request Parse(string request)
        {
            var lines = request.Split("\r\n");
            var startLine = lines.First().Split(" ");

            var method = ParseMethod(startLine[0]);
            var url = startLine[1];

            var headers = ParseHeaders(lines.Skip(1));
            var cookies = ParseCookies(headers);
            var session = GetSession(cookies);

            var bodyLines = lines.Skip(headers.Count+ 2).ToArray();
            var body = string.Join("\r\n", bodyLines);
            var form = ParseForm(headers, body);

            return new Request
            {
                Method = method,
                Url = url,
                Headers = headers,
                Cookies = cookies,
                Body = body,
                Session = session,
                Form = form
            };
        }

        private static Method ParseMethod(string method)
        {
            try
            {
                return (Method)Enum.Parse(typeof(Method), method, true);
            }
            catch (Exception)
            {
                Console.WriteLine($"Method '{method}' is not supported");
                return Method.Get;
            }
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headerCollection = new HeaderCollection();
            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty)
                {
                    break;
                }

                var headerParts = headerLine.Split(":", 2);
                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid");
                }

                var headerName = headerParts[0];
                var headerValue = headerParts[1].Trim();
                headerCollection.Add(headerName, headerValue);
            }

            return headerCollection;
        }

        private static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookieCollection = new CookieCollection();
            if (headers.Contains(Header.Cookie))
            {
                var cookieHeader = headers[Header.Cookie];
                string[] allCookies = cookieHeader.Split(";");
                foreach (var cookieText in allCookies)
                {
                    string[] cookieParts = cookieText.Split("=");
                    string cookieName = cookieParts[0].Trim();
                    string cookieValue = cookieParts[1].Trim();
                    cookieCollection.Add(cookieName, cookieValue);
                }
            }

            return cookieCollection;
        }

        private static Dictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseFromData(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }

            return formCollection;
        }

        private static Dictionary<string, string> ParseFromData(string bodyLines)
            => HttpUtility.UrlDecode(bodyLines)
                .Split('&')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(
                    part => part[0],
                    part => part[1],
                    StringComparer.InvariantCultureIgnoreCase);
    }
}
