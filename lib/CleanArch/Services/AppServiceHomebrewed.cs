using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Abstractions;

// app seervice which just relies on IOC -
// does NOT take dep on IServiceProvider (service-locator anti-pattern )
// does NOT use mediatR (for scenarios where using 3rd party libs are not allowed)
public class AppServiceHomebrewed : IApplicationService
{
    private readonly IEnumerable<IUseCaseHandlerMarker> _useCaseHandlers;

    public AppServiceHomebrewed(IEnumerable<IUseCaseHandlerMarker> allUseCaseHanlers)
    {
        _useCaseHandlers = allUseCaseHanlers;
    }

    public async Task<TUseCaseResult> Handle<TUseCaseResult>(IUseCase<TUseCaseResult> useCase) where TUseCaseResult : IUseCaseResult
    {
        var useCaseType = useCase.GetType();
        var useCaseResulType = typeof(TUseCaseResult);
        var targetUseCaseHandlerType = typeof(IUseCaseHandler<,>).MakeGenericType(useCaseType, useCaseResulType);

        var applicableUseCaseHandlers = _useCaseHandlers
        .Where(o => o.GetType().IsAssignableTo(targetUseCaseHandlerType))
        .ToList();

        if (!applicableUseCaseHandlers.Any())
            throw new ArgumentException($"No usecase handler found for use case {useCaseType.Name}", nameof(useCase));

        if (applicableUseCaseHandlers.Count > 1)
            throw new InvalidOperationException(@$"Found more than one usecase handlers for use case {useCaseType.Name}. 
                                                   A usecase can only have a single handler.");

        var useCaseHandler = applicableUseCaseHandlers.Single();

        var handleMethodPtr = useCaseHandler.GetType().GetMethod("Handle");
        var handleMethodTaskPtr = (Task<TUseCaseResult>)handleMethodPtr.Invoke(useCaseHandler, new[] { useCase });
        return await handleMethodTaskPtr;
    }
}


