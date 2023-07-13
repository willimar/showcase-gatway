using showcase.gatway.Extensions;
using System.Net;

namespace showcase.gatway.Delegates.Showcase.Authenticate
{
    public class RegisterAccountHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content is null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("No body found.") };
            }

            var url = @"localhost";

            PublishOption publishOption = new(ChannelOption.Factory("register-account", url))
            {
                Body = new ReadOnlyMemory<byte>(await request.Content.ReadAsByteArrayAsync(cancellationToken)),
                Headers = request.Headers.ToDictionary(),
            };


            publishOption.BasicPublish();

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }
}
