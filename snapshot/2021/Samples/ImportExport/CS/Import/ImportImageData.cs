
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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
   /// <summary>
   /// Data class which stores the information for importing image format
   /// </summary>
   class ImportImageData : ImportData
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="commandData">Revit command data</param>
      /// <param name="importFormat">Format to import</param>
      public ImportImageData(ExternalCommandData commandData, ImportFormat importFormat)
          : base(commandData, importFormat)
      {
         if (OptionalFunctionalityUtils.IsPDFImportAvailable())
            m_filter = "All Image Files (*.bmp, *.gif, *.jpg, *.jpeg, *.pdf, *.png, *.tif)|*.bmp;*.gif;*.jpg;*.jpeg;*.pdf;*.png;*.tif";
         else
            m_filter = "All Image Files (*.bmp, *.gif, *.jpg, *.jpeg, *.png, *.tif)|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif";

         m_title = "Import Image";
      }

      /// <summary>
      /// Collect the parameters and export
      /// </summary>
      /// <returns></returns>
      public override bool Import()
      {
         using (Transaction t = new Transaction(m_activeDoc))
         {
            t.SetName("Import");
            t.Start();

            // Step 1: Create an ImageType 
            //ImageTypeOptions specifies the source of the image, and how to create the ImageType.
            // It can be used to specify:
            //   - The image file.  Either as a local file path, or as an ExternalResourceRef
            //   - Whether to store the local file path as an absolute path or a relative path.
            //   - Whether to create an Import or a Link image.
            // In addition, if the source is a PDF file, then ImageTypeOptions can be used to specify:
            //   - which page from the PDF to use
            //   - the resolution (in dots-per-inch) at which to rasterize the PDF
            // For other image types the page number should be 1 (the default),
            // and the resolution is only used to determine the size of the image.

            ImageTypeOptions typeOptions = new ImageTypeOptions(m_importFileFullName, true, ImageTypeSource.Import);
            ImageType imageType = ImageType.Create(m_activeDoc, typeOptions);

            // Step 2: Create an ImageInstance, but only if the active view is able to contain images.
            View view = CommandData.Application.ActiveUIDocument.Document.ActiveView;
            if (ImageInstance.IsValidView(view))
            {

               // ImagePlacementOptions
               ImagePlacementOptions placementOptions = new ImagePlacementOptions();
               placementOptions.PlacementPoint = Autodesk.Revit.DB.BoxPlacement.TopLeft;
               placementOptions.Location = new XYZ(1, 1, 1);

               ImageInstance imageInstance = ImageInstance.Create(m_activeDoc, view, imageType.Id, placementOptions);
            }

            t.Commit();
         }

         return true;
      }
   }
}
