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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using RecipeBot.Domain.Data;
using RecipeBot.Domain.Models;
using RecipeBot.Domain.Repositories;
using RecipeBot.Domain.TestUtils;
using RecipeBot.Persistence.Entities;
using Xunit;

namespace RecipeBot.Persistence.Test;

public class RecipeRepositoryTest : IDisposable
{
    private const int characterLimit = 1000;
    private static readonly string? assemblyDirectory = Path.GetDirectoryName(typeof(RecipeRepositoryTest).GetTypeInfo().Assembly.Location);
    private static readonly string testDirectory = Path.Combine(assemblyDirectory!, "test-data");
    private readonly RecipeDomainModelTestBuilder testBuilder;

    public RecipeRepositoryTest()
    {
        testBuilder = new RecipeDomainModelTestBuilder(new RecipeDomainModelTestBuilder.ConstructionProperties
        {
            MaxAuthorNameLength = characterLimit,
            MaxTitleLength = characterLimit,
            MaxFieldNameLength = characterLimit,
            MaxFieldDataLength = characterLimit
        });

        if (!Directory.Exists(testDirectory))
        {
            Directory.CreateDirectory(testDirectory);
        }
    }

    [Fact]
    public void Repository_is_recipe_repository()
    {
        // Setup
        var provider = RecipeBotDbContextTestProvider.CreateInMemoryProvider();
        using (RecipeBotDbContext context = provider.CreateContext())
        {
            // Call
            var repository = new RecipeRepository(context);

            // Assert
            Assert.IsAssignableFrom<IRecipeRepository>(repository);
        }
    }

    [Fact]
    public async Task Given_empty_database_when_saving_basic_recipe_saves_expected_data()
    {
        // Setup
        const string fileName = "BasicRecipe-Save.sqlite";

        var fixture = new Fixture();
        var category = fixture.Create<RecipeCategory>();
        RecipeModel recipeModel = testBuilder.SetCategory(category)
                                             .Build();

        Action<RecipeBotDbContext> assertPersistedDataAction =
            context =>
            {
                RecipeEntity? recipeEntity = context.RecipeEntities
                                                    .Include(e => e.Author)
                                                    .Include(e => e.RecipeFields)
                                                    .Include(e => e.Tags)
                                                    .Single();

                Assert.Equal(recipeModel.Title, recipeEntity.RecipeTitle);
                Assert.Equal((PersistentRecipeCategory)category, recipeEntity.RecipeCategory);

                AuthorModel expectedAuthor = recipeModel.Author;
                AuthorEntity authorEntity = recipeEntity.Author;
                Assert.NotNull(authorEntity);
                Assert.Equal(expectedAuthor.AuthorName, authorEntity.AuthorName);
                Assert.Equal(expectedAuthor.AuthorImageUrl, authorEntity.AuthorImageUrl);

                Assert.Empty(recipeEntity.RecipeFields);
                Assert.Empty(recipeEntity.Tags);
            };

        // Call & Assert
        var test = new RecipeRepositoryPersistDataTest(fileName, recipeModel, assertPersistedDataAction);
        await test.ExecuteTest();
    }

    [Fact]
    public async Task Given_empty_database_when_saving_recipe_with_all_data_saves_expected_data()
    {
        // Setup
        const string fileName = "RecipeAllData-Save.sqlite";

        var fixture = new Fixture();
        var category = fixture.Create<RecipeCategory>();

        string[] tags =
        {
            fixture.Create<string>(),
            fixture.Create<string>()
        };

        const int nrOfFields = 3;
        RecipeModel recipeModel = testBuilder.SetCategory(category)
                                             .AddFields(nrOfFields)
                                             .AddTags(tags)
                                             .Build();

        Action<RecipeBotDbContext> assertPersistedDataAction =
            context =>
            {
                RecipeEntity recipeEntity = context.RecipeEntities
                                                   .Include(e => e.Author)
                                                   .Include(e => e.RecipeFields)
                                                   .Include(e => e.Tags)
                                                   .ThenInclude(e => e.Tag)
                                                   .Single();

                Assert.Equal(recipeModel.Title, recipeEntity.RecipeTitle);
                Assert.Equal((PersistentRecipeCategory)category, recipeEntity.RecipeCategory);

                AuthorModel expectedAuthor = recipeModel.Author;
                AuthorEntity authorEntity = recipeEntity.Author;
                Assert.NotNull(authorEntity);
                Assert.Equal(expectedAuthor.AuthorName, authorEntity.AuthorName);
                Assert.Equal(expectedAuthor.AuthorImageUrl, authorEntity.AuthorImageUrl);

                ICollection<RecipeFieldEntity> recipeFieldEntities = recipeEntity.RecipeFields;
                Assert.Equal(nrOfFields, recipeFieldEntities.Count);
                for (var i = 0; i < nrOfFields; i++)
                {
                    RecipeFieldModel fieldModel = recipeModel.RecipeFields.ElementAt(i);
                    RecipeFieldEntity fieldEntity = recipeFieldEntities.ElementAt(i);

                    Assert.Equal(fieldModel.FieldName, fieldEntity.RecipeFieldName);
                    Assert.Equal(fieldModel.FieldData, fieldEntity.RecipeFieldData);
                    Assert.Equal(i, fieldEntity.Order);
                }

                int nrOfTags = tags.Length;
                Assert.Equal(nrOfTags, recipeEntity.Tags.Count);
                for (var i = 0; i < nrOfTags; i++)
                {
                    string expectedTag = tags[i];
                    RecipeTagEntity tagEntity = recipeEntity.Tags.ElementAt(i);

                    Assert.Equal(expectedTag, tagEntity.Tag.Tag);
                    Assert.Equal(i, tagEntity.Order);
                }
            };

        // Call & Assert
        var test = new RecipeRepositoryPersistDataTest(fileName, recipeModel, assertPersistedDataAction);
        await test.ExecuteTest();
    }

    [Fact]
    public async Task Given_database_with_author_data_when_saving_recipe_with_author_then_recipe_links_to_author()
    {
        // Setup
        const string fileName = "BasicRecipeAuthor-Save.sqlite";

        RecipeModel recipeModel = testBuilder.Build();

        Action<RecipeBotDbContext> seedDatabaseAction =
            context =>
            {
                context.AuthorEntities.Add(new AuthorEntity
                {
                    AuthorName = recipeModel.Author.AuthorName,
                    AuthorImageUrl = recipeModel.Author.AuthorImageUrl
                });
            };

        Action<RecipeBotDbContext> assertPersistedDataAction =
            context =>
            {
                RecipeEntity recipeEntity = context.RecipeEntities
                                                   .Include(e => e.Author)
                                                   .Single();

                AuthorModel expectedAuthor = recipeModel.Author;
                AuthorEntity authorEntity = recipeEntity.Author;
                Assert.NotNull(authorEntity);
                Assert.Equal(expectedAuthor.AuthorName, authorEntity.AuthorName);
                Assert.Equal(expectedAuthor.AuthorImageUrl, authorEntity.AuthorImageUrl);

                Assert.Same(authorEntity, recipeEntity.Author);
            };

        // Call & Assert
        var test = new RecipeRepositoryPersistDataTest(fileName, recipeModel, assertPersistedDataAction, seedDatabaseAction);
        await test.ExecuteTest();
    }

    [Fact]
    public async Task Given_database_with_tag_data_when_saving_recipe_with_tag_then_recipe_links_to_tag()
    {
        // Setup
        const string fileName = "BasicRecipeTags-Save.sqlite";

        var fixture = new Fixture();
        string[] tags =
        {
            fixture.Create<string>(),
            fixture.Create<string>()
        };

        RecipeModel recipeModel = testBuilder.AddTags(tags)
                                             .Build();

        Action<RecipeBotDbContext> seedDatabaseAction =
            context =>
            {
                context.TagEntities.AddRange(tags.Reverse().Select(t => new TagEntity
                {
                    Tag = t
                }));
            };

        Action<RecipeBotDbContext> assertPersistedDataAction =
            context =>
            {
                RecipeEntity recipeEntity = context.RecipeEntities
                                                   .Include(e => e.Tags)
                                                   .ThenInclude(e => e.Tag)
                                                   .Single();

                int nrOfTags = tags.Length;
                IReadOnlyCollection<RecipeTagEntity> orderedTags = recipeEntity.Tags
                                                                               .OrderBy(e => e.Order)
                                                                               .ToArray();
                Assert.Equal(nrOfTags, orderedTags.Count);
                for (var i = 0; i < nrOfTags; i++)
                {
                    string expectedTag = tags[i];
                    RecipeTagEntity tagEntity = orderedTags.ElementAt(i);

                    Assert.Equal(expectedTag, tagEntity.Tag.Tag);
                    Assert.Equal(i, tagEntity.Order);
                }
            };

        // Call & Assert
        var test = new RecipeRepositoryPersistDataTest(fileName, recipeModel, assertPersistedDataAction, seedDatabaseAction);
        await test.ExecuteTest();
    }

    public void Dispose()
    {
        if (Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, true);
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Provider of <see cref="RecipeBotDbContext"/> that can be used for unit testing.
    /// </summary>
    private class RecipeBotDbContextTestProvider
    {
        private readonly DbContextOptions<RecipeBotDbContext> contextOptions;
        private readonly object contextLock = new object();

        private RecipeBotDbContextTestProvider(string connectionString)
        {
            contextOptions = new DbContextOptionsBuilder<RecipeBotDbContext>().UseSqlite(connectionString)
                                                                              .Options;
            CreateDatabaseFile();
        }

        /// <summary>
        /// Creates an instance of <see cref="RecipeBotDbContextTestProvider"/> to return the <see cref="RecipeBotDbContext"/>
        /// with an in memory configuration.
        /// </summary>
        /// <returns>A <see cref="RecipeBotDbContextTestProvider"/>.</returns>
        public static RecipeBotDbContextTestProvider CreateInMemoryProvider()
        {
            return new RecipeBotDbContextTestProvider("Filename=:memory:");
        }

        /// <summary>
        /// Creates an instance of <see cref="RecipeBotDbContextTestProvider"/> to return the <see cref="RecipeBotDbContext"/>
        /// with a connection to the <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The path of the database file to connect to.</param>
        /// <returns>A <see cref="RecipeBotDbContextTestProvider"/>.</returns>
        /// <remarks>If the <paramref name="filePath"/> refers to an existing database file, the file will be replaced
        /// without data preservation.</remarks>
        public static RecipeBotDbContextTestProvider CreateProvider(string filePath)
        {
            return new RecipeBotDbContextTestProvider($"Filename={filePath}");
        }

        public RecipeBotDbContext CreateContext()
        {
            return new RecipeBotDbContext(contextOptions);
        }

        private void CreateDatabaseFile()
        {
            lock (contextLock)
            {
                using (RecipeBotDbContext context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
            }
        }
    }

    private class RecipeRepositoryPersistDataTest
    {
        private readonly string fileName;
        private readonly RecipeModel recipe;
        private readonly Action<RecipeBotDbContext>? seedDatabaseAction;
        private readonly Action<RecipeBotDbContext> assertPersistedDataAction;

        public RecipeRepositoryPersistDataTest(string fileName,
                                               RecipeModel recipe,
                                               Action<RecipeBotDbContext> assertPersistedDataAction,
                                               Action<RecipeBotDbContext>? seedDatabaseAction = null)
        {
            this.fileName = fileName;
            this.recipe = recipe;
            this.seedDatabaseAction = seedDatabaseAction;
            this.assertPersistedDataAction = assertPersistedDataAction;
        }

        public async Task ExecuteTest()
        {
            var contextProvider = RecipeBotDbContextTestProvider.CreateProvider(Path.Combine(testDirectory, fileName));
            if (seedDatabaseAction != null)
            {
                using (RecipeBotDbContext context = contextProvider.CreateContext())
                {
                    seedDatabaseAction?.Invoke(context);
                    await context.SaveChangesAsync();
                }
            }

            using (RecipeBotDbContext context = contextProvider.CreateContext())
            {
                var repository = new RecipeRepository(context);
                await repository.SaveRecipeAsync(recipe);
            }

            using (RecipeBotDbContext context = contextProvider.CreateContext())
            {
                assertPersistedDataAction(context);

                await context.Database.EnsureDeletedAsync();
            }
        }
    }
}