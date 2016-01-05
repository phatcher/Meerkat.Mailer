using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

using Common.Logging;

namespace Meerkat.Mailer
{
    public static class XmlSerializerExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default <see cref="XmlWriterSettings"/> that give a nicely formatted file without header
        /// </summary>
        /// <param name="conformance"></param>
        /// <returns></returns>
        public static XmlWriterSettings DefaultXmlWriterSettings(ConformanceLevel conformance = ConformanceLevel.Auto)
        {
            var xws = new XmlWriterSettings
            {
                ConformanceLevel = conformance,
                OmitXmlDeclaration = true,
                Indent = true
            };

            return xws;
        }

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="fileName">Name of file, can include root relative paths if in a web application.</param>
        /// <returns>Instance of T.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Polymorphic method")]
        public static T LoadXmlDocument<T>(this string fileName)
        {
            return LoadXmlDocument<T>(() => new StreamReader(fileName.MapPath()));
        }

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="stream">Stream to load from.</param>
        /// <returns>Instance of T.</returns>
        public static T LoadXmlDocument<T>(this Stream stream)
        {
            return LoadXmlDocument<T>(() => new StreamReader(stream));
        }

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="func">Function to provide a <see cref="StreamReader" /></param>
        /// <returns>Instance of T.</returns>
        public static T LoadXmlDocument<T>(Func<TextReader> func)
        {
            var serializer = new XmlSerializer(typeof(T));
            TextReader reader = null;

            try
            {
                reader = func();
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Serialize an entity to XML.
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="entity">Entity to use</param>
        /// <param name="fileName">File to create</param>
        /// <param name="types">Subtypes required for serialization</param>
        /// <param name="settings">Parameters to be used for the XML</param>
        public static void XmlSerialize<T>(this T entity, string fileName, Type[] types = null, XmlWriterSettings settings = null)
        {
            using (var stream = new StreamWriter(fileName))
            {
                stream.TextWriterXmlSerialize(entity, types, settings);
            }
        }

        /// <summary>
        /// Serialize an entity to XML
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="entity">Entity to use</param>
        /// <param name="types">Subtypes required for serialization</param>
        /// <param name="settings">Parameters to be used for the XML</param>        
        /// <returns></returns>
        public static string XmlSerialize<T>(this T entity, Type[] types = null, XmlWriterSettings settings = null)
        {
            using (var stream = new StringWriter())
            {
                stream.TextWriterXmlSerialize(entity, types, settings);
                return stream.ToString();
            }
        }

        /// <summary>
        /// XML serialize an entity to a TextWriter.
        /// </summary>
        /// <param name="textWriter"></param>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="entity">Entity to use</param>
        /// <param name="types">Subtypes required for serialization</param>
        /// <param name="settings">Parameters to be used for the XML</param>    
        public static void TextWriterXmlSerialize<T>(this TextWriter textWriter, T entity, Type[] types = null, XmlWriterSettings settings = null)
        {
            if (settings == null)
            {
                settings = DefaultXmlWriterSettings();
            }

            using (var writer = XmlWriter.Create(textWriter, settings))
            {
                var serializer = XmlSerializer<T>(types);

                serializer.Serialize(writer, entity);
                writer.Close();
            }
        }

        private static XmlSerializer XmlSerializer<T>(Type[] types = null)
        {
            return types == null || types.Length == 0 ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), types);
        }
    }
}
