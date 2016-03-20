﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace Microsoft.Json.Schema.Generator
{
    /// <summary>
    /// Provides data for the AdditionalTypeRequired event.
    /// </summary>
    public class AdditionalTypeRequiredEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalTypeRequiredEventArgs"/> type;
        /// </summary>
        /// <param name="hint">
        /// Hint that guides generation of the additional type.
        /// </param>
        /// <param name="schema">
        /// Schema to which the hint applies.
        /// </param>
        public AdditionalTypeRequiredEventArgs(CodeGenHint hint, JsonSchema schema)
        {
            Hint = hint;
            Schema = schema;
        }

        /// <summary>
        /// Hint that guides generation of the additional type.
        /// </summary>
        public CodeGenHint Hint { get; }

        /// <summary>
        /// Schema to which the hint applies.
        /// </summary>
        public JsonSchema Schema { get; }
    }
}