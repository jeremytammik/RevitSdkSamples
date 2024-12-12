using System;

namespace Rooms
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.UI.Macros.AddInIdAttribute("51FFA7F2-0DA4-4D7B-8D7E-FED1897D4F8D")]
    public partial class ThisApplication
    {
        private void Module_Startup(object sender, EventArgs e)
        {

        }

        private void Module_Shutdown(object sender, EventArgs e)
        {

        }

        #region Revit Macros generated code
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Module_Startup);
            this.Shutdown += new System.EventHandler(Module_Shutdown);
        }
        #endregion

        public void GetAllRoomsInformation()
        {
            SamplesRoom sampleRoom = new SamplesRoom(this);
            sampleRoom.Run();
        }
    }
}
