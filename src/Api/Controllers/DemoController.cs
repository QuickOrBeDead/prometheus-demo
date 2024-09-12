namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using Prometheus;
using System.IO;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private static readonly Histogram OperationDurationHistogram = Metrics.CreateHistogram("operation_duration_seconds", "Operation call processing durations.");
    private static readonly Gauge OperationsInProgressGauge = Metrics.CreateGauge("operations_in_progress", "Number of operations ongoing.");
    private static readonly Counter OperationsFailedWithInvalidOperationExceptionCounter = Metrics.CreateCounter("operations_failed_invalid_operation_exception_total", "Number operations that failed with InvalidOperationException.");

    [HttpGet("OperationDuration")]
    public IActionResult OperationDuration()
    {
        using (OperationDurationHistogram.NewTimer())
        {
            Thread.Sleep(Random.Shared.Next(100, 5000));
        }

        return Ok();
    }

    [HttpGet("OperationsInProgress")]
    public IActionResult OperationsInProgress()
    {
        using (OperationsInProgressGauge.TrackInProgress())
        {
            Thread.Sleep(Random.Shared.Next(100, 5000));
        }

        return Ok();
    }

    [HttpGet("OperationException")]
    public IActionResult OperationException()
    {
        bool IsImportRelatedException(Exception ex)
        {
            return ex is InvalidOperationException;
        }

        OperationsFailedWithInvalidOperationExceptionCounter.CountExceptions(
            () =>
                {
                    if (Random.Shared.Next(1, 10) <= 3)
                    {
                        throw new InvalidOperationException();
                    }
                }, 
            IsImportRelatedException);

        return StatusCode(500);
    }
}