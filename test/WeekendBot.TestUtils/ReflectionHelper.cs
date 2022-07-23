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
using System.Reflection;

namespace WeekendBot.TestUtils
{
    /// <summary>
    /// Helper class containing methods related to <see cref="System.Reflection"/>.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets a custom attribute from a method without arguments.
        /// </summary>
        /// <typeparam name="TObject">The type of object to retrieve the attributes from.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <param name="methodName">The name of the method of <typeparamref name="TObject"/> to
        ///  retrieve the attribute for.</param>
        /// <returns>The <typeparamref name="TAttribute"/>, <c>null</c> if the attribute was not found.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodName"/> is <c>null</c>, empty
        /// or consists of whitespace.</exception>
        public static TAttribute GetCustomAttribute<TObject, TAttribute>(string methodName)
            where TObject : class
            where TAttribute : Attribute
        {
            return GetCustomAttribute<TObject, TAttribute>(methodName, Type.EmptyTypes);
        }

        /// <summary>
        /// Gets a custom attribute from a method with arguments.
        /// </summary>
        /// <typeparam name="TObject">The type of object to retrieve the attributes from.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <param name="methodName">The name of the method of <typeparamref name="TObject"/> to
        ///  retrieve the attribute for.</param>
        /// <param name="argumentTypes">The array of argument types that are passed in the method.</param>
        /// <returns>The <typeparamref name="TAttribute"/>, <c>null</c> if the attribute was not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="argumentTypes"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodName"/> is <c>null</c>, empty
        /// or consists of whitespace.</exception>
        public static TAttribute GetCustomAttribute<TObject, TAttribute>(string methodName, Type[] argumentTypes)
            where TObject : class
            where TAttribute : Attribute
        {
            if (argumentTypes == null)
            {
                throw new ArgumentNullException(nameof(argumentTypes));
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(methodName));
            }

            return typeof(TObject).GetMethod(methodName, argumentTypes)?.GetCustomAttribute<TAttribute>();
        }
    }
}