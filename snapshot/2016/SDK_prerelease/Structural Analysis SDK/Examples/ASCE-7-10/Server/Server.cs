using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB.CodeChecking;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.CodeChecking.Storage.LoadCombination;
using Autodesk.Revit.DB.CodeChecking.LoadCombination;
using Autodesk.Revit.DB.Structure;

namespace ASCE_7_10
{
public class Server : Autodesk.Revit.DB.CodeChecking.LoadCombination.Server<LoadCombination>
{
    public static readonly Guid ID = new Guid("34ce88f1-b265-4ab3-a8c2-ff6a49aef797");
    #region ICodeCheckingServer Members

    public override List<LoadCombinationPreview> GeneratePreview(Autodesk.Revit.DB.CodeChecking.LoadCombination.ServiceData data)
    {
        LoadCombinationManager manager = LoadCombinationManager.GetManager(data.Document);

        LoadCombination loadCombination = manager.LoadCombinationParams.GetEntity<LoadCombination>(data.Document);

        if (loadCombination != null)
        {
            List<LoadCombinationPreview> combinations = new List<LoadCombinationPreview>();

            LoadCaseArray selectedCases = manager.LoadCombinationParams.GetLoadCasesArray(ID);

            Dictionary<BuiltInCategory,LoadCaseArray> groups = LoadCombinationUtils.GroupLoadCasesDueToCategory(selectedCases);

            List<LoadCaseArray> dead = LoadCombinationUtils.PerformAnd(groups[BuiltInCategory.OST_LoadCasesDead]);
            List<LoadCaseArray> earthquake = LoadCombinationUtils.PerformOrExlusive(groups[BuiltInCategory.OST_LoadCasesSeismic]);
            List<LoadCaseArray> live = LoadCombinationUtils.PerformOrInclusive(groups[BuiltInCategory.OST_LoadCasesLive]);
            List<LoadCaseArray> roof = LoadCombinationUtils.PerformOrInclusive(groups[BuiltInCategory.OST_LoadCasesRoofLive]);
            List<LoadCaseArray> snow = LoadCombinationUtils.PerformOrExlusive(groups[BuiltInCategory.OST_LoadCasesSnow]);
            List<LoadCaseArray> wind = LoadCombinationUtils.PerformOrExlusive(groups[BuiltInCategory.OST_LoadCasesWind]);

            List<LoadCombinationPreview> previews;

            if (loadCombination.Method == GenerationMethod.LRFD)
            {
                //1.4D
                previews = LoadCombinationUtils.CreateLoadCombinations("1.4D", dead, 1.4);
                combinations.AddRange(previews);

                //1.2D+1.6l+0.5(Lr or S)
                //+ 1.2D+1.6L+0.5Lr
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.6L+0.5Lr", dead, 1.2, live, 1.6, roof, 0.5);
                combinations.AddRange(previews);
                //+ 1.2D+1.6L+0.5S
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.6L+0.5S", dead, 1.2, live, 1.6, snow, 0.5);
                combinations.AddRange(previews);

                //1.2D+1.6(Lr or S) + (L or 0.5W)
                //+1.2D+1.6Lr + L
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.6Lr + L",dead, 1.2, roof, 1.6, live, 1.0);
                combinations.AddRange(previews);
                //+1.2D+1.6Lr + 0.5W
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.6Lr + 0.5W",dead, 1.2, roof, 1.6, wind, 0.5);
                combinations.AddRange(previews);
                //+1.2D+1.6S + L
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.6S + L",dead, 1.2, snow, 1.6, live, 1.0);
                combinations.AddRange(previews);
                //+1.2D+1.6S + 0.5W
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.6S + 0.5W", dead, 1.2, snow, 1.6, wind, 0.5);
                combinations.AddRange(previews);

                //1.2D+1.0W+L+0.5(Lr or S)
                //+1.2D+1.0W+L+0.5Lr
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.0W+L+0.5Lr",dead, 1.2, wind, 1.0, live, 1.0,roof,0.5);
                combinations.AddRange(previews);
                //+1.2D+1.0W+L+0.5S
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.0W+L+0.5S",dead, 1.2, wind, 1.0, live, 1.0, snow, 0.5);
                combinations.AddRange(previews);

                //1.2D+1.0E+L+0.2S
                previews = LoadCombinationUtils.CreateLoadCombinations("1.2D+1.0E+L+0.2S", dead, 1.2, earthquake, 1.0, live, 1.0, snow, 0.2);
                combinations.AddRange(previews);

                //0.9D+1.0W
                previews = LoadCombinationUtils.CreateLoadCombinations("0.9D+1.0W",dead, 0.9, wind, 1.0);
                combinations.AddRange(previews);

                //0.9D+1.0E
                previews = LoadCombinationUtils.CreateLoadCombinations("0.9D+1.0E", dead, 0.9, earthquake, 1.0);
                combinations.AddRange(previews);
            }
            else
            {
                //D
                previews = LoadCombinationUtils.CreateLoadCombinations("D", dead, 1.0);
                combinations.AddRange(previews);
                //D+L
                previews = LoadCombinationUtils.CreateLoadCombinations("D+L", dead, 1.0,live,1.0);
                combinations.AddRange(previews);
                //D+(Lr or S)
                //+D+Lr
                previews = LoadCombinationUtils.CreateLoadCombinations("D+Lr", dead, 1.0, roof,1.0);
                combinations.AddRange(previews);
                //+D+S
                previews = LoadCombinationUtils.CreateLoadCombinations("D+S", dead, 1.0, snow, 1.0);
                combinations.AddRange(previews);
                //D+0.75L+0.75(Lr or S)
                //+D+0.75L+0.75Lr
                previews = LoadCombinationUtils.CreateLoadCombinations("D+0.75L+0.75Lr", dead, 1.0, live, 0.75,roof,0.75);
                combinations.AddRange(previews);
                //+D+0.75L+0.75S
                previews = LoadCombinationUtils.CreateLoadCombinations("D+0.75L+0.75S", dead, 1.0, live, 0.75, snow, 0.75);
                combinations.AddRange(previews);
                //D+(0.6W or 0.7E)
                //D+0.6W
                previews = LoadCombinationUtils.CreateLoadCombinations("D+0.6W", dead, 1.0, wind, 0.6);
                combinations.AddRange(previews);
                //D+0.7E
                previews = LoadCombinationUtils.CreateLoadCombinations("D+0.7E", dead, 1.0, earthquake, 0.7);
                combinations.AddRange(previews);
                //D+0.75(0.6W)+0.75L+0.75(Lr or S)
                //+D+0.75(0.6W)+0.75L+0.75Lr
                previews = LoadCombinationUtils.CreateLoadCombinations("D+0.75(0.6W)+0.75L+0.75Lr", dead, 1.0, wind, 0.75*0.6,live,0.75,roof,0.75);
                combinations.AddRange(previews);
                //+D+0.75(0.6W)+0.75L+0.75S
                previews = LoadCombinationUtils.CreateLoadCombinations("D+0.75(0.6W)+0.75L+0.75S", dead, 1.0, wind, 0.75 * 0.6, live, 0.75, snow, 0.75);
                combinations.AddRange(previews);
                //D+0.75(0.7E)+0.75L+0.75S
                previews = LoadCombinationUtils.CreateLoadCombinations("D+0.75(0.7E)+0.75L+0.75S", dead, 1.0, earthquake, 0.75 * 0.7, live, 0.75, snow, 0.75);
                combinations.AddRange(previews);
                //0.6D+0.6W
                previews = LoadCombinationUtils.CreateLoadCombinations("0.6D+0.6W", dead, 0.6, wind, 0.6);
                combinations.AddRange(previews);
                //0.6D+0.7E
                previews = LoadCombinationUtils.CreateLoadCombinations("0.6D+0.7E", dead, 0.6, earthquake, 0.7);
                combinations.AddRange(previews);

            }


            return combinations;
        }

        return null;
    }

    public override bool RemoveLoadCombinations()
    {
        return true;
    }

    #endregion

    #region IExternalServer Members

    public override string GetDescription()
    {
        return "This is the code which generate load combinations according ASCE-7-10";
    }

    public override string GetName()
    {
        return "ASCE-7-10";
    }

    public override Guid GetServerId()
    {
        return ID;
    }

    public override string GetVendorId()
    {
        return "John Smith";
    }

    #endregion
}
}
