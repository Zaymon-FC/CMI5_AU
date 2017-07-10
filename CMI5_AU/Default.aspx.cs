using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMI5_AU.Services;

namespace CMI5_AU
{
    public partial class _Default : Page
    {

        XApiService _xApiService = new XApiService();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Launched_Click(object sender, EventArgs e)
        {
            _xApiService.SendStatement("launched");
        }

        protected void Terminated_Click(object sender, EventArgs e)
        {
            _xApiService.SendStatement("terminated");
        }

        protected void Passed_Click(object sender, EventArgs e)
        {
            _xApiService.SendStatement("passed");
        }

        protected void Completed_Click(object sender, EventArgs e)
        {
            _xApiService.SendStatement("completed");
        }

        protected void Progressed_Click(object sender, EventArgs e)
        {
            _xApiService.SendStatement("progressed");
        }

        protected void Initialized_Click(object sender, EventArgs e)
        {
            _xApiService.SendStatement("initialized");
        }

        protected void AllButtons_Click(object sender, EventArgs e)
        {
            Launched_Click(sender, e);
            System.Threading.Thread.Sleep(100);
            Initialized_Click(sender, e);
            System.Threading.Thread.Sleep(100);
            Progressed_Click(sender, e);
            System.Threading.Thread.Sleep(100);
            Completed_Click(sender, e);
            System.Threading.Thread.Sleep(100);
            Passed_Click(sender, e);
            System.Threading.Thread.Sleep(100);
            Terminated_Click(sender, e);
        }
    }
}