using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace showcase.gatway.Aggregators
{
    public class SwaggerDetailAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
            };

            return new DownstreamResponse(response) { };
        }
    }
}
