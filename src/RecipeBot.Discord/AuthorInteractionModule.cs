﻿// Copyright (C) 2022 Dennis Tang. All rights reserved.
//
// This file is part of RecipeBot.
//
// RecipeBot is free software: you can redistribute it and/or modify
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Utils;
using Discord;
using Discord.Common.Services;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using RecipeBot.Discord.Controllers;
using RecipeBot.Discord.Properties;

namespace RecipeBot.Discord;

/// <summary>
/// Module containing commands to interact with author entries.
/// </summary>
public class AuthorInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILoggingService logger;

    /// <summary>
    /// Creates a new instance of <see cref="AuthorInteractionModule"/>.
    /// </summary>
    /// <param name="scopeFactory">The <see cref="IServiceScopeFactory"/> to resolve dependencies with.</param>
    /// <param name="logger">The logger to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is <c>null</c>.</exception>
    public AuthorInteractionModule(IServiceScopeFactory scopeFactory, ILoggingService logger)
    {
        scopeFactory.IsNotNull(nameof(scopeFactory));
        logger.IsNotNull(nameof(logger));

        this.scopeFactory = scopeFactory;
        this.logger = logger;
    }

    [SlashCommand("myuserdata-delete-all", "Deletes all user data")]
    public async Task DeleteAuthor()
    {
        try
        {
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var controller = scope.ServiceProvider.GetRequiredService<IAuthorController>();
                ControllerResult<string> response = await controller.DeleteAuthorAsync(Context.User);
                if (response.HasError)
                {
                    await RespondAsync(string.Format(Resources.InteractionModule_ERROR_0_, response.ErrorMessage), ephemeral: true);
                }
                else
                {
                    await RespondAsync(response.Result, ephemeral: true);
                }
            }
        }
        catch (Exception e)
        {
            Task[] tasks =
            {
                RespondAsync(e.Message, ephemeral: true),
                logger.LogErrorAsync(e)
            };

            await Task.WhenAll(tasks);
        }
    }

    [SlashCommand("author-list", "Lists all stored authors in the database")]
    [DefaultMemberPermissions(GuildPermission.Administrator | GuildPermission.ModerateMembers)]
    public async Task ListAuthors()
    {
        try
        {
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var controller = scope.ServiceProvider.GetRequiredService<IAuthorController>();
                IEnumerable<Task> tasks = await GetTasksAsync(controller.GetAllAuthorsAsync());

                await Task.WhenAll(tasks);
            }
        }
        catch (Exception e)
        {
            Task[] tasks =
            {
                RespondAsync(e.Message, ephemeral: true),
                logger.LogErrorAsync(e)
            };

            await Task.WhenAll(tasks);
        }
    }

    private async Task<IEnumerable<Task>> GetTasksAsync(Task<ControllerResult<IReadOnlyList<string>>> getControllerResultTask)
    {
        ControllerResult<IReadOnlyList<string>> result = await getControllerResultTask;
        if (result.HasError)
        {
            return new[]
            {
                RespondAsync(string.Format(Resources.InteractionModule_ERROR_0_, result.ErrorMessage), ephemeral: true)
            };
        }

        IReadOnlyList<string> messages = result.Result!;
        if (!messages.Any())
        {
            return new[]
            {
                RespondAsync(string.Format(Resources.InteractionModule_ERROR_0_,
                                           Resources.Controller_should_not_have_returned_an_empty_collection_when_querying),
                             ephemeral: true)
            };
        }

        var tasks = new List<Task>
        {
            RespondAsync(messages[0], ephemeral: true)
        };
        for (var i = 1; i < messages.Count; i++)
        {
            tasks.Add(FollowupAsync(messages[i], ephemeral: true));
        }

        return tasks;
    }
}