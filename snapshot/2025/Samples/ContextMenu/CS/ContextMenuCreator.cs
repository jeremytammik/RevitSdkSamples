//
// (C) Copyright 2003-2022 by Autodesk, Inc.
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
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.ContextMenu.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IContextMenuCreator,
   /// build context menu passed in.
   /// </summary>
   public class ContextMenuCreator : IContextMenuCreator
   {
      public void BuildContextMenu(Autodesk.Revit.UI.ContextMenu menu)
      {
         // Add a command menu item.
         var item1 = new CommandMenuItem("Show Selection", typeof(ShowSelection).FullName, typeof(ContextMenuApplication).Assembly.Location);
         item1.SetAvailabilityClassName(typeof(ShowSelection).FullName);
         menu.AddItem(item1);

         // Add a separator.
         menu.AddItem(new SeparatorItem());

         var subMenu = new Autodesk.Revit.UI.ContextMenu();
         var item2 = new CommandMenuItem("Show Selection", typeof(ShowSelection).FullName, typeof(ContextMenuApplication).Assembly.Location);
         item2.SetAvailabilityClassName(typeof(ShowSelection).FullName);
         subMenu.AddItem(item2);
         // Add a sub-menu item.
         menu.AddItem(new SubMenuItem("subMenu", subMenu));
      }
   }

}
