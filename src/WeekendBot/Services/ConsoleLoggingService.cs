﻿// Copyright (C) 2022 Dennis Tang. All rights reserved.
//
// This file is part of WeekendBot.
//
// WeekendBot is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading.Tasks;
using Discord.Common;
using Discord.Common.Options;
using Microsoft.Extensions.Options;
using WeekendBot.Core;
using WeekendBot.Utils;

namespace WeekendBot.Services;

/// <summary>
/// Console implementation of a logging service.
/// </summary>
public class ConsoleLoggingService : ILoggingService
{
    private readonly ITimeProvider timeProvider;
    private readonly StringFormatOptions formatOptions;

    /// <summary>
    /// Creates a new instance of <see cref="ConsoleLoggingService"/>.
    /// </summary>
    /// <param name="timeProvider">The <see cref="ITimeProvider"/> to retrieve the time with.</param>
    /// <param name="formatOptions">The options to format the messages with.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="timeProvider"/> is <c>null</c>.</exception>
    public ConsoleLoggingService(ITimeProvider timeProvider, IOptions<StringFormatOptions> formatOptions)
    {
        timeProvider.IsNotNull(nameof(timeProvider));
        formatOptions.IsNotNull(nameof(formatOptions));

        this.timeProvider = timeProvider;
        this.formatOptions = formatOptions.Value;
    }

    public async Task LogInfoAsync(string message)
    {
        string logMessage = await GetLogMessage(message);
        Console.WriteLine(logMessage);
    }

    public async Task LogWarningAsync(string message)
    {
        string logMessage = await GetLogMessage(message);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(logMessage);
        Console.ResetColor();
    }

    public async Task LogErrorAsync(string message)
    {
        string logMessage = await GetLogMessage(message);
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(logMessage);
        Console.ResetColor();
    }

    public async Task LogDebugAsync(string message)
    {
#if DEBUG
        string logMessage = await GetLogMessage(message);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(logMessage);
        Console.ResetColor();
#endif
    }

    private async Task<string> GetLogMessage(string message)
    {
        DateTime currentDateTimeAsync = await timeProvider.GetCurrentDateTimeAsync();
        return $"[{formatOptions.Format(currentDateTimeAsync)}]\t{message}";
    }
}