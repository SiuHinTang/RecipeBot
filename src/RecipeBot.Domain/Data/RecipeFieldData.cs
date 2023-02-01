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
using Common.Utils;

namespace RecipeBot.Domain.Data;

/// <summary>
/// Class containing information about the fields of a recipe.
/// </summary>
public class RecipeFieldData
{
    /// <summary>
    /// Creates a new instance of <see cref="RecipeFieldData"/>.
    /// </summary>
    /// <param name="fieldName">The name of the recipe field.</param>
    /// <param name="fieldData">The data of the recipe field.</param>
    /// <exception cref="ArgumentException">Thrown when any parameter is <c>null</c> or consists of whitespaces.</exception>
    public RecipeFieldData(string fieldName, string fieldData)
    {
        fieldName.IsNotNullOrWhiteSpaces(nameof(fieldName));
        fieldData.IsNotNullOrWhiteSpaces(nameof(fieldData));

        FieldName = fieldName;
        FieldData = fieldData;
    }

    /// <summary>
    /// Gets the name of the recipe field.
    /// </summary>
    public string FieldName { get; }

    /// <summary>
    /// Gets the data of the recipe field.
    /// </summary>
    public string FieldData { get; }
}