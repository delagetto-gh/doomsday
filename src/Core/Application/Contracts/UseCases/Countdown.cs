using System;
using CleanArch.Abstractions;

namespace Core.Application.Contracts.UseCases;

public static class Countdown
{
    public class UseCase : IUseCase<DateTimeOffset>
    { }
}