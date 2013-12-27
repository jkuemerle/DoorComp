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
            var cmd = new SqlCommand("SELECT Code, Description, Status, LogoURL FROM Events WHERE Status = @Status");
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
                    retVal.Add(new EventInfo() { Code = res.GetString(0), Description = res.GetString(1), Status = (EventStatus)Enum.Parse(typeof(EventStatus), res.GetInt32(2).ToString()), LogoURL = res.GetString(3) });
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

        public EventInfo GetEvent(string EventCode)
        {
            EventInfo ret = new EventInfo();
            if (null == _conn)
                InitConnection();
            var cmd = new SqlCommand("SELECT TOP 1 Code, Description, Status, LogoURL FROM Events WHERE Code = @Code");
            cmd.Connection = _conn;
            if (_conn.State == System.Data.ConnectionState.Closed)
                _conn.Open();
            cmd.Parameters.Add(new SqlParameter("@Code", EventCode));
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
                }
                return ret;
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
    }
}
