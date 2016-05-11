﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Json.Schema.ToDotNet.UnitTests;
using Xunit;
using Xunit.Abstractions;
using Assert = Microsoft.Json.Schema.ToDotNet.UnitTests.Assert;

namespace Microsoft.Json.Schema.ToDotNet.Hints.UnitTests
{
    public class DictionaryHintTests : CodeGenerationTestBase
    {
        public class TestCase
        {
            public TestCase(
                string name,
                string schemaText,
                string hintsText,
                string expectedClassText,
                string expectedComparerText,
                bool definesAdditionalClass)
            {
                Name = name;
                SchemaText = schemaText;
                HintsText = hintsText;
                ExpectedClassText = expectedClassText;
                ExpectedComparerText = expectedComparerText;
                DefinesAdditionalClass = definesAdditionalClass;
            }

            public TestCase()
            {
                // Needed for deserialization.
            }

            public string Name;
            public string SchemaText;
            public string HintsText;
            public string ExpectedClassText;
            public string ExpectedComparerText;
            public bool DefinesAdditionalClass;

            public void Deserialize(IXunitSerializationInfo info)
            {
                Name = info.GetValue<string>(nameof(Name));
                SchemaText = info.GetValue<string>(nameof(SchemaText));
                HintsText = info.GetValue<string>(nameof(HintsText));
                ExpectedClassText = info.GetValue<string>(nameof(ExpectedClassText));
                ExpectedComparerText = info.GetValue<string>(nameof(ExpectedComparerText));
                DefinesAdditionalClass = info.GetValue<bool>(nameof(DefinesAdditionalClass));
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(Name), Name);
                info.AddValue(nameof(SchemaText), SchemaText);
                info.AddValue(nameof(HintsText), HintsText);
                info.AddValue(nameof(ExpectedClassText), ExpectedClassText);
                info.AddValue(nameof(ExpectedComparerText), ExpectedComparerText);
                info.AddValue(nameof(DefinesAdditionalClass), DefinesAdditionalClass);
            }

            public override string ToString()
            {
                return Name;
            }
        }
        public static readonly TheoryData<TestCase> TestCases = new TheoryData<TestCase>
        {
            new TestCase(
                "Dictionary<string, string>",
@"{
  ""type"": ""object"",
  ""properties"": {
    ""dictProp"": {
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.DictProp"": [
    {
      ""kind"": ""DictionaryHint""
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    public partial class C
    {
        public static IEqualityComparer<C> ValueComparer => CEqualityComparer.Instance;

        public bool ValueEquals(C other) => ValueComparer.Equals(this, other);
        public int ValueGetHashCode() => ValueComparer.GetHashCode(this);

        [DataMember(Name = ""dictProp"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, string> DictProp { get; set; }
    }
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace N
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type C for equality.
    /// </summary>
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    internal sealed class CEqualityComparer : IEqualityComparer<C>
    {
        internal static readonly CEqualityComparer Instance = new CEqualityComparer();

        public bool Equals(C left, C right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            if (!object.ReferenceEquals(left.DictProp, right.DictProp))
            {
                if (left.DictProp == null || right.DictProp == null || left.DictProp.Count != right.DictProp.Count)
                {
                    return false;
                }

                foreach (var value_0 in left.DictProp)
                {
                    string value_1;
                    if (!right.DictProp.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (value_0.Value != value_1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetHashCode(C obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            int result = 17;
            unchecked
            {
                if (obj.DictProp != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_2 in obj.DictProp)
                    {
                        xor_0 ^= value_2.Key.GetHashCode();
                        if (value_2.Value != null)
                        {
                            xor_0 ^= value_2.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }
    }
}",
            definesAdditionalClass: false),

            new TestCase(
                "Dictionary<string, D>",
@"{
  ""type"": ""object"",
  ""properties"": {
    ""dictProp"": {
      ""type"": ""object"",
      ""additionalProperties"": {
        ""$ref"": ""#/definitions/d""
      }
    }
  },
  ""definitions"": {
    ""d"": {
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.DictProp"": [
    {
      ""kind"": ""DictionaryHint""
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    public partial class C
    {
        public static IEqualityComparer<C> ValueComparer => CEqualityComparer.Instance;

        public bool ValueEquals(C other) => ValueComparer.Equals(this, other);
        public int ValueGetHashCode() => ValueComparer.GetHashCode(this);

        [DataMember(Name = ""dictProp"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, D> DictProp { get; set; }
    }
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace N
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type C for equality.
    /// </summary>
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    internal sealed class CEqualityComparer : IEqualityComparer<C>
    {
        internal static readonly CEqualityComparer Instance = new CEqualityComparer();

        public bool Equals(C left, C right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            if (!object.ReferenceEquals(left.DictProp, right.DictProp))
            {
                if (left.DictProp == null || right.DictProp == null || left.DictProp.Count != right.DictProp.Count)
                {
                    return false;
                }

                foreach (var value_0 in left.DictProp)
                {
                    D value_1;
                    if (!right.DictProp.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (!D.ValueComparer.Equals(value_0.Value, value_1))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetHashCode(C obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            int result = 17;
            unchecked
            {
                if (obj.DictProp != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_2 in obj.DictProp)
                    {
                        xor_0 ^= value_2.Key.GetHashCode();
                        if (value_2.Value != null)
                        {
                            xor_0 ^= value_2.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }
    }
}",
            definesAdditionalClass: true),

            new TestCase(
                "Dictionary<string, IList<D>>",
@"{
  ""type"": ""object"",
  ""properties"": {
    ""dictProp"": {
      ""type"": ""object"",
      ""additionalProperties"": {
        ""type"": ""array"",
        ""items"": {
          ""$ref"": ""#/definitions/d""
        }
      }
    }
  },
  ""definitions"": {
    ""d"": {
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.DictProp"": [
    {
      ""kind"": ""DictionaryHint""
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    public partial class C
    {
        public static IEqualityComparer<C> ValueComparer => CEqualityComparer.Instance;

        public bool ValueEquals(C other) => ValueComparer.Equals(this, other);
        public int ValueGetHashCode() => ValueComparer.GetHashCode(this);

        [DataMember(Name = ""dictProp"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, IList<D>> DictProp { get; set; }
    }
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace N
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type C for equality.
    /// </summary>
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    internal sealed class CEqualityComparer : IEqualityComparer<C>
    {
        internal static readonly CEqualityComparer Instance = new CEqualityComparer();

        public bool Equals(C left, C right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            if (!object.ReferenceEquals(left.DictProp, right.DictProp))
            {
                if (left.DictProp == null || right.DictProp == null || left.DictProp.Count != right.DictProp.Count)
                {
                    return false;
                }

                foreach (var value_0 in left.DictProp)
                {
                    IList<D> value_1;
                    if (!right.DictProp.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (!object.ReferenceEquals(value_0.Value, value_1))
                    {
                        if (value_0.Value == null || value_1 == null)
                        {
                            return false;
                        }

                        if (value_0.Value.Count != value_1.Count)
                        {
                            return false;
                        }

                        for (int index_0 = 0; index_0 < value_0.Value.Count; ++index_0)
                        {
                            if (!D.ValueComparer.Equals(value_0.Value[index_0], value_1[index_0]))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public int GetHashCode(C obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            int result = 17;
            unchecked
            {
                if (obj.DictProp != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_2 in obj.DictProp)
                    {
                        xor_0 ^= value_2.Key.GetHashCode();
                        if (value_2.Value != null)
                        {
                            xor_0 ^= value_2.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }
    }
}",
            definesAdditionalClass: true),

            new TestCase(
                "Dictionary<HintedKeyType, IList<D>>",
@"{
  ""type"": ""object"",
  ""properties"": {
    ""dictProp"": {
      ""type"": ""object"",
      ""additionalProperties"": {
        ""type"": ""array"",
        ""items"": {
          ""$ref"": ""#/definitions/d""
        }
      }
    }
  },
  ""definitions"": {
    ""d"": {
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.DictProp"": [
    {
      ""kind"": ""DictionaryHint"",
      ""arguments"": {
        ""keyTypeName"": ""Uri""
      }
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    public partial class C
    {
        public static IEqualityComparer<C> ValueComparer => CEqualityComparer.Instance;

        public bool ValueEquals(C other) => ValueComparer.Equals(this, other);
        public int ValueGetHashCode() => ValueComparer.GetHashCode(this);

        [DataMember(Name = ""dictProp"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<Uri, IList<D>> DictProp { get; set; }
    }
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace N
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type C for equality.
    /// </summary>
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    internal sealed class CEqualityComparer : IEqualityComparer<C>
    {
        internal static readonly CEqualityComparer Instance = new CEqualityComparer();

        public bool Equals(C left, C right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            if (!object.ReferenceEquals(left.DictProp, right.DictProp))
            {
                if (left.DictProp == null || right.DictProp == null || left.DictProp.Count != right.DictProp.Count)
                {
                    return false;
                }

                foreach (var value_0 in left.DictProp)
                {
                    IList<D> value_1;
                    if (!right.DictProp.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (!object.ReferenceEquals(value_0.Value, value_1))
                    {
                        if (value_0.Value == null || value_1 == null)
                        {
                            return false;
                        }

                        if (value_0.Value.Count != value_1.Count)
                        {
                            return false;
                        }

                        for (int index_0 = 0; index_0 < value_0.Value.Count; ++index_0)
                        {
                            if (!D.ValueComparer.Equals(value_0.Value[index_0], value_1[index_0]))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public int GetHashCode(C obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            int result = 17;
            unchecked
            {
                if (obj.DictProp != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_2 in obj.DictProp)
                    {
                        xor_0 ^= value_2.Key.GetHashCode();
                        if (value_2.Value != null)
                        {
                            xor_0 ^= value_2.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }
    }
}",
            definesAdditionalClass: true),

            new TestCase(
                "Dictionary<string, IList<IList<D>>>",
@"{
  ""type"": ""object"",
  ""properties"": {
    ""dictProp"": {
      ""type"": ""object"",
      ""additionalProperties"": {
        ""type"": ""array"",
        ""items"": {
          ""type"": ""array"",
          ""items"": {
            ""$ref"": ""#/definitions/d""
          }
        }
      }
    }
  },
  ""definitions"": {
    ""d"": {
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.DictProp"": [
    {
      ""kind"": ""DictionaryHint""
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    public partial class C
    {
        public static IEqualityComparer<C> ValueComparer => CEqualityComparer.Instance;

        public bool ValueEquals(C other) => ValueComparer.Equals(this, other);
        public int ValueGetHashCode() => ValueComparer.GetHashCode(this);

        [DataMember(Name = ""dictProp"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, IList<IList<D>>> DictProp { get; set; }
    }
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace N
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type C for equality.
    /// </summary>
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    internal sealed class CEqualityComparer : IEqualityComparer<C>
    {
        internal static readonly CEqualityComparer Instance = new CEqualityComparer();

        public bool Equals(C left, C right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            if (!object.ReferenceEquals(left.DictProp, right.DictProp))
            {
                if (left.DictProp == null || right.DictProp == null || left.DictProp.Count != right.DictProp.Count)
                {
                    return false;
                }

                foreach (var value_0 in left.DictProp)
                {
                    IList<IList<D>> value_1;
                    if (!right.DictProp.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (!object.ReferenceEquals(value_0.Value, value_1))
                    {
                        if (value_0.Value == null || value_1 == null)
                        {
                            return false;
                        }

                        if (value_0.Value.Count != value_1.Count)
                        {
                            return false;
                        }

                        for (int index_0 = 0; index_0 < value_0.Value.Count; ++index_0)
                        {
                            if (!object.ReferenceEquals(value_0.Value[index_0], value_1[index_0]))
                            {
                                if (value_0.Value[index_0] == null || value_1[index_0] == null)
                                {
                                    return false;
                                }

                                if (value_0.Value[index_0].Count != value_1[index_0].Count)
                                {
                                    return false;
                                }

                                for (int index_1 = 0; index_1 < value_0.Value[index_0].Count; ++index_1)
                                {
                                    if (!D.ValueComparer.Equals(value_0.Value[index_0][index_1], value_1[index_0][index_1]))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public int GetHashCode(C obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            int result = 17;
            unchecked
            {
                if (obj.DictProp != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_2 in obj.DictProp)
                    {
                        xor_0 ^= value_2.Key.GetHashCode();
                        if (value_2.Value != null)
                        {
                            xor_0 ^= value_2.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }
    }
}",
            definesAdditionalClass: true),

            new TestCase(
                "Dictionary<HintedKeyType, HintedValueType>",
@"{
  ""type"": ""object"",
  ""properties"": {
    ""dictProp"": {
      ""type"": ""object"",
      ""additionalProperties"": {
        ""$ref"": ""#/definitions/d""
      }
    }
  },
  ""definitions"": {
    ""d"": {
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.DictProp"": [
    {
      ""kind"": ""DictionaryHint"",
      ""arguments"": {
        ""keyTypeName"": ""A"",
        ""valueTypeName"": ""B"",
        ""comparisonKind"": ""EqualityComparerEquals"",
        ""hashKind"": ""ScalarValueType"",
        ""initializationKind"": ""SimpleAssign""
      }
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    public partial class C
    {
        public static IEqualityComparer<C> ValueComparer => CEqualityComparer.Instance;

        public bool ValueEquals(C other) => ValueComparer.Equals(this, other);
        public int ValueGetHashCode() => ValueComparer.GetHashCode(this);

        [DataMember(Name = ""dictProp"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<A, B> DictProp { get; set; }
    }
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace N
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type C for equality.
    /// </summary>
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    internal sealed class CEqualityComparer : IEqualityComparer<C>
    {
        internal static readonly CEqualityComparer Instance = new CEqualityComparer();

        public bool Equals(C left, C right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            if (!object.ReferenceEquals(left.DictProp, right.DictProp))
            {
                if (left.DictProp == null || right.DictProp == null || left.DictProp.Count != right.DictProp.Count)
                {
                    return false;
                }

                foreach (var value_0 in left.DictProp)
                {
                    B value_1;
                    if (!right.DictProp.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (!B.ValueComparer.Equals(value_0.Value, value_1))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetHashCode(C obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            int result = 17;
            unchecked
            {
                if (obj.DictProp != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_2 in obj.DictProp)
                    {
                        xor_0 ^= value_2.Key.GetHashCode();
                        if (value_2.Value != null)
                        {
                            xor_0 ^= value_2.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }
    }
}",
            definesAdditionalClass: true),
        };

        [Theory(DisplayName = nameof(DictionaryHint))]
        [MemberData(nameof(TestCases))]
        public void DictionaryHint(TestCase testCase)
        {
            Settings.GenerateEqualityComparers = true;
            Settings.HintDictionary = new HintDictionary(testCase.HintsText);
            var generator = new DataModelGenerator(Settings, TestFileSystem.FileSystem);

            JsonSchema schema = SchemaReader.ReadSchema(testCase.SchemaText);

            generator.Generate(schema);

            var expectedContentsDictionary = new Dictionary<string, ExpectedContents>
            {
                [Settings.RootClassName] = new ExpectedContents
                {
                    ClassContents = testCase.ExpectedClassText,
                    ComparerClassContents = testCase.ExpectedComparerText
                }
            };

            // We won't bother to compare the contents of the original class (which
            // has no interesting properties), but the assertion method below needs to
            // know how many classes were generated.
            if (testCase.DefinesAdditionalClass)
            {
                expectedContentsDictionary.Add("D", new ExpectedContents());
            }

            Assert.FileContentsMatchExpectedContents(TestFileSystem, expectedContentsDictionary);
        }
    }
}