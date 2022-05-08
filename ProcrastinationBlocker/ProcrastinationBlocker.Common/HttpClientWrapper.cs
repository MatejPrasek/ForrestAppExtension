namespace ProcrastinationBlocker.Common;

public class HttpClientWrapper : IHttpClient
{
    private HttpClient HttpClient { get; }
    public HttpClientWrapper()
    {
        HttpClient = new HttpClient();
    }
    public HttpResponseMessage Send(HttpRequestMessage request)
    {
        return HttpClient.Send(request);
    }
}