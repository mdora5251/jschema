﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Json.Schema
{
    public static class SchemaReader
    {
        public static JsonSchema ReadSchema(TextReader reader, string filePath)
        {
            return ReadSchema(reader.ReadToEnd(), filePath);
        }

        public static JsonSchema ReadSchema(string jsonText, string filePath)
        {
            // Change "$ref" to "$$ref" before we ask Json.NET to deserialize it,
            // because Json.NET treats "$ref" specially.
            jsonText = RefProperty.ConvertFromInput(jsonText);

            var traceWriter = new SchemaValidationExceptionCapturingTraceWriter();

            var serializer = new JsonSerializer
            {
                ContractResolver = new JsonSchemaContractResolver(),
                TraceWriter = traceWriter
            };

            JsonSchema schema;
            using (var jsonReader = new JsonTextReader(new StringReader(jsonText)))
            {
                try
                {
                    schema = serializer.Deserialize<JsonSchema>(jsonReader);
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonSyntaxException(filePath, ex);
                }
            }


            if (traceWriter.SchemaValidationExceptions.Any())
            {
                throw new SchemaValidationException(traceWriter.SchemaValidationExceptions);
            }

            return schema;
        }
    }
}
