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
using Common.Utils;
using RecipeBot.Domain.Models;

namespace RecipeBot.Domain.TestUtils;

/// <summary>
/// Class which creates instances of <see cref="RecipeDomainEntity"/> that can be used for testing.
/// </summary>
public class RecipeDomainModelTestFactory
{
    private readonly int maxTitleLength;
    private readonly int maxAuthorNameLength;
    private readonly int maxFieldNameLength;
    private readonly int maxFieldDataLength;

    /// <summary>
    /// Creates a new instance of <see cref="RecipeDomainModelTestFactory"/>.
    /// </summary>
    /// <param name="constructionProperties">The <see cref="ConstructionProperties"/> to construct the factory with.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="constructionProperties"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="constructionProperties"/> contains invalid values.</exception>
    public RecipeDomainModelTestFactory(ConstructionProperties constructionProperties)
    {
        constructionProperties.IsNotNull(nameof(constructionProperties));

        constructionProperties.MaxTitleLength.IsValidArgument(i => i > 0, $"{nameof(constructionProperties.MaxTitleLength)} must be larger than 0.",
                                                              nameof(constructionProperties.MaxTitleLength));
        constructionProperties.MaxAuthorNameLength.IsValidArgument(i => i > 0, $"{nameof(constructionProperties.MaxAuthorNameLength)} must be larger than 0.",
                                                                   nameof(constructionProperties.MaxAuthorNameLength));
        constructionProperties.MaxFieldNameLength.IsValidArgument(i => i > 0, $"{nameof(constructionProperties.MaxFieldNameLength)} must be larger than 0.",
                                                                  nameof(constructionProperties.MaxFieldNameLength));
        constructionProperties.MaxFieldDataLength.IsValidArgument(i => i > 0, $"{nameof(constructionProperties.MaxFieldDataLength)} must be larger than 0.",
                                                                  nameof(constructionProperties.MaxFieldDataLength));

        maxTitleLength = constructionProperties.MaxTitleLength;
        maxAuthorNameLength = constructionProperties.MaxAuthorNameLength;
        maxFieldNameLength = constructionProperties.MaxFieldNameLength;
        maxFieldDataLength = constructionProperties.MaxFieldDataLength;
    }

    /// <summary>
    /// Creates a default <see cref="RecipeDomainEntity"/> without fields and an image.
    /// </summary>
    /// <returns>A <see cref="RecipeDomainEntity"/>.</returns>
    public RecipeDomainEntity Create()
    {
        return CreateRecipeModel(Enumerable.Empty<RecipeFieldModel>());
    }

    /// <summary>
    /// Creates a default <see cref="RecipeDomainEntity"/> with fields and without an image.
    /// </summary>
    /// <returns>A <see cref="RecipeDomainEntity"/>.</returns>
    public RecipeDomainEntity CreateWithFields()
    {
        return CreateRecipeModel(new[]
        {
            CreateRecipeFiledModel(1),
            CreateRecipeFiledModel(2),
            CreateRecipeFiledModel(3)
        });
    }

    /// <summary>
    /// Creates a default <see cref="RecipeDomainEntity"/> with an image and without fields.
    /// </summary>
    /// <returns>A <see cref="RecipeDomainEntity"/>.</returns>
    public RecipeDomainEntity CreateWithImage()
    {
        return CreateRecipeModel(Enumerable.Empty<RecipeFieldModel>(), "https://recipeBot.recipe.image");
    }

    /// <summary>
    /// Creates a default <see cref="RecipeDomainEntity"/> with image and fields.
    /// </summary>
    /// <returns>A <see cref="RecipeDomainEntity"/>.</returns>
    public RecipeDomainEntity CreateWithImageAndFields()
    {
        return CreateRecipeModel(new[]
        {
            CreateRecipeFiledModel(1),
            CreateRecipeFiledModel(2),
            CreateRecipeFiledModel(3)
        }, "https://recipeBot.recipe.image");
    }

    private RecipeDomainEntity CreateRecipeModel(IEnumerable<RecipeFieldModel> fieldDomainEntities)
    {
        string title = GetStringWithRandomLength('x', maxTitleLength);
        return new RecipeDomainEntity(CreateAuthorModel(), fieldDomainEntities, title);
    }

    private RecipeDomainEntity CreateRecipeModel(IEnumerable<RecipeFieldModel> fieldDomainEntities, string imageUrl)
    {
        string title = GetStringWithRandomLength('x', maxTitleLength);
        return new RecipeDomainEntity(CreateAuthorModel(), fieldDomainEntities, title, imageUrl);
    }

    private AuthorModel CreateAuthorModel()
    {
        string authorName = GetStringWithRandomLength('+', maxAuthorNameLength);
        return new AuthorModel(authorName, "https://recipebot.author.image");
    }

    private RecipeFieldModel CreateRecipeFiledModel(int seed)
    {
        string fieldName = GetStringWithRandomLength(seed, '-', maxFieldNameLength);
        string fieldData = GetStringWithRandomLength(seed, '=', maxFieldDataLength);

        return new RecipeFieldModel(fieldName, fieldData);
    }

    private static string GetStringWithRandomLength(char character, int maximumStringLength)
    {
        return GetStringWithRandomLength(21, character, maximumStringLength);
    }

    private static string GetStringWithRandomLength(int seed, char character, int maximumStringLength)
    {
        var random = new Random(seed);
        int stringLength = maximumStringLength - random.Next(0, maximumStringLength);
        return new string(character, stringLength);
    }

    /// <summary>
    /// Class holding construction variables for <see cref="RecipeDomainModelTestFactory"/>.
    /// </summary>
    public class ConstructionProperties
    {
        /// <summary>
        /// Gets or sets the maximum title length.
        /// </summary>
        public int MaxTitleLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum author name length.
        /// </summary>
        public int MaxAuthorNameLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum field name length.
        /// </summary>
        public int MaxFieldNameLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum field data length.
        /// </summary>
        public int MaxFieldDataLength { get; set; }
    }
}