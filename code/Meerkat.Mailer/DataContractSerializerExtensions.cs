using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

using Meerkat.Logging;

namespace Meerkat.Mailer
{
    public static class DataContractSerializerExtensions
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Serialize an entity to XML.
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="entity">Entity to use</param>
        /// <param name="fileName">File to create</param>
        public static void DataContractSerialize<T>(this T entity, string fileName)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = File.Open(fileName, FileMode.OpenOrCreate))
            {
                serializer.WriteObject(stream, entity);
            }
        }

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="fileName">Name of file, can include root relative paths if in a web application.</param>
        /// <returns>Instance of T.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Polymorphic method")]
        public static T LoadDataContract<T>(this string fileName)
        {
            return LoadDataContract<T>(new FileStream(fileName.MapPath(), FileMode.Open));
        }

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="stream">Stream to load from.</param>
        /// <returns>Instance of T.</returns>
        public static T LoadDataContract<T>(this Stream stream)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var rq = new XmlDictionaryReaderQuotas();
            var reader = XmlDictionaryReader.CreateTextReader(stream, rq);

            try
            {
                return (T)serializer.ReadObject(reader);
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
    }
}