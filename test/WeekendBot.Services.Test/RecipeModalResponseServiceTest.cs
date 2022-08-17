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

using Discord;
using NSubstitute;
using Xunit;

namespace WeekendBot.Services.Test;

public class RecipeModalResponseServiceTest
{
    [Fact]
    public void Recipe_with_user_data_returns_expected_response()
    {
        // Setup
        const string authorName = "Recipe author";
        const string authorImageUrl = "https://AuthorImage.url";
        var user = Substitute.For<IUser>();
        user.Username.Returns(authorName);
        user.GetAvatarUrl().ReturnsForAnyArgs(authorImageUrl);

        const string recipeTitle = "Recipe title";
        const string recipeIngredients = "My ingredients";
        const string recipeSteps = "My recipe steps";
        const string recipeNotes = "My notes";

        var modal = new RecipeModal
        {
            RecipeTitle = recipeTitle,
            Ingredients = recipeIngredients,
            CookingSteps = recipeSteps,
            Notes = recipeNotes
        };

        // Call
        Embed response = RecipeModalResponseService.GetRecipeModalResponse(modal, user);

        // Assert
        AssertCommonEmbedResponseProperties(user, modal, response);
        Assert.Null(response.Image);
    }

    [Fact]
    public void Recipe_with_user_data_and_attachment_returns_expected_response()
    {
        // Setup
        const string authorName = "Recipe author";
        const string authorImageUrl = "https://AuthorImage.url";
        var user = Substitute.For<IUser>();
        user.Username.Returns(authorName);
        user.GetAvatarUrl().ReturnsForAnyArgs(authorImageUrl);

        const string recipeImageUrl = "https://RecipeImage.url";
        var attachment = Substitute.For<IAttachment>();
        attachment.ContentType.Returns("image/");
        attachment.Url.Returns(recipeImageUrl);

        const string recipeTitle = "Recipe title";
        const string recipeIngredients = "My ingredients";
        const string recipeSteps = "My recipe steps";
        const string recipeNotes = "My notes";
        var modal = new RecipeModal
        {
            RecipeTitle = recipeTitle,
            Ingredients = recipeIngredients,
            CookingSteps = recipeSteps,
            Notes = recipeNotes
        };

        // Call
        Embed response = RecipeModalResponseService.GetRecipeModalResponse(modal, user, attachment);

        // Assert
        AssertCommonEmbedResponseProperties(user, modal, response);

        EmbedImage? embedImage = response.Image;
        Assert.NotNull(embedImage);
        EmbedImage resultImage = embedImage!.Value;
        Assert.Equal(recipeImageUrl, resultImage.Url);
    }

    private static void AssertCommonEmbedResponseProperties(IUser user, RecipeModal modal, IEmbed actualResponse)
    {
        EmbedAuthor? actualResponseAuthor = actualResponse.Author;
        Assert.NotNull(actualResponseAuthor);
        EmbedAuthor embedAuthor = actualResponseAuthor!.Value;
        AssertAuthor(user.Username, user.GetAvatarUrl(), embedAuthor);

        Assert.Equal(modal.RecipeTitle, actualResponse.Title);

        Assert.Equal(3, actualResponse.Fields.Length);
        AssertField("Ingredients", modal.Ingredients, actualResponse.Fields[0]);
        AssertField("Cooking steps", modal.CookingSteps, actualResponse.Fields[1]);
        AssertField("Additional notes", modal.Notes, actualResponse.Fields[2]);
    }

    private static void AssertAuthor(string expectedAuthorName, string expectedAuthorImageUrl, EmbedAuthor actualAuthor)
    {
        Assert.Equal(expectedAuthorName, actualAuthor.Name);
        Assert.Equal(expectedAuthorImageUrl, actualAuthor.IconUrl);
    }

    private static void AssertField(string expectedName, string? expectedValue, EmbedField actualField)
    {
        Assert.Equal(expectedName, actualField.Name);
        Assert.Equal(expectedValue, actualField.Value);
        Assert.False(actualField.Inline);
    }
}