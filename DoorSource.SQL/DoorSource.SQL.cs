using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel.Composition;
using System.Data.SqlClient;

using DoorComp.Common;

namespace DoorSource.SQL
{
    [Export(typeof(IDoorSource))]
    public class DoorSourceSQL : IDoorSource
    {
        private static SqlConnection _conn = null;
        private static object _connLock = new object();

        static DoorSourceSQL()
        {
            if (null == _conn)
                InitConnection();
        }

        private static void InitConnection() {
            if(null == _conn)
                lock(_connLock)
                {
                    if(null == _conn)
                        _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DoorSource"].ConnectionString);
                }
        }

        public DoorInfo GetDoor(string DoorID)
        {
            return Retrieve(DoorID);
        }

        private DoorInfo Retrieve(string DoorID)
        {
            if (null == _conn)
                InitConnection();
            var ret = new DoorInfo();
            var cs = ConfigurationManager.ConnectionStrings["DoorSource"].ConnectionString;
            using(var cmd = new SqlCommand("SELECT TOP 1 DoorID, Location, Description FROM Doors WHERE DoorID = @DoorID")) {
                if(_conn.State != System.Data.ConnectionState.Open)
                    _conn.Open();
                cmd.Connection = _conn;
                cmd.Parameters.Add(new SqlParameter("@DoorID", DoorID));
                SqlDataReader res = null;
                try
                {
                    res = cmd.ExecuteReader();
                    if (res.HasRows && res.Read())
                    {
                        ret.DoorID = DoorID;
                        ret.Location = res.GetString(1);
                        ret.Description = res.GetString(2);
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
}
