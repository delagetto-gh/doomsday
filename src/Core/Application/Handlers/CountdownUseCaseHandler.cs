using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using CleanArch.Abstractions;
using Core.Application.Contracts.UseCases;

public class CountdownUseCaseHandler : IUseCaseStreamingHandler<Countdown.UseCase, DateTimeOffset>
{
    private readonly PeriodicTimer _timer;

    public CountdownUseCaseHandler()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    }

    public async IAsyncEnumerable<DateTimeOffset> Handle(Countdown.UseCase useCase, [EnumeratorCancellation] CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            var now = DateTimeOffset.Now;
            yield return now;
        }
    }
}
