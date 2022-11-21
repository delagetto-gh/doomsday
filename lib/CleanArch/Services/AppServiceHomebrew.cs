using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.Abstractions;

// app service which just relies on DI -
// does NOT take dep on IServiceProvider (service-locator anti-pattern )
// does NOT use mediatR (for scenarios where using 3rd party libs are not allowed)
public partial class AppServiceHomebrew : IApplicationService
{
    private readonly IEnumerable<IUseCaseHandler> _allUseCaseHandlers;

    public AppServiceHomebrew(IEnumerable<IUseCaseHandler> allUseCaseHandlers)
    {
        _allUseCaseHandlers = allUseCaseHandlers;
    }

    public async Task<TUseCaseResult> Handle<TUseCaseResult>(IUseCase<TUseCaseResult> useCase, CancellationToken stoppingToken = default)
    {
        var useCaseHandler = GetUseCaseHandler(useCase, typeof(IUseCaseHandler<,>));
        var handlePtr = useCaseHandler.GetType().GetMethod("Handle");
        var handleTask = (Task<TUseCaseResult>)handlePtr.Invoke(useCaseHandler, new object[] { useCase, stoppingToken});
        return await handleTask;
    }

    public IAsyncEnumerable<TUseCaseResult> Stream<TUseCaseResult>(IUseCase<TUseCaseResult> useCase, CancellationToken stoppingToken = default)
    {
        var useCaseHandler = GetUseCaseHandler(useCase, typeof(IUseCaseStreamingHandler<,>));
        var handlePtr = useCaseHandler.GetType().GetMethod("Handle");
        var handleTask = (IAsyncEnumerable<TUseCaseResult>)handlePtr.Invoke(useCaseHandler, new object[] { useCase, stoppingToken });
        return handleTask;
    }

    private object GetUseCaseHandler<TUseCaseResult>(IUseCase<TUseCaseResult> useCase, Type targetHandlerType)
    {
        var useCaseType = useCase.GetType();
        var useCaseResulType = typeof(TUseCaseResult);
        var genericisedTargetHandlerType = targetHandlerType.MakeGenericType(useCaseType, useCaseResulType);

        var applicableUseCaseHandlers = _allUseCaseHandlers
        .Where(handler => handler.GetType().IsAssignableTo(genericisedTargetHandlerType))
        .ToList();

        if (!applicableUseCaseHandlers.Any())
            throw new ArgumentException($"No usecase handler found for use case {useCaseType.Name}", nameof(useCase));

        if (applicableUseCaseHandlers.Count > 1)
            throw new InvalidOperationException(@$"Found more than one usecase handlers for use case {useCaseType.Name}. 
                                                   A usecase can only have a single handler.");

        var useCaseHandler = applicableUseCaseHandlers.Single();
        return useCaseHandler;
    }
}


