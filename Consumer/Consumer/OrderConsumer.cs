using MassTransit;
using MessageContract.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Consumer
{
    public class OrderConsumer : IConsumer<IOrderEvent>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly List<Guid> Ids;
        public OrderConsumer(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            Ids = new List<Guid>();
        }
        public async Task Consume(ConsumeContext<IOrderEvent> context)
        {
            try
            {
                if (Ids.Contains(context.Message.OrderId))
                    return;
                Ids.Add(context.Message.OrderId);
                var json = JsonConvert.SerializeObject(context.Message);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("event/createorderevent", data);

                var body = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    _logger.LogError("{@type} {@details} {@message} {@request}", "cosumererror", body, "", json);
            }
            catch (Exception ex)
            {
                _logger.LogError("{@type} {@details} {@message} {@request}", "cosumererror", "", ex.Message, "");
                throw;
            }
            finally
            {
                Ids.Remove(context.Message.OrderId);
            }
        }
    }
}
