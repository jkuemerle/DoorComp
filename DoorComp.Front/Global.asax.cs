using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.IO;

using ServiceStack;
using ServiceStack.ServiceHost;
using DoorComp.Common;

namespace DoorComp.Front
{
    public class Global : System.Web.HttpApplication
    {
        [Import]
        private IEventSource _eventSource;

        [Import]
        private IPictureSource _picSource;
        [Import]
        private IVote _voteSource;
        [Import]
        private IClaimSource _claimSource;
        [Import]
        private IDoorSource _doorSource;

        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
            AddSources();
        }

        private void AddSources()
        {
            var p = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "bin");
            var catalog = new DirectoryCatalog(p, "*.dll");

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            if (null != this._eventSource)
                this.Application.Add("EventSource", this._eventSource);
            if(null != this._picSource)
            {
                if(this._picSource.RequiresCredentials)
                {
                    var creds = new PictureCredentials(System.IO.File.ReadAllLines(@"c:\temp\flickrcred.txt"));
                    this._picSource.Init(creds);
                }
                this.Application.Add("PhotoSource", this._picSource);
            }
            if(null != this._voteSource)
                this.Application.Add("VoteSource", this._voteSource);
            if(null != this._claimSource)
                this.Application.Add("ClaimSource", this._claimSource);
            if (null != this._doorSource)
                this.Application.Add("DoorSource", this._doorSource);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}