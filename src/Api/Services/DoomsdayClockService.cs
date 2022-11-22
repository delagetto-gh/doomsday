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
    private readonly IApplicationApi _appApi;

    public DoomsdayClockService(IApplicationApi appApi)
    {
        _appApi = appApi;
    }

    public override async Task GetCountdown(GetCountdownRequest request, IServerStreamWriter<GetCountdownResponse> responseStream, ServerCallContext context)
    {
        //map public contract => application contract (could use AutoMapper ACL)
        var useCase = new Countdown.UseCase();

        // execute app  (handle usecase) & get response
        await foreach (var time in _appApi.Stream(useCase, context.CancellationToken))
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
        var useCaseResult = await _appApi.Handle(useCase, context.CancellationToken);

        //map app response to public contract (use AutoMapper ACL)
        var response = new PingResponse
        {
            Value = useCaseResult
        };

        //return the contract response;
        return response;
    }
}