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

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RecipeBot.Persistence.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RecipeBot.Persistence.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No matching author found..
        /// </summary>
        internal static string AuthorRepository_Delete_AuthorEntity_No_matching_author_found {
            get {
                return ResourceManager.GetString("AuthorRepository_Delete_AuthorEntity_No_matching_author_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Recipe entries could not be loaded due to invalid AuthorId &apos;{0}&apos;..
        /// </summary>
        internal static string Recipe_entries_unsuccessfully_loaded_due_to_invalid_AuthorId_0 {
            get {
                return ResourceManager.GetString("Recipe_entries_unsuccessfully_loaded_due_to_invalid_AuthorId_0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Recipe with id &apos;{0}&apos; could not be deleted due to invalid AuthorId &apos;{1}&apos;..
        /// </summary>
        internal static string RecipeEntityId_0_unsuccessfully_deleted_due_to_invalid_AuthorId_1 {
            get {
                return ResourceManager.GetString("RecipeEntityId_0_unsuccessfully_deleted_due_to_invalid_AuthorId_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Recipe with id &apos;{0}&apos; could not be loaded due to invalid AuthorId &apos;{1}&apos;..
        /// </summary>
        internal static string RecipeEntityId_0_unsuccessfully_loaded_due_to_invalid_AuthorId_1 {
            get {
                return ResourceManager.GetString("RecipeEntityId_0_unsuccessfully_loaded_due_to_invalid_AuthorId_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Author has no recipe that matches with id &apos;{0}&apos;..
        /// </summary>
        internal static string RecipeRepository_Author_has_no_recipe_matches_with_EntityId_0_ {
            get {
                return ResourceManager.GetString("RecipeRepository_Author_has_no_recipe_matches_with_EntityId_0_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No recipe matches with id &apos;{0}&apos;..
        /// </summary>
        internal static string RecipeRepository_No_recipe_matches_with_EntityId_0 {
            get {
                return ResourceManager.GetString("RecipeRepository_No_recipe_matches_with_EntityId_0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No tag matches with id &apos;{0}&apos;..
        /// </summary>
        internal static string RecipeTagEntryRepository_DeleteTagAsync_No_tag_matches_with_EntityId_0_ {
            get {
                return ResourceManager.GetString("RecipeTagEntryRepository_DeleteTagAsync_No_tag_matches_with_EntityId_0_", resourceCulture);
            }
        }
    }
}
