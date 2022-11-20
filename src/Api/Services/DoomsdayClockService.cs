using System;
using System.Threading;
using System.Threading.Tasks;
using Doomsday;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Api.Services;

/// <summary>
/// gRPC Api for <see cref="DoomsdayClock"/> 
/// </summary>
public class DoomsdayClockService : Doomsday.DoomsdayClock.DoomsdayClockBase
{
    private readonly PeriodicTimer _timer;

    public DoomsdayClockService()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    }

    public override async Task GetCountdown(GetCountdownRequest request, IServerStreamWriter<GetCountdownResponse> responseStream, ServerCallContext context)
    {
        var stoppingtoken = context.CancellationToken;
        while (await _timer.WaitForNextTickAsync(stoppingtoken))
        {
            var now = DateTimeOffset.Now;
            var response = new GetCountdownResponse
            {
                Time = Timestamp.FromDateTimeOffset(now)
            };
            await responseStream.WriteAsync(response);
        }
    }

    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PingResponse
        {
            Value = "Pong"
        });
    }
}