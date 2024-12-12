using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ExtensionRevitLauncher
{
    /// <summary>
    /// Reads modules configuration files and returns the list of supported module.
    /// </summary>
    class ExtensionXMLReader
    {
        const string xmlExtensionModule = "Module";
        const string xmlExtensionModules = "Modules";
        const string xmlName = "name";
        const string xmlPath = "path";
        const string xmlNamespace = "namespace";
        const string xmlImg = "img";
        const string xmlDescription = "description";
        const string xmlTechnology = "technology";

        /// <summary>
        /// Reads the modules configuration file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static List<ExtensionModuleData> ReadFile(string path)
        {
            List<ExtensionModuleData> modules = new List<ExtensionModuleData>();
            XmlDocument xmlDocument = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                xmlDocument.Load(path);

                foreach (XmlNode nodeModules in xmlDocument.ChildNodes)
                {
                    if (nodeModules.Name != xmlExtensionModules)
                        continue;

                    foreach (XmlNode nodeModule in nodeModules.ChildNodes)
                    {
                        if (nodeModule.Name == xmlExtensionModule)
                        {
                            ExtensionModuleData module = new ExtensionModuleData();
                            module.Namespace = ReadAttribute(nodeModule, xmlNamespace);
                            module.Description = ReadAttribute(nodeModule, xmlDescription);
                            module.Img = ReadAttribute(nodeModule, xmlImg);
                            module.Name = ReadAttribute(nodeModule, xmlName);
                            module.Path = ReadAttribute(nodeModule, xmlPath);
                            module.Technology = ReadAttribute(nodeModule, xmlTechnology);

                            modules.Add(module);
                        }
                    }
                }
            }

            return modules;
        }
        /// <summary>
        /// Reads the specified attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        static string ReadAttribute(XmlNode node, string attribute)
        {
            XmlNode attNode = node.Attributes.GetNamedItem(attribute);

            if (attNode != null)
                return attNode.ToString();

            return "";
        }

        
    }
}
