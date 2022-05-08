namespace ProcrastinationBlocker.Common
{
    public interface IHttpClient
    {
        public HttpResponseMessage Send(HttpRequestMessage request);
    }
}
