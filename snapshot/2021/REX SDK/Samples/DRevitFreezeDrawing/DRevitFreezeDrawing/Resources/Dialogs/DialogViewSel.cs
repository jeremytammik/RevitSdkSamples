//
// (C) Copyright 2016 by Autodesk, Inc. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using REX.Common;
using Autodesk.REX.Framework;

namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
   /// <summary>
   /// Dialog to select views to be exported/freezed
   /// </summary>
   partial class DialogViewSel : REXExtensionForm
   {
      #region<members>
      List<REXView> m_REXViewList;
      #endregion

      public DialogViewSel(REXExtension argExt) : base(argExt)
      {
         m_REXViewList = new List<REXView>();

         InitializeComponent();
         this.Icon = ThisExtension.GetIcon();
      }
      //***********************************************************************
      public void SetLists(List<Autodesk.Revit.DB.View> argViewList)
      {
         m_REXViewList.Clear();

         foreach (Autodesk.Revit.DB.View v in argViewList)
         {
            m_REXViewList.Add(new REXView(v));
         }

         m_REXViewList.Sort(CompareViewsByName);
         checkedList_Views.Items.Clear();

         InitCheckedList();

         CheckNone();
      }
      //***********************************************************************
      private void InitCheckedList()
      {
         foreach (REXView rv in m_REXViewList)
         {
            checkedList_Views.Items.Add(rv.Name);
            rv.Tag = checkedList_Views.Items.Count - 1;
            rv.ifVisible = true;
         }
      }

      //***********************************************************************
      private void CheckNone()
      {
         for (int i = 0; i < checkedList_Views.Items.Count; i++)
            checkedList_Views.SetItemChecked(i, false);
      }
      //***********************************************************************
      private void CheckAll()
      {
         for (int i = 0; i < checkedList_Views.Items.Count; i++)
            checkedList_Views.SetItemChecked(i, true);
      }

      //***********************************************************************
      /// <summary>
      /// this function returns SelectedViews
      /// </summary>
      /// <returns></returns>
      public List<Autodesk.Revit.DB.View> GetSelectedViews()
      {
         List<Autodesk.Revit.DB.View> list = new List<Autodesk.Revit.DB.View>();
         foreach (REXView rv in m_REXViewList)
         {
            if (rv.ifChecked)
            {
               list.Add(rv.ViewElement);
            }
         }
         return list;
      }

      //*************************************************************************
      /// <summary>
      /// this functions get checked items to m_REXViewList
      /// </summary>
      private void GetCheckedItems()
      {
         foreach (REXView rv in m_REXViewList)
         {
            rv.ifChecked = false;
         }

         for (int i = 0; i < checkedList_Views.Items.Count; i++)
         {
            bool check = checkedList_Views.GetItemChecked(i);
            if (check)
            {
               foreach (REXView rv in m_REXViewList)
               {
                  if (rv.Tag == i)
                  {
                     rv.ifChecked = true;
                     break;
                  }
               }
            }
         }
      }
      //***********************************************************************
      private void CheckFromSelection()
      {
         foreach (REXView rv2 in m_REXViewList)
         {
            if (rv2.ifChecked)
            {
               checkedList_Views.SetItemChecked(rv2.Tag, true);
            }
         }
      }
      //***********************************************************************
      private void DialogViewSel_Load(object sender, EventArgs e)
      {
         CheckNone();
         CheckFromSelection();
      }
      //***********************************************************************
      private void button_CheckNone_Click(object sender, EventArgs e)
      {
         CheckNone();
      }
      //***********************************************************************
      private void button_CheckAll_Click(object sender, EventArgs e)
      {
         CheckAll();
      }
      //***********************************************************************
      private void button_OK_Click(object sender, EventArgs e)
      {
         GetCheckedItems();
         this.Hide();
      }

      //***********************************************************************
      private void button_Cancel_Click_1(object sender, EventArgs e)
      {
         this.Hide();
      }

      private void Help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         REXCommand Cmd = new REXCommand(REXCommandType.Help);
         ThisExtension.Command(ref Cmd);
      }

      /// <summary>
      /// This function is an Comparison to sort views by name
      /// </summary>
      private static int CompareViewsByName(REXView arg1, REXView arg2)
      {
         return arg1.Name.CompareTo(arg2.Name);
      }
   }
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************


   internal class REXView
   {
      public string Name;
      public Autodesk.Revit.DB.View ViewElement;
      public bool ifView;
      public int Tag;
      public bool ifChecked;
      public bool ifVisible;

      public REXView()
      {
      }

      public REXView(Autodesk.Revit.DB.View argView)
      {
         ViewElement = argView;
         Name = argView.get_Parameter(BuiltInParameter.VIEW_FAMILY).AsString() + ": " + argView.Name;
         if (argView.ViewType == Autodesk.Revit.DB.ViewType.DrawingSheet)
            ifView = false;
         else
            ifView = true;
      }

      public REXView(REXView argRV)
      {
         this.ifChecked = argRV.ifChecked;
         this.ifVisible = argRV.ifVisible;
         this.Tag = argRV.Tag;
         this.ifView = argRV.ifView;
         this.Name = argRV.Name;
         this.ViewElement = argRV.ViewElement;
      }

      public void CopyMembersTo(REXView argRV)
      {
         argRV.ifChecked = this.ifChecked;
         argRV.ifView = this.ifView;
         argRV.ifVisible = this.ifVisible;
         argRV.Name = this.Name;
         argRV.Tag = this.Tag;
         argRV.ViewElement = this.ViewElement;
      }

      public void CopyDlgImportantMembFrom(REXView argRV)
      {
         this.ifChecked = argRV.ifChecked;
         this.ifVisible = argRV.ifVisible;
         this.Tag = argRV.Tag;
      }
   }
}