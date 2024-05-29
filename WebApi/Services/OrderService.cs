using Microsoft.Extensions.Options;

namespace WebApi;

public class OrderService
{
    private delegate Task OrderProcessHandler(Order order);
    private OrderProcessHandler orderHandler = default!;

    public OrderService(IOptionsMonitor<CloudServicesOptions> optionsMonitor)
    {
        SetHandler(optionsMonitor.CurrentValue);
        optionsMonitor.OnChange((options)=>{
            SetHandler(options);
        });
    }

    public async Task Process(Order order)
    {
         orderHandler.Invoke(order);
    }

    private void SetHandler(CloudServicesOptions options)
    {
        if(options.Persist)
        {
            orderHandler = SaveBoth;
        }
        else
        {
            orderHandler = SaveToFile;
        }
    }
    private Task SaveToFile(Order order)
    {
        return File.WriteAllTextAsync($"order.json", order.ToString());
    }

    private Task SaveToCloud(Order order)
    {
        return Task.CompletedTask;
    }

    private async Task SaveBoth(Order order)
    {
        await SaveToCloud(order);
        await SaveToFile(order);
    }
}
