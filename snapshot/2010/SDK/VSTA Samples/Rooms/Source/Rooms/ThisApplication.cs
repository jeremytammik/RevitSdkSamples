using System;
using Revit.SDK.Samples.Rooms.CS;

namespace Rooms
{
    [System.AddIn.AddIn("ThisApplication", Version = "1.0", Publisher = "", Description = "")]
    public partial class ThisApplication
    {
        private void Module_Startup(object sender, EventArgs e)
        {

        }

        private void Module_Shutdown(object sender, EventArgs e)
        {

        }

        #region VSTA generated code
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
