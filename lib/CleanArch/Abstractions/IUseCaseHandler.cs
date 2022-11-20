using System.Threading.Tasks;

namespace CleanArch.Abstractions;

public interface IUseCaseHandler<TUseCase, TUseCaseResult> : IUseCaseHandlerMarker
                                                             where TUseCase : IUseCase<TUseCaseResult>
                                                             where TUseCaseResult : IUseCaseResult
{
    Task<TUseCaseResult> Handle(TUseCase useCase);
}


