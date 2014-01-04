using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.SqlClient;

using DoorComp.Common;

namespace EventSource.SQL
{
    [Export(typeof(IEventSource))]
    public class EventSourceSQL : IEventSource
    {
        private static SqlConnection _conn = null;
        private static object _connLock = new object();

        static EventSourceSQL()
        {
            if (null == _conn)
                InitConnection();
        }

        private static void InitConnection()
        {
            lock(_connLock)
            {
                if (null == _conn)
                    _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EventSource"].ConnectionString);
            }
        }

        public IList<EventInfo> ListEvents(EventStatus Status)
        {
            var retVal = new List<EventInfo>();
            if (null == _conn)
                InitConnection();
            string statusClause = string.Empty;
            if(Status != EventStatus.None)
            {
                statusClause = " Status IN (";
                if (Status.HasFlag(EventStatus.Active))
                {
                    statusClause += string.Format("{0},", (int)EventStatus.Active);
                }
                if (Status.HasFlag(EventStatus.Closed))
                {
                    statusClause += string.Format("{0},", (int)EventStatus.Closed);
                }
                if (Status.HasFlag(EventStatus.Archived))
                {
                    statusClause += string.Format("{0},", (int)EventStatus.Archived);
                }
                if (statusClause.Substring(statusClause.Length - 1, 1) == ",")
                    statusClause = statusClause.Substring(0, statusClause.Length - 1);
                statusClause += ")";
            }
            var cmd = new SqlCommand(string.Format("SELECT Code, Description, Status, LogoURL FROM Events WHERE {0} ORDER BY Status, Code",statusClause));
            
            cmd.Connection = _conn;
            if (_conn.State == System.Data.ConnectionState.Closed)
                _conn.Open();
            cmd.Parameters.Add(new SqlParameter("@Status", Status));
            SqlDataReader res = null;
            try
            {
                res = cmd.ExecuteReader();
                while (res.Read())
                {
                    retVal.Add(new EventInfo() { Code = res.GetString(0), Description = res.GetString(1), 
                        Status = (EventStatus)Enum.Parse(typeof(EventStatus), res.GetInt32(2).ToString()), 
                        LogoURL = res.GetString(3), 
                        StatusString = Enum.Parse(typeof(EventStatus), res.GetInt32(2).ToString()).ToString(),
                        IsOpen = (EventStatus.Active == (EventStatus)Enum.Parse(typeof(EventStatus), res.GetInt32(2).ToString()))
                    });
                }
                return retVal;
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                if (null != res && !res.IsClosed)
                    res.Close();
            }
        }

        public EventInfo GetEventByID(string ID)
        {
            return GetEventDetails(ID, null);
        }
        public EventInfo GetEvent(string EventCode)
        {
            return GetEventDetails(null, EventCode);
        }

        private EventInfo GetEventDetails(string ID, string EventCode)
        {
            EventInfo ret = new EventInfo();
            if (null == _conn)
                InitConnection();
            SqlCommand cmd = new SqlCommand();
            string commandBase = "SELECT TOP 1 Code, Description, Status, LogoURL FROM Events WHERE ";
            if(!string.IsNullOrEmpty(ID))
            {
                commandBase = string.Format("{0} ID = @ID",commandBase);
                cmd.Parameters.Add(new SqlParameter("@ID", ID));
            }
            else
            {
                commandBase = string.Format("{0} Code = @Code",commandBase);
                cmd.Parameters.Add(new SqlParameter("@Code", EventCode));
            }
            cmd.CommandText = commandBase;
            cmd.Connection = _conn;
            if (_conn.State == System.Data.ConnectionState.Closed)
                _conn.Open();
            SqlDataReader res = null;
            try
            {
                res = cmd.ExecuteReader();
                if (res.HasRows && res.Read())
                {
                    ret.Code = res.GetString(0);
                    ret.Description = res.GetString(1);
                    ret.Status = (EventStatus)Enum.Parse(typeof(EventStatus), res.GetInt32(2).ToString());
                    ret.LogoURL = res.GetString(3);
                    ret.StatusString = Enum.Parse(typeof(EventStatus), res.GetInt32(2).ToString()).ToString();
                    ret.IsOpen = (EventStatus.Active == (EventStatus)Enum.Parse(typeof(EventStatus), res.GetInt32(2).ToString()));
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (null != res && !res.IsClosed)
                    res.Close();
            }
        }
    }
}
