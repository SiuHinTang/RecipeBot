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

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using RecipeBot.Discord.Data;

namespace RecipeBot.Discord.Controllers;

/// <summary>
/// Interface for describing a controller dealing with interactions with recipe entries.
/// </summary>
public interface IRecipeEntriesController
{
    /// <summary>
    /// Lists all recipes.
    /// </summary>
    /// <returns>A collection of messages containing formatted recipe entries.</returns>
    Task<ControllerResult<IReadOnlyList<string>>> ListAllRecipesAsync();

    /// <summary>
    /// Lists all recipes filtered by category.
    /// </summary>
    /// <param name="category">The category to filter the recipes on.</param>
    /// <returns>A collection of messages containing formatted recipe entries.</returns>
    /// <exception cref="InvalidEnumArgumentException">Thrown when <paramref name="category"/> is an invalid value of <see cref="DiscordRecipeCategory"/>.</exception>
    Task<ControllerResult<IReadOnlyList<string>>> ListAllRecipesAsync(DiscordRecipeCategory category);
}