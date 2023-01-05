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
using System.Linq;
using AutoFixture;
using Discord;
using RecipeBot.Domain.Data;
using RecipeBot.Domain.Models;
using RecipeBot.Domain.TestUtils;
using RecipeBot.Services;
using Xunit;

namespace RecipeBot.Test.Services;

public class RecipeEmbedFactoryTest
{
    private readonly RecipeDomainModelTestBuilder domainModelBuilder;

    public RecipeEmbedFactoryTest()
    {
        domainModelBuilder = new RecipeDomainModelTestBuilder(new RecipeDomainModelTestBuilder.ConstructionProperties
        {
            MaxAuthorNameLength = EmbedAuthorBuilder.MaxAuthorNameLength,
            MaxTitleLength = EmbedBuilder.MaxTitleLength,
            MaxFieldNameLength = EmbedFieldBuilder.MaxFieldNameLength,
            MaxFieldDataLength = EmbedFieldBuilder.MaxFieldValueLength
        });
    }

    [Theory]
    [MemberData(nameof(GetRecipeCategoriesAndColor))]
    public void Basic_recipe_with_category_returns_embed_with_color(
        RecipeCategory category, Color expectedColor)
    {
        // Setup
        RecipeModel recipeModel = domainModelBuilder.SetCategory(category)
                                                    .Build();

        // Call
        Embed embed = RecipeEmbedFactory.Create(recipeModel);

        // Assert
        Assert.Equal(expectedColor, embed.Color);
    }

    [Fact]
    public void Basic_recipe_should_return_embed_without_image_and_fields()
    {
        // Setup
        RecipeModel recipeModel = domainModelBuilder.Build();

        // Call
        Embed embed = RecipeEmbedFactory.Create(recipeModel);

        // Assert
        Assert.Equal(recipeModel.Title, embed.Title);
        Assert.Null(embed.Image);

        AssertAuthor(recipeModel.Author, embed.Author);
        AssertFields(recipeModel.RecipeFields, embed.Fields);

        Assert.Null(embed.Footer);
    }

    [Fact]
    public void Recipe_with_all_data_should_return_embed_with_fields_image_and_footer()
    {
        // Setup
        var fixture = new Fixture();
        var category = fixture.Create<RecipeCategory>();
        RecipeModel recipeModel = domainModelBuilder.SetCategory(category)
                                                    .AddImage()
                                                    .AddTags(new[]
                                                    {
                                                        "Tag1",
                                                        "Tag2"
                                                    })
                                                    .AddFields(3)
                                                    .Build();

        // Call
        Embed embed = RecipeEmbedFactory.Create(recipeModel);

        // Assert
        Assert.Equal(recipeModel.Title, embed.Title);

        EmbedImage? embedImage = embed.Image;
        Assert.NotNull(embedImage);
        Assert.Equal(recipeModel.RecipeImageUrl, embedImage.Value.Url);

        AssertAuthor(recipeModel.Author, embed.Author);
        AssertFields(recipeModel.RecipeFields, embed.Fields);

        EmbedFooter? embedFooter = embed.Footer;
        Assert.NotNull(embedFooter);
        var expectedFooterText = $"{TagTestHelper.CategoryMapping[category]}, Tag1, Tag2";
        Assert.Equal(expectedFooterText, embedFooter.Value.Text);
    }

    [Fact]
    public void Recipe_with_image_should_return_embed_with_image()
    {
        // Setup
        RecipeModel recipeModel = domainModelBuilder.AddImage()
                                                    .Build();

        // Call
        Embed embed = RecipeEmbedFactory.Create(recipeModel);

        // Assert
        Assert.Equal(recipeModel.Title, embed.Title);

        EmbedImage? embedImage = embed.Image;
        Assert.NotNull(embedImage);
        Assert.Equal(recipeModel.RecipeImageUrl, embedImage!.Value.Url);

        AssertAuthor(recipeModel.Author, embed.Author);
        AssertFields(recipeModel.RecipeFields, embed.Fields);

        Assert.Null(embed.Footer);
    }

    [Fact]
    public void Recipe_with_fields_should_return_embed_with_fields()
    {
        // Setup
        RecipeModel recipeModel = domainModelBuilder.AddFields(3)
                                                    .Build();

        // Call
        Embed embed = RecipeEmbedFactory.Create(recipeModel);

        // Assert
        Assert.Equal(recipeModel.Title, embed.Title);

        EmbedImage? embedImage = embed.Image;
        Assert.Null(embedImage);

        AssertAuthor(recipeModel.Author, embed.Author);
        AssertFields(recipeModel.RecipeFields, embed.Fields);

        Assert.Null(embed.Footer);
    }

    [Fact]
    public void Recipe_with_tags_should_return_embed_with_tags()
    {
        // Setup
        var fixture = new Fixture();
        var category = fixture.Create<RecipeCategory>();
        RecipeModel recipeModel = domainModelBuilder.SetCategory(category)
                                                    .AddTags(new[]
                                                    {
                                                        "Tag1",
                                                        "Tag2"
                                                    })
                                                    .Build();

        // Call
        Embed embed = RecipeEmbedFactory.Create(recipeModel);

        // Assert
        Assert.Equal(recipeModel.Title, embed.Title);

        EmbedImage? embedImage = embed.Image;
        Assert.Null(embedImage);

        AssertAuthor(recipeModel.Author, embed.Author);
        AssertFields(recipeModel.RecipeFields, embed.Fields);

        EmbedFooter? embedFooter = embed.Footer;
        Assert.NotNull(embedFooter);
        var expectedFooterText = $"{TagTestHelper.CategoryMapping[category]}, Tag1, Tag2";
        Assert.Equal(expectedFooterText, embedFooter.Value.Text);
    }

    public static IEnumerable<object[]> GetRecipeCategoriesAndColor()
    {
        yield return new object[]
        {
            RecipeCategory.Meat,
            new Color(250, 85, 87)
        };

        yield return new object[]
        {
            RecipeCategory.Fish,
            new Color(86, 153, 220)
        };

        yield return new object[]
        {
            RecipeCategory.Vegetarian,
            new Color(206, 221, 85)
        };
        yield return new object[]
        {
            RecipeCategory.Vegan,
            new Color(6, 167, 125)
        };
        yield return new object[]
        {
            RecipeCategory.Drinks,
            new Color(175, 234, 224)
        };
        yield return new object[]
        {
            RecipeCategory.Pastry,
            new Color(206, 132, 173)
        };
        yield return new object[]
        {
            RecipeCategory.Dessert,
            new Color(176, 69, 162)
        };
        yield return new object[]
        {
            RecipeCategory.Snack,
            new Color(249, 162, 114)
        };
        yield return new object[]
        {
            RecipeCategory.Other,
            new Color(165, 161, 164)
        };
    }

    private static void AssertAuthor(AuthorModel authorData, EmbedAuthor? actualAuthor)
    {
        Assert.NotNull(actualAuthor);
        Assert.Equal(authorData.AuthorName, actualAuthor.Value.Name);
        Assert.Equal(authorData.AuthorImageUrl, actualAuthor.Value.IconUrl);
    }

    private static void AssertFields(IEnumerable<RecipeFieldModel> recipeFields, IEnumerable<EmbedField> embedFields)
    {
        int nrOfRecipeFields = recipeFields.Count();
        Assert.Equal(nrOfRecipeFields, embedFields.Count());
        for (var i = 0; i < nrOfRecipeFields; i++)
        {
            AssertField(recipeFields.ElementAt(i), embedFields.ElementAt(i));
        }
    }

    private static void AssertField(RecipeFieldModel model, EmbedField actualField)
    {
        Assert.Equal(model.FieldName, actualField.Name);
        Assert.Equal(model.FieldData, actualField.Value);
        Assert.False(actualField.Inline);
    }
}