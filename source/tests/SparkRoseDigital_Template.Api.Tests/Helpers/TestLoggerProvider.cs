﻿using System;
using Microsoft.Extensions.Logging;

namespace SparkRoseDigital_Template.Api.Tests.Helpers;

public class TestLoggerProvider : ILoggerProvider
{
    private readonly Action<string> _logTo;
    private readonly LogLevel _level;

    public TestLoggerProvider(Action<string> logTo, LogLevel level)
    {
        _logTo = logTo;
        _level = level;
    }

    public ILogger CreateLogger(string categoryName) =>
        new TestLogger(_logTo, _level);

    public void Dispose()
    {
    }
}