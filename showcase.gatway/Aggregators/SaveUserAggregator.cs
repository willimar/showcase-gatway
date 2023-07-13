using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net.Http.Formatting;

namespace showcase.gatway.Aggregators
{
    public class SaveUserAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var response = new HttpResponseMessage();

            try
            {
                var content1 = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
                var content2 = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

                var sample = new { content1, content2 };

                response.Content = new ObjectContent<object>(sample, new JsonMediaTypeFormatter());
            }
            catch (Exception)
            {

                throw;
            }

            return new DownstreamResponse(response) { };
        }
    }
}
