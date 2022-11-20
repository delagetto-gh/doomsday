using CleanArch.Abstractions;

namespace Core.Application.Contracts.UseCases;

public static class Ping
{
    public class UseCase : IUseCase<UseCaseResult>
    {

    }

    public class UseCaseResult : IUseCaseResult
    {
        public string Value { get; set; }
    }
}