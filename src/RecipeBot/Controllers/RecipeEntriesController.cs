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
using System.Text;
using System.Threading.Tasks;
using Common.Utils;
using Discord;
using RecipeBot.Discord.Controllers;
using RecipeBot.Discord.Data;
using RecipeBot.Domain.Data;
using RecipeBot.Domain.Repositories;
using RecipeBot.Domain.Repositories.Data;
using RecipeBot.Properties;
using RecipeBot.Services;

namespace RecipeBot.Controllers;

/// <summary>
/// A concrete implementation of the <see cref="IRecipeEntriesController"/>.
/// </summary>
public class RecipeEntriesController : IRecipeEntriesController
{
    private readonly IMessageCharacterLimitProvider limitProvider;
    private readonly IRecipeDataEntryCollectionRepository repository;

    /// <summary>
    /// Creates a new instance of <see cref="RecipeEntriesController"/>.
    /// </summary>
    /// <param name="limitProvider">The limit provider to retrieve the message character limits from.</param>
    /// <param name="repository">The repository to handle with the persistence of recipe entries.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is <c>null</c>.</exception>
    public RecipeEntriesController(IMessageCharacterLimitProvider limitProvider,
        IRecipeDataEntryCollectionRepository repository)
    {
        limitProvider.IsNotNull(nameof(limitProvider));
        repository.IsNotNull(nameof(repository));

        this.limitProvider = limitProvider;
        this.repository = repository;
    }

    public async Task<ControllerResult<IReadOnlyList<string>>> ListAllRecipesAsync()
    {
        IReadOnlyList<RecipeEntryData> entries = await repository.LoadRecipeEntriesAsync();

        return new ControllerResult<IReadOnlyList<string>>(CreateMessages(entries, Resources.RecipeEntriesController_ListAllRecipesAsync_No_saved_recipes_are_found));
    }

    public async Task<ControllerResult<IReadOnlyList<string>>> ListAllRecipesAsync(DiscordRecipeCategory category)
    {
        category.IsValidEnum(nameof(category));

        RecipeCategory repositoryCategory = RecipeCategoryConverter.ConvertFrom(category);
        IReadOnlyList<RecipeEntryData> entries = await repository.LoadRecipeEntriesAsync(repositoryCategory);


        return new ControllerResult<IReadOnlyList<string>>(CreateMessages(entries, Resources.RecipeEntriesController_ListAllRecipesAsync_No_saved_recipes_are_found_with_category));
    }

    private IReadOnlyList<string> CreateMessages(IReadOnlyList<RecipeEntryData> entries, string emptyMessage)
    {
        if (!entries.Any())
        {
            return new[]
            {
                emptyMessage
            };
        }
        
        var messages = new List<string>();
        StringBuilder messageBuilder = new StringBuilder().AppendLine(CreateHeader());
        string formattedCurrentMessage = Format.Code(messageBuilder.ToString());

        foreach (RecipeEntryData currentEntry in entries)
        {
            string formattedEntry = CreateFormattedEntry(currentEntry);

            var messageWithCurrentEntry = $"{messageBuilder}{formattedEntry}";
            if (Format.Code(messageWithCurrentEntry).Length > limitProvider.MaxMessageLength)
            {
                messages.Add(formattedCurrentMessage);
                messageBuilder.Clear();
                messageBuilder.AppendLine(CreateHeader());
            }

            messageBuilder.AppendLine(formattedEntry);

            var currentMessage = messageBuilder.ToString();
            formattedCurrentMessage = Format.Code(currentMessage);
        }

        messages.Add(formattedCurrentMessage);
        return messages;
    }

    private static string CreateHeader()
    {
        var header = $"{"Id",-3} {"Title",-50} {"Author",-50} ";
        return header;
    }

    private static string CreateFormattedEntry(RecipeEntryData recipeEntry)
    {
        return $"{recipeEntry.Id,-3} {recipeEntry.Title,-50} {recipeEntry.AuthorName,-50}";
    }
}