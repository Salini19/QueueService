using QueueService;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<MonitorLoop>();
        services.AddHostedService<QueuedHostedService>();
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            if (!int.TryParse(context.Configuration["QueueCapacity"],
                out int queueCapacity))
            {
                queueCapacity = 3;
            }

            return new DefaultBackgroundTaskQueue(queueCapacity);
        });
    });

IHost host = builder.Build();

MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
monitorLoop.StartMonitorLoop();

host.Run();

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<QueuedHostedService>();
//    })
//    .Build();

//await host.RunAsync();
