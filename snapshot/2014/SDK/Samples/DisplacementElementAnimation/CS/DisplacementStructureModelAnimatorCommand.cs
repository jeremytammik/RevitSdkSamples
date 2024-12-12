using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DisplacementElementAnimation.CS
{
    /// <summary>
    /// The command that initializes and starts the model animation.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class DisplacementStructureModelAnimatorCommand : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
           new DisplacementStructureModelAnimator(commandData.Application, true).StartAnimation();

           return Result.Succeeded;
        }

        #endregion
    }

    /// <summary>
    /// The command that initializes and starts the model animation step by step.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class DisplacementStructureModelAnimatorCommandStepByStep : IExternalCommand
    {
       #region IExternalCommand Members

       public static DisplacementStructureModelAnimator m_displacementstructuremodelAnimator;

       public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
       {
          if (m_displacementstructuremodelAnimator == null)
          {
             m_displacementstructuremodelAnimator = new DisplacementStructureModelAnimator(commandData.Application, false);
             m_displacementstructuremodelAnimator.StartAnimation();
          }
          else
          {
             m_displacementstructuremodelAnimator.AnimateNextStep();
          }

          return Result.Succeeded;
       }

       #endregion
    }
 }
