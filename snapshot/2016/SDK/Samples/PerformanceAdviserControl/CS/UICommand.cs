//
// (C) Copyright 2003-2015 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.UI;
using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.PerformanceAdviserControl.CS
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   class UICommand : Autodesk.Revit.UI.IExternalCommand
   {
      #region IExternalCommand implementation


    /// <summary>
    /// Implement this method as an external command for Revit.
    /// </summary>
    /// <param name="commandData">An object that is passed to the external application 
    /// which contains data related to the command, 
    /// such as the application object and active view.</param>
    /// <param name="message">A message that can be set by the external application 
    /// which will be displayed if a failure or cancellation is returned by 
    /// the external command.</param>
    /// <param name="elements">A set of elements to which the external application 
    /// can add elements that are to be highlighted in case of failure or cancellation.</param>
    /// <returns>Return the status of the external command. 
    /// A result of Succeeded means that the API external method functioned as expected. 
    /// Cancelled can be used to signify that the user cancelled the external operation 
    /// at some point. Failure should be returned if the application is unable to proceed with 
    /// the operation.
    /// </returns>
      public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {

         //A list of rule information to be used below
         List<RuleInfo> ruleInfoList = new List<RuleInfo>();

         #region Get Rule info through iteration from PerformanceAdviser

            ///Here, we query the information about rules registered in PerformanceAdviser so that
            ///we can later decide in a dialog which rules we want to run.

        PerformanceAdviser performanceAdviser = PerformanceAdviser.GetPerformanceAdviser();


            ICollection<PerformanceAdviserRuleId> allIds = performanceAdviser.GetAllRuleIds();
            foreach (PerformanceAdviserRuleId ruleID in allIds)
            {
               string ruleName = performanceAdviser.GetRuleName(ruleID);
               string ruleDescription = performanceAdviser.GetRuleDescription(ruleID);
               bool isEnabled = performanceAdviser.IsRuleEnabled(ruleID);
               
               //We want to mark user-defined (API) rules, so we check to see if the current rule ID is
               //equal to the rule ID we created.
               bool isOurRule = (ruleID == FlippedDoorCheck.Id);

               RuleInfo oneRule = new RuleInfo(ruleID, isOurRule, ruleName, ruleDescription, isEnabled);
               ruleInfoList.Add(oneRule);
            }

         #endregion

         #region Prepare and show UI
         
           //This dialog box will allow the user to select and run performance rules, so it needs
           //the PerformanceAdviser and active document passed to it.
           TestDisplayDialog tdd = new TestDisplayDialog(PerformanceAdviser.GetPerformanceAdviser(), commandData.Application.ActiveUIDocument.Document);

           foreach (RuleInfo r in ruleInfoList)
               {
                  /// Add the rule data we just collected in the previous loop the the dialog box
                  /// we are about to show.
                  tdd.AddData(r.RuleName, r.IsOurRule, r.IsEnabled);
               }

          tdd.ShowDialog();
            #endregion

          return Autodesk.Revit.UI.Result.Succeeded;
      }
      #endregion
   }
}



