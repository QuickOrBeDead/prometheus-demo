var httpClient = new HttpClient
                     {
                         BaseAddress = new Uri("http://localhost:8181")
                     };

Console.WriteLine($"started at {DateTime.Now}");

var iterationCount = 1800;
var tasks = new[]
                {
                    Task.Run(
                        async () =>
                            {
                                for (var i = 0; i < iterationCount; i++)
                                {
                                    await httpClient.GetAsync("demo/OperationDuration");
                                    Thread.Sleep(500);
                                }
                            }),
                    Task.Run(
                        () =>
                            {
                                for (var i = 0; i < iterationCount / 10; i++)
                                {
                                    var workers = new Task[10];
                                    for (var j = 0; j < 10; j++)
                                    {
                                        workers[j] = Task.Run(
                                            async () =>
                                                {
                                                    await httpClient.GetAsync("demo/OperationsInProgress");
                                                    Thread.Sleep(250);
                                                });
                                    }

                                    Task.WaitAll(workers);
                                }
                            }),
                    Task.Run(
                        async () =>
                            {
                                for (var i = 0; i < iterationCount; i++)
                                {
                                    await httpClient.GetAsync("demo/OperationException");
                                    Thread.Sleep(500);
                                }
                            })
                };

Task.WaitAll(tasks);

Console.WriteLine($"ended at {DateTime.Now}");

