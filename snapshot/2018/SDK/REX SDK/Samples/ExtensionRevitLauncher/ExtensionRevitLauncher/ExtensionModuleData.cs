using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionRevitLauncher
{
    /// <summary>
    /// The class represents properties of the particular module.
    /// </summary>
    class ExtensionModuleData
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the path to the module assembly.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets the main namespace of the module (in fact the namespace of the DirectAccess class).
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }
        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>The image path.</value>
        public string Img { get; set; }
        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the technology of the module.
        /// </summary>
        /// <value>The technology.</value>
        public string Technology { get; set; }
    }
}
