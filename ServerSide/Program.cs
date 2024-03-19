using System.Net;

using HttpListener listener = new HttpListener();

listener.Prefixes.Add("http://localhost:27003/");

listener.Start();

while (true)
{
    var context = listener.GetContext();

    _ = Task.Run(() =>
    {
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    response.ContentType = "text/html";
    response.Headers.Add("Content-Type", "text/html");
    response.Headers.Add("Server", "Step");
    response.Headers.Add("Date", DateTime.Now.ToString());

    var url = request.RawUrl;
    Console.WriteLine(url);

        if (url == "/")
        {
            response.StatusCode = 200;

            using var writer = new StreamWriter(response.OutputStream);

            var index = File.ReadAllText("MySite/index.html.txt");
            writer.Write(index);
        }
        else
        {

            var urls = url?.Split('/').ToList();

            if (urls[1] == "MySite")
            {
                var files = Directory.GetFiles(urls[1]);

                foreach (var file in files)
                    Console.WriteLine(file);

                response.StatusCode = 200;
                using var writer = new StreamWriter(response.OutputStream);
                var index = File.ReadAllText($"MySite\\{urls[2]}.txt");
                writer.Write(index);
            }
        }
    });
}