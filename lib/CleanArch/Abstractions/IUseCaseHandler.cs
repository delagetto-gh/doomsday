using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.Abstractions;

public interface IUseCaseHandler<in TUseCase, TUseCaseResult> : IUseCaseHandler
                                                             where TUseCase : IUseCase<TUseCaseResult>
{
    Task<TUseCaseResult> Handle(TUseCase useCase, CancellationToken stoppingToken);
}

public interface IUseCaseStreamingHandler<in TUseCase, TUseCaseResult> : IUseCaseHandler
                                                                      where TUseCase : IUseCase<TUseCaseResult>
{
    IAsyncEnumerable<TUseCaseResult> Handle(TUseCase useCase, CancellationToken stoppingToken);
}

public interface IUseCaseHandler
{ }


