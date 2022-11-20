using System;
using System.Threading.Tasks;
using CleanArch.Abstractions;
using Core.Application.Contracts.UseCases;

public class PingUseCaseHandler : IUseCaseHandler<Ping.UseCase, Ping.UseCaseResult>
{
    public async Task<Ping.UseCaseResult> Handle(Ping.UseCase useCase)
    {
        await Task.Delay(TimeSpan.FromSeconds(5)); //sim some latency
        
        return new Ping.UseCaseResult
        {
            Value = "Pong"
        };
    }
}
