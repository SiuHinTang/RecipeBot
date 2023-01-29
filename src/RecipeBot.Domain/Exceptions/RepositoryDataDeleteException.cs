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
using System.Runtime.Serialization;

namespace RecipeBot.Domain.Exceptions;

/// <summary>
/// Exception thrown when data could not be successfully deleted from the repository.
/// </summary>
[Serializable]
public class RepositoryDataDeleteException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryDataDeleteException"/> class.
    /// </summary>
    public RepositoryDataDeleteException() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryDataDeleteException"/> class 
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RepositoryDataDeleteException(string message) : base(message) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryDataDeleteException"/> class with a specified error message 
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception,
    /// or <c>null</c> if no inner exception is specified.</param>
    public RepositoryDataDeleteException(string message, Exception innerException) : base(message, innerException) {}

    /// <summary>
    /// Initializes a new instance of <see cref="RepositoryDataDeleteException"/> with
    /// serialized data.</summary>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized
    /// object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual
    /// information about the source or destination.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is
    /// <c>null</c>.</exception>
    /// <exception cref="SerializationException">The class name is <c>null</c> or
    /// <see cref="Exception.HResult" /> is zero (0).</exception>
    protected RepositoryDataDeleteException(SerializationInfo info, StreamingContext context) : base(info, context) {}
}