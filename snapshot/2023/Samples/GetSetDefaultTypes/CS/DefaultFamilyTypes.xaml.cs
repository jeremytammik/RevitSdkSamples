using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//
// (C) Copyright 2003-2019 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.GetSetDefaultTypes.CS
{
   /// <summary>
   /// Interaction logic for DefaultFamilyTypes.xaml
   /// </summary>
   public partial class DefaultFamilyTypes : Page, Autodesk.Revit.UI.IDockablePaneProvider
   {
      public static DockablePaneId PaneId = new DockablePaneId(new Guid("{DF0F08C3-447C-4615-B9B9-4843D821012E}"));

      public DefaultFamilyTypes()
      {
         InitializeComponent();

         _handler = new DefaultFamilyTypeCommandHandler();
         _event = Autodesk.Revit.UI.ExternalEvent.Create(_handler);

      }

      /// <summary>
      /// Sets document to the default family type pane.
      /// It will get all valid default types for the category and fill the data grid.
      /// </summary>
      public void SetDocument(Document document)
      {
         if (_document == document)
            return;

         _document = document;

         _dataGrid_DefaultFamilyTypes.Items.Clear();

         List<BuiltInCategory> categories = GetAllFamilyCateogries(_document);
         if (categories.Count < 1)
            return;

         foreach (int cid in categories)
         {
            FamilyTypeRecord record = new FamilyTypeRecord();
            record.CategoryName = Enum.GetName(typeof(BuiltInCategory), cid);

            //RLog.WriteComment(String.Format("The valid default family type candidates of {0} are:", Enum.GetName(typeof(BuiltInCategory), cid)));
            FilteredElementCollector collector = new FilteredElementCollector(_document);
            collector = collector.OfClass(typeof(FamilySymbol));
            var query = from FamilySymbol et in collector
                        where et.IsValidDefaultFamilyType(new ElementId(cid))
                        select et; // Linq query  

            ElementId defaultFamilyTypeId = _document.GetDefaultFamilyTypeId(new ElementId(cid));

            List<DefaultFamilyTypeCandidate> defaultFamilyTypeCandidates = new List<DefaultFamilyTypeCandidate>();
            foreach (FamilySymbol t in query)
            {
               DefaultFamilyTypeCandidate item = new DefaultFamilyTypeCandidate()
               {
                  Name = t.FamilyName + " - " + t.Name,
                  Id = t.Id,
                  CateogryId = new ElementId(cid)
               };
               defaultFamilyTypeCandidates.Add(item);
               if (t.Id == defaultFamilyTypeId)
                  record.DefaultFamilyType = item;
            }
            record.DefaultFamilyTypeCandidates = defaultFamilyTypeCandidates;


            int index = _dataGrid_DefaultFamilyTypes.Items.Add(record);

         }
      }

      private List<BuiltInCategory> GetAllFamilyCateogries(Document document)
      {
         FilteredElementCollector collector = new FilteredElementCollector(document);
         collector = collector.OfClass(typeof(Family));
         var query = collector.ToElements();

         List<BuiltInCategory> categoryids = new List<BuiltInCategory>();

         // The corresponding UI for OST_MatchModel is "Architecture->Build->Component"
         categoryids.Add(BuiltInCategory.OST_MatchModel);

         // The corresponding UI for OST_MatchModel is "Annotate->Detail->Component"
         categoryids.Add(BuiltInCategory.OST_MatchDetail);

         foreach (Family t in query)
            if (!categoryids.Contains(t.FamilyCategory.BuiltInCategory))
               categoryids.Add(t.FamilyCategory.BuiltInCategory);


         return categoryids;
      }

      #region IDockablePaneProvider Members

      public void SetupDockablePane(Autodesk.Revit.UI.DockablePaneProviderData data)
      {
         data.FrameworkElement = this as FrameworkElement;

         data.InitialState = new Autodesk.Revit.UI.DockablePaneState();
         data.InitialState.DockPosition = Autodesk.Revit.UI.DockPosition.Top;
      }

      #endregion

      private ExternalEvent _event = null;
      private DefaultFamilyTypeCommandHandler _handler;
      private Document _document;

      /// <summary>
      /// Responses to the type selection changed.
      /// It will set the selected type as default type.
      /// </summary>
      private void DefaultFamilyTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count == 1 && e.RemovedItems.Count == 1)
         {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            if (cb == null)
               return;

            DefaultFamilyTypeCandidate item = e.AddedItems[0] as DefaultFamilyTypeCandidate;
            if (item == null)
               return;

            _handler.SetData(item.CateogryId, item.Id);
            _event.Raise();
         }


      }
   }

   /// <summary>
   /// The default family type candidate.
   /// </summary>
   public class DefaultFamilyTypeCandidate
   {
      /// <summary>
      /// The name.
      /// </summary>
      public String Name
      {
         get;
         set;
      }

      /// <summary>
      /// The element id.
      /// </summary>
      public ElementId Id
      {
         get;
         set;
      }

      /// <summary>
      /// The category id.
      /// </summary>
      public ElementId CateogryId
      {
         get;
         set;
      }

      public override string ToString()
      {
         return Name;
      }
   }

   /// <summary>
   /// The element type record for the data grid.
   /// </summary>
   public class FamilyTypeRecord
   {
      /// <summary>
      /// The category name.
      /// </summary>
      public String CategoryName
      {
         get;
         set;
      }

      /// <summary>
      /// List of default family type candidates.
      /// </summary>
      public List<DefaultFamilyTypeCandidate> DefaultFamilyTypeCandidates
      {
         get;
         set;
      }

      /// <summary>
      /// The current default family type.
      /// </summary>
      public DefaultFamilyTypeCandidate DefaultFamilyType
      {
         get;
         set;
      }
   }

   /// <summary>
   /// The command handler to set current selection as default family type.
   /// </summary>
   public class DefaultFamilyTypeCommandHandler : IExternalEventHandler
   {
      ElementId _builtInCategory;
      ElementId _defaultTypeId;
      public void SetData(ElementId categoryId, ElementId typeId)
      {
         _builtInCategory = categoryId;
         _defaultTypeId = typeId;
      }

      public string GetName()
      {
         return "Reset Default family type";
      }


      public void Execute(Autodesk.Revit.UI.UIApplication revitApp)
      {
         using (Transaction tran = new Transaction(revitApp.ActiveUIDocument.Document, "Set Default family type to " + _defaultTypeId.ToString()))
         {
            tran.Start();
            revitApp.ActiveUIDocument.Document.SetDefaultFamilyTypeId(_builtInCategory, _defaultTypeId);
            tran.Commit();
         }
      }

   }  // class CommandHandler

}
