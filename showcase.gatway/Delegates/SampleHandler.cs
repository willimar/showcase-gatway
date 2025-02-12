﻿namespace showcase.gatway.Delegates
{
    public class SampleHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //do stuff and optionally call the base handler..
            //return await base.SendAsync(request, cancellationToken);
            await Task.CompletedTask;
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }
}
