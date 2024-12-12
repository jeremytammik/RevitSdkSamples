//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.PerformanceAdviserControl.CS
{
   /// <summary>
   /// A class that implements IPerformanceAdviserRule.  This class implements several methods that will be
   /// run automatically when PerformanceAdviser::ExecuteRules or ExecuteAllRules is called.
   /// </summary>
    public class FlippedDoorCheck : Autodesk.Revit.DB.IPerformanceAdviserRule
    {
       #region Constructor
       /// <summary>
       /// Set up rule name, description, and error handling
       /// </summary>
        public FlippedDoorCheck()
        {
            m_name = "Flipped Door Check";
            m_description = "An API-based rule to search for and return any doors that are face-flipped";
            m_doorWarningId = new Autodesk.Revit.DB.FailureDefinitionId(new Guid("25570B8FD4AD42baBD78469ED60FB9A3"));
            m_doorWarning = Autodesk.Revit.DB.FailureDefinition.CreateFailureDefinition(m_doorWarningId, Autodesk.Revit.DB.FailureSeverity.Warning, "Some doors in this project are face-flipped.");
        }
       #endregion




       #region IPerformanceAdviserRule implementation
        /// <summary>
        /// Does some preliminary work before executing tests on elements.  In this case,
        /// we instantiate a list of FamilyInstances representing all doors that are flipped.
        /// </summary>
        /// <param name="document">The document being checked</param>
        public void InitCheck(Autodesk.Revit.DB.Document document)
        {
           if (m_FlippedDoors == null)
              m_FlippedDoors = new List<Autodesk.Revit.DB.ElementId>();
           else
              m_FlippedDoors.Clear();
           return;
        }
       
       /// <summary>
        /// This method does most of the work of the IPerformanceAdviserRule implementation.
        /// It is called by PerformanceAdviser.
        /// It examines the element passed to it (which was previously filtered by the filter
        /// returned by GetElementFilter() (see below)).  After checking to make sure that the
        /// element is an instance, it checks the FacingFlipped property of the element.
        /// 
        /// If it is flipped, it adds the instance to a list to be used later.
        /// </summary>
        /// <param name="document">The active document</param>
        /// <param name="element">The current element being checked</param>
        public void ExecuteElementCheck(Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.Element element)
        {
            if ((element is Autodesk.Revit.DB.FamilyInstance))
            {
               Autodesk.Revit.DB.FamilyInstance doorCurrent = element as Autodesk.Revit.DB.FamilyInstance;
                if (doorCurrent.FacingFlipped)
                    m_FlippedDoors.Add(doorCurrent.Id);
            }
         
        }

       /// <summary>
       /// This method is called by PerformanceAdviser after all elements in document
       /// matching the ElementFilter from GetElementFilter() are checked by ExecuteElementCheck().
       /// 
       /// This method checks to see if there are any elements (door instances, in this case) in the
       /// m_FlippedDoor instance member.  If there are, it iterates through that list and displays
       /// the instance name and door tag of each item.
       /// </summary>
       /// <param name="document">The active document</param>
        public void FinalizeCheck(Autodesk.Revit.DB.Document document)
        {
           if (m_FlippedDoors.Count == 0)
              System.Diagnostics.Debug.WriteLine("No doors were flipped.  Test passed.");

           else
           {
              //Pass the element IDs of the flipped doors to the revit failure reporting APIs.
              Autodesk.Revit.DB.FailureMessage fm = new Autodesk.Revit.DB.FailureMessage(m_doorWarningId);
              fm.SetFailingElements(m_FlippedDoors);
              Autodesk.Revit.DB.Transaction failureReportingTransaction = new Autodesk.Revit.DB.Transaction(document, "Failure reporting transaction");
              failureReportingTransaction.Start();
              Autodesk.Revit.DB.PerformanceAdviser.GetPerformanceAdviser().PostWarning(fm);
              failureReportingTransaction.Commit();
              m_FlippedDoors.Clear();
           }
      
        }

       /// <summary>
       /// Gets the description of the rule
       /// </summary>
       /// <returns>The rule description</returns>
        public string GetDescription()
        {
            return m_description;
        }

        /// <summary>
        /// This method supplies an element filter to reduce the number of elements that PerformanceAdviser
        /// will pass to GetElementCheck().  In this case, we are filtering for door elements.
        /// </summary>
        /// <param name="document">The document being checked</param>
        /// <returns>A door element filter</returns>
        public Autodesk.Revit.DB.ElementFilter GetElementFilter(Autodesk.Revit.DB.Document document)
        {
            return new Autodesk.Revit.DB.ElementCategoryFilter(Autodesk.Revit.DB.BuiltInCategory.OST_Doors);
        }

       /// <summary>
       /// Gets the name of the rule
       /// </summary>
       /// <returns>The rule name</returns>
        public string GetName()
        {
            return m_name;
        }



        /// <summary>
        /// Returns true if this rule will iterate through elements and check them, false otherwise
        /// </summary>
        /// <returns>True</returns>
        public bool WillCheckElements()
        {
            return true;
        }

        #endregion

       #region Other instance methods
        /// <summary>
        /// This method is used by PerformanceAdviser to get the
        ///  ID of the rule. It returns a global static field to make sharing the ID in different places
        ///  in the application easier.
        /// </summary>
        /// <returns>The Rule ID of this rule</returns>
        public Autodesk.Revit.DB.PerformanceAdviserRuleId getRuleId()
        {
           return FlippedDoorCheck.Id;
        }
        #endregion

        /// <summary>
        /// The rule ID for this rule;
        /// </summary>
        public static Autodesk.Revit.DB.PerformanceAdviserRuleId Id
        {
           get
           {
              return m_Id;
           }
        }

       #region Data
        /// <summary>
       /// A list of all family instances in the document that have the FaceFlipped property set to true;
       /// </summary>
        private List<Autodesk.Revit.DB.ElementId> m_FlippedDoors;

       /// <summary>
       /// A short name for the rule
       /// </summary>
        private string m_name;

       /// <summary>
       /// A short description of the rule
       /// </summary>
        private string m_description;

       /// <summary>
       /// The rule ID for this rule;
       /// </summary>
       private static Autodesk.Revit.DB.PerformanceAdviserRuleId m_Id = new Autodesk.Revit.DB.PerformanceAdviserRuleId((new Guid("BC38854474284491BD03795675AC7386")));

       /// <summary>
       ///  The ID of the failure definition for our API-based door flip check rule
       /// </summary>
       private Autodesk.Revit.DB.FailureDefinitionId m_doorWarningId;
       
       /// <summary>
       /// The failure definition for our API-based door flip check rule
       /// </summary>
       private Autodesk.Revit.DB.FailureDefinition m_doorWarning;
       
       #endregion

    }



 
}
