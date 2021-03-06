﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Framework, 2017.1
//
// Copyright 2017 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using PDS.WITSMLstudio.Framework.Properties;

namespace PDS.WITSMLstudio.Framework
{
    /// <summary>
    /// Provides custom extension methods for .NET framework types.
    /// </summary>
    public static class FrameworkExtensions
    {
        private static readonly string DefaultEncryptionKey = Settings.Default.DefaultEncryptionKey;

        /// <summary>
        /// Gets the version for the <see cref="System.Reflection.Assembly"/> containing the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldCount">The field count.</param>
        /// <returns>The version number string.</returns>
        public static string GetAssemblyVersion(this Type type, int fieldCount = 4)
        {
            return type.Assembly.GetName().Version.ToString(fieldCount);
        }

        /// <summary>
        /// Throws an exception if the input parameter is null.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameter"/> is null.</exception>
        public static void NotNull(this object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Creates an array of trimmed strings by splitting this string at each occurence of a separator.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>A string array.</returns>
        public static string[] SplitAndTrim(this string value, string separator)
        {
            return string.IsNullOrWhiteSpace(value)
                ? new string[0]
                : value.Split(new[] { separator }, StringSplitOptions.None)
                       .Select(x => x.Trim())
                       .ToArray();
        }

        /// <summary>
        /// Determines whether the collection contains the specified value, ignoring case.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the collection contains the specified value; otherwise, false.</returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> source, string value)
        {
            return source.Any(x => x.EqualsIgnoreCase(value));
        }

        /// <summary>
        /// Determines whether two specified strings have the same value, ignoring case.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <returns>
        /// true if the value of a is the same as the value of b; otherwise, false.
        /// If both a and b are null, the method returns true.
        /// </returns>
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Converts the specified string to camel case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The camel case string value.</returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return value.Substring(0, 1).ToLowerInvariant() + value.Substring(1);
        }

        /// <summary>
        /// Converts the specified string to pascal case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The pascal case string value.</returns>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return value.Substring(0, 1).ToUpperInvariant() + value.Substring(1);
        }

        /// <summary>
        /// Performs the specified action on each item in the collection.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action to perform on each item in the collection.</param>
        /// <returns>The source collection, for chaining.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
            
            return collection;
        }

        /// <summary>
        /// Performs the specified action on each item in the collection.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action to perform on each item in the collection.</param>
        /// <returns>The source collection, for chaining.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            int index = 0;

            foreach (var item in collection)
                action(item, index++);

            return collection;
        }

        /// <summary>
        /// Gets the property value from the specified object instance.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns>The property value.</returns>
        public static object GetPropertyValue(this object instance, string propertyPath)
        {
            foreach (var propertyName in propertyPath.Split('.'))
            {
                if (instance == null) return null;

                var type = instance.GetType();
                var info = type.GetProperty(propertyName);

                if (info == null) return null;

                instance = info.GetValue(instance);
            }

            return instance;
        }

        /// <summary>
        /// Gets the property value from the specified object instance.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="instance">The object instance.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns>The property value.</returns>
        public static T GetPropertyValue<T>(this object instance, string propertyPath)
        {
            var propertyValue = instance.GetPropertyValue(propertyPath);
            if (propertyValue == null) return default(T);
            return (T)propertyValue;
        }

        /// <summary>
        /// Gets the description for the specified enumeration member.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <returns>
        /// The description from the <see cref="DescriptionAttribute"/> when available;
        /// otherwise, the value's ToString() representation.
        /// </returns>
        public static string GetDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();

            return attribute != null
                ? attribute.Description
                : value.ToString();
        }

        /// <summary>
        /// Gets the name for the specified enumeration member.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <returns>
        /// The name from the <see cref="XmlEnumAttribute"/> when available;
        /// otherwise, the value's ToString() representation.
        /// </returns>
        public static string GetName(this Enum value)
        {
            var attribute = value.GetAttribute<XmlEnumAttribute>();

            return attribute != null
                ? attribute.Name
                : value.ToString();
        }

        /// <summary>
        /// Gets the custom attribute defined for the specified enumeration member.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="value">The enumeration value.</param>
        /// <returns>The defined attribute, or null.</returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var fieldInfo = enumType.GetField(Enum.GetName(enumType, value));

            return fieldInfo?.GetCustomAttribute<TAttribute>(true);
        }

        /// <summary>
        /// Parses the enum.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <returns></returns>
        public static object ParseEnum(this Type enumType, string enumValue)
        {
            if (string.IsNullOrWhiteSpace(enumValue)) return null;

            if (Enum.IsDefined(enumType, enumValue))
            {
                return Enum.Parse(enumType, enumValue);
            }

            var enumMember = enumType.GetMembers().FirstOrDefault(x =>
            {
                if (x.Name.EqualsIgnoreCase(enumValue))
                    return true;

                var xmlEnumAttrib = x.GetCustomAttribute<XmlEnumAttribute>();
                return xmlEnumAttrib != null && xmlEnumAttrib.Name.EqualsIgnoreCase(enumValue);
            });

            // must be a valid enumeration member
            if (!enumType.IsEnum || enumMember == null)
            {
                throw new ArgumentException();
            }

            return Enum.Parse(enumType, enumMember.Name);
        }

        /// <summary>
        /// Determines whether the specified type is numeric.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true if the type is numeric; otherwise, false</returns>
        public static bool IsNumeric(this Type type)
        {
            if (type == null) return false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            var typeCode = Type.GetTypeCode(type);
            return typeCode >= TypeCode.SByte && typeCode <= TypeCode.Decimal;
        }

        /// <summary>
        /// Encrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The encrypted value.</returns>
        public static string Encrypt(this string value, string key = null)
        {
            if (value == null) return null;

            var bytes = Encoding.Unicode.GetBytes(value);
            var entropy = Encoding.Unicode.GetBytes(key ?? DefaultEncryptionKey);

            bytes = ProtectedData.Protect(bytes, entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The decrypted value.</returns>
        public static string Decrypt(this string value, string key = null)
        {
            if (value == null) return null;

            var bytes = Convert.FromBase64String(value);
            var entropy = Encoding.Unicode.GetBytes(key ?? DefaultEncryptionKey);

            bytes = ProtectedData.Unprotect(bytes, entropy, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// Creates a new <see cref="SecureString"/> from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="SecureString"/> instance.</returns>
        public static SecureString ToSecureString(this string value)
        {
            var secure = new SecureString();

            if (!string.IsNullOrWhiteSpace(value))
                value.ForEach(secure.AppendChar);

            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Gets the base exception of the specified type.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="ex">The exception.</param>
        /// <returns>An exception of the specified type, or null.</returns>
        public static T GetBaseException<T>(this Exception ex) where T : Exception
        {
            var typed = ex as T;
            if (typed != null) return typed;

            var inner = ex.InnerException;

            while (inner != null)
            {
                typed = inner as T;
                if (typed != null) return typed;

                inner = inner.InnerException;
            }

            return null;
        }

        /// <summary>
        /// Updates the name of the root element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="type">The type.</param>
        /// <returns>A new <see cref="XElement"/> instance.</returns>
        public static XElement UpdateRootElementName(this XElement element, Type type)
        {
            var xmlRoot = type.GetCustomAttribute<XmlRootAttribute>();
            var xmlType = type.GetCustomAttribute<XmlTypeAttribute>();
            var elementName = type.Name;

            if (!string.IsNullOrWhiteSpace(xmlRoot?.ElementName))
                elementName = xmlRoot.ElementName;

            else if (!string.IsNullOrWhiteSpace(xmlType?.TypeName))
                elementName = xmlType.TypeName;

            if (element.Name.LocalName.Equals(elementName))
                return element;

            var xElementName = !string.IsNullOrWhiteSpace(xmlRoot?.Namespace)
                ? XNamespace.Get(xmlRoot.Namespace).GetName(elementName)
                : elementName;

            // Update element name to match XSD type name
            var clone = new XElement(element)
            {
                Name = xElementName
            };

            // Remove xsi:type attribute used for abstract types
            var xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            clone.Attribute(xsi.GetName("type"))?.Remove();

            return clone;
        }

        /// <summary>
        /// Converts an <see cref="XElement"/> to an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>An <see cref="XmlElement"/> instance.</returns>
        public static XmlElement ToXmlElement(this XElement element)
        {
            using (var reader = element.CreateReader())
            {
                var doc = new XmlDocument();
                doc.Load(reader);
                return doc.DocumentElement;
            }
        }

        /// <summary>
        /// Evaluates the specified XPath expression.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>A collection of elements or attributes.</returns>
        public static IEnumerable<object> Evaluate(this XDocument document, string expression, IXmlNamespaceResolver resolver)
        {
            return ((IEnumerable) document.XPathEvaluate(expression, resolver)).Cast<object>();
        }
    }
}
