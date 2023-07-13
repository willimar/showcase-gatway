namespace showcase.gatway.Delegates.Showcase.Authenticate
{
    public class RegisterUserHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //do stuff and optionally call the base handler..
            //return await base.SendAsync(request, cancellationToken);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }
}
