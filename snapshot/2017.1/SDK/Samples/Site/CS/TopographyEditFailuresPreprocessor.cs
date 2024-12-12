//
// (C) Copyright 2003-2016 by Autodesk, Inc. All rights reserved.
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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.Site.CS
{
    class TopographyEditFailuresPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            return FailureProcessingResult.Continue;
        }

    }

    class TopographyEditFailuresPreprocessorVerbose : IFailuresPreprocessor
    {
        // For debugging
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            try
            {
                TaskDialog.Show("Preprocess failures", "Hello");
                IList<FailureMessageAccessor> failureMessages = failuresAccessor.GetFailureMessages();
                int numberOfFailures = failureMessages.Count;
                TaskDialog.Show("Preprocess failures", "Found " + numberOfFailures + " failure messages.");
                if (numberOfFailures < 5)
                {
                    foreach (FailureMessageAccessor msgAccessor in failureMessages)
                    {
                        TaskDialog.Show("Failure!", msgAccessor.GetDescriptionText());
                    }
                }
                else
                {
                    TaskDialog.Show("Failure 1 of " + numberOfFailures, failureMessages.First<FailureMessageAccessor>().GetDescriptionText());
                }
                TaskDialog.Show("Preprocess failures", "Goodbye");
                return FailureProcessingResult.Continue;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Exception", e.ToString());
                return FailureProcessingResult.ProceedWithRollBack;
            }
        }

    }
}
