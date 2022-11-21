using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArch.Abstractions;
using Core.Application.Contracts.UseCases;
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
    private readonly IApplicationService _appService;

    public DoomsdayClockService(IApplicationService appService)
    {
        _appService = appService;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    }

    public override async Task GetCountdown(GetCountdownRequest request, IServerStreamWriter<GetCountdownResponse> responseStream, ServerCallContext context)
    {
        //map public contract => application contract (could use AutoMapper ACL)
        var useCase = new Countdown.UseCase();

        // execute app  (handle usecase) & get response
        await foreach (var time in _appService.Stream(useCase, context.CancellationToken))
        {
            //map app response to public contract (use AutoMapper ACL)
            var response = new GetCountdownResponse
            {
                Time = Timestamp.FromDateTimeOffset(time)
            };

            await responseStream.WriteAsync(response);
        }
    }

    public override async Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        //map public contract => application contract (could use AutoMapper ACL)
        var useCase = new Ping.UseCase();

        // execute app  (handle usecase) & get response
        var useCaseResult = await _appService.Handle(useCase, context.CancellationToken);

        //map app response to public contract (use AutoMapper ACL)
        var response = new PingResponse
        {
            Value = useCaseResult
        };

        //return the contract response;
        return response;
    }
}