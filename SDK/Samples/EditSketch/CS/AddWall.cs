using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.EditSketch.CS
{
   internal class AddWall
   {
      // See API docs for Wall.Create for additional details
      // Returns the ID of the wall created
      public static Wall execute(Document doc, XYZ start, XYZ end, double height, String levelName)
      {

         Wall w = null;

         using (Transaction t = new Transaction(doc, "Creating wall"))
         {

            t.Start();

            // Creating a line for the location 
            Line line = Line.CreateBound(start, end);

            // Find level
            FilteredElementCollector fec = new FilteredElementCollector(doc);
            fec.OfClass(typeof(Level));
            Level level = fec.Cast<Level>().First<Level>(lvl => lvl.Name == levelName);
 
            double offset = 0;
            ElementId defaultElementTypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.WallType);

            // Create a wall using the location line and wall type
            w = Wall.Create(doc, line, defaultElementTypeId, level.Id, height, offset, true, true);

            // Important for the wall to have a profile sketch if we would like to edit it
            w.CreateProfileSketch();

            t.Commit();
         }

         return w;
      }
   }
}
