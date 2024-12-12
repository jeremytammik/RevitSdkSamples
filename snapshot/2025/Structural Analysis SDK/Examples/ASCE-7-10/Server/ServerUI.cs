using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace ASCE_7_10
{
    public class ServerUI : Autodesk.Revit.UI.CodeChecking.LoadCombination.Server<LoadCombination>
    {
        #region ICodeCheckingServerUI Members

        public override string GetResource(string key, string context)
        {
            string txt = ASCE_7_10.Properties.Resources.ResourceManager.GetString(key);

            if (!string.IsNullOrEmpty(txt))
                return txt;

            return key;
        }

        public override Uri GetResourceImage(string key, string context)
        {
            return new Uri(@"pack://application:,,,/ASCE_7_10;component/" + key);
        }

        #endregion
    }
}
