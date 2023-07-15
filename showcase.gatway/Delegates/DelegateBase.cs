﻿using showcase.gatway.Extensions;
using System.Net;

namespace showcase.gatway.Delegates
{
    public class DelegateOption
    {
        public string RabbitUrl { get; set; } = string.Empty;
    }

    public delegate void Execute(PublishOption publishOption);
    public delegate void Before(ref string actionName);

    public class DelegateBase : DelegatingHandler
    {
        private readonly DelegateOption _option;

        public event Execute? BeforeExecute;
        public event Before? PrepareAction;

        public DelegateBase(DelegateOption option)
        {
            this._option = option;
        }

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

            var actionName = request.RequestUri.AbsolutePath[1..];

            if (PrepareAction is not null)
            {
                PrepareAction(ref actionName);
            }

            PublishOption publishOption = new(ChannelOption.Factory(actionName, _option.RabbitUrl))
            {
                Body = new ReadOnlyMemory<byte>(await request.Content.ReadAsByteArrayAsync(cancellationToken)),
                Headers = request.Headers.ToDictionary(),
            };

            if (BeforeExecute is not null)
            {
                BeforeExecute(publishOption);
            }

            publishOption.BasicPublish();


            return new HttpResponseMessage(HttpStatusCode.Accepted) { Content = new StringContent("Data sent to server.") };
        }
    }
}
