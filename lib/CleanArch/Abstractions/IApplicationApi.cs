using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.Abstractions;

public interface IApplicationApi
{
    Task<TUseCaseResult> Handle<TUseCaseResult>(IUseCase<TUseCaseResult> useCase, CancellationToken stoppingToken = default);
    IAsyncEnumerable<TUseCaseResult> Stream<TUseCaseResult>(IUseCase<TUseCaseResult> useCase, CancellationToken stoppingToken = default);
}


