namespace SolutionModelsLib.Xml
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;

    /// <summary>
    /// Implements methods to serialize and deserialze XML from/to
    /// file via <see cref="DataContractSerializer"/>, wich requires
    /// a reference on the System.Runtime.Serialization assembly.
    /// </summary>
    public class Storage
    {
        ///<summary>
        /// Writes the associated XML of class Model T into string and returns it.
        ///</summary>
        ///<param name="rootModel"></param>
        public static string WriteXmlToString(ISolutionModel rootModel)
        {
            using (var stringWriter = new StringWriter())     // Write Xml to string
            {
                XmlWriter xmlWriter = null;
                try
                {
                    xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "  ",
                        CloseOutput = true
                    });

                    var dataContractSerializer = new DataContractSerializer(typeof(SolutionModel));
                    dataContractSerializer.WriteObject(xmlWriter, rootModel);
                }
                finally
                {
                    if (xmlWriter != null)
                        xmlWriter.Close();
                }

                return stringWriter.ToString();
            }
        }

        ///<summary>
        /// Writes the associated XML of class Model T into a file.
        ///</summary>
        ///<param name="rootModel"></param>
        ///<param name="filename"></param>
        public static void WriteXmlToFile(string filename, ISolutionModel rootModel)
        {
            XmlWriter xmlWriter = null;
            try
            {
                var fileStream = new FileStream(filename, FileMode.Create);
                xmlWriter = XmlWriter.Create(fileStream, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    CloseOutput = true
                });

                var dataContractSerializer = new DataContractSerializer(typeof(SolutionModel));
                dataContractSerializer.WriteObject(xmlWriter, rootModel);
            }
            finally
            {
                if (xmlWriter != null)
                    xmlWriter.Close();
            }
        }

        ///<summary>
        /// Writes the associated XML of class Model T into a file.
        ///</summary>
        ///<param name="filename"></param>
        public static ISolutionModel ReadXmlFromFile<T>(string filename)
        {
            XmlReader xmlReader = null;
            try
            {
                xmlReader = XmlReader.Create(filename, new XmlReaderSettings
                {
                    CloseInput = true
                });

                var dataContractSerializer = new DataContractSerializer(typeof(SolutionModel));
                return (ISolutionModel)dataContractSerializer.ReadObject(xmlReader);
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
            }
        }

        ///<summary>
        /// Reads the associated XML of class Model T from a string into an
        /// instance of class model T and returns it.
        ///
        /// An exception is thrown if the XML appears to be invalid for class model T.
        ///</summary>
        ///<param name="input"></param>
        public static ISolutionModel ReadXmlFromString<T>(string input)
        {
            using (var inputStream = new StringReader(input))
            {
                XmlReader xmlReader = null;
                try
                {
                    xmlReader = XmlReader.Create(inputStream, new XmlReaderSettings
                    {
                        CloseInput = true
                    });

                    var dataContractSerializer = new DataContractSerializer(typeof(SolutionModel));
                    return (SolutionModel)dataContractSerializer.ReadObject(xmlReader);
                }
                finally
                {
                    if (xmlReader != null)
                        xmlReader.Close();
                }
            }
        }
    }
}
