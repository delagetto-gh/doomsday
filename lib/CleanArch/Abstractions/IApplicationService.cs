using System.Threading.Tasks;

namespace CleanArch.Abstractions;

public interface IApplicationService
{
    Task<TUseCaseResult> Handle<TUseCaseResult>(IUseCase<TUseCaseResult> useCase) where TUseCaseResult : IUseCaseResult;
}


