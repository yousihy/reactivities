using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using JsonNet.ContractResolvers;
using Domain;
using Persistence;

public static class Seeder
{
    public static async Task SeedActivities(string jsonData,IServiceProvider serviceProvider)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new PrivateSetterContractResolver()
        };
        List<Activity> activities = JsonConvert.DeserializeObject<List<Activity>>(jsonData, settings);
        using (
         var serviceScope = serviceProvider
           .GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope
                          .ServiceProvider.GetService<DataContext>();
            if (!context.Activities.Any())
            {
                await context.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }
        }
    }
}