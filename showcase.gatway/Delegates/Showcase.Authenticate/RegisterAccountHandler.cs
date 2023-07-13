using showcase.gatway.Extensions;
using System.Net;

namespace showcase.gatway.Delegates.Showcase.Authenticate
{
    public class RegisterAccountHandler : DelegatingHandler
    {
        const string Url = "host.docker.internal";
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content is null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("No body found.") };
            }

            if (request.RequestUri is null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Invalid URL.") };
            }

            PublishOption publishOption = new(ChannelOption.Factory(request.RequestUri.AbsolutePath[1..], Url))
            {
                Body = new ReadOnlyMemory<byte>(await request.Content.ReadAsByteArrayAsync(cancellationToken)),
                Headers = request.Headers.ToDictionary(),
            };


            publishOption.BasicPublish();

            return new HttpResponseMessage(HttpStatusCode.Accepted) { Content = new StringContent("Data sent to server.") };
        }
    }
}
