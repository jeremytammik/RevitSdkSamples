using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;

namespace ASCE_7_10
{
    [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("LoadCombination", "68d87b7c-410e-4d19-a59f-1b6f0ded206a")]
    public class LoadCombination : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
    {
        [SchemaProperty()]
        [EnumControl(Presentation=PresentationMode.OptionList,
            Item=PresentationItem.Text,
            Category="General",
            Description="Generation method"
        )]
        public GenerationMethod Method{get;set;}
    
        public LoadCombination()
        {
        }
    }

    public enum GenerationMethod
    {
        ASD,
        LRFD
    }
}
