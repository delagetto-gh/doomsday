using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArch.Abstractions;
using Core.Application.Contracts.UseCases;

public class PingUseCaseHandler : IUseCaseHandler<Ping.UseCase, string>
{
    public async Task<string> Handle(Ping.UseCase useCase, CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5)); //sim some latency
        
        return "Pong";
    }
}
