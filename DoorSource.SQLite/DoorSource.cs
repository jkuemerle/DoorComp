using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.SQLite;

using DoorComp.Common;


namespace DoorSource.SQLite
{
    [Export(typeof(IDoorSource))]
    public class DoorSource : IDoorSource
    {
        public DoorInfo GetDoor(string DoorID)
        {
            return Retrieve(DoorID);
        }

        private DoorInfo Retrieve(string DoorID)
        {
            var ret = new DoorInfo();
            var cs = ConfigurationManager.ConnectionStrings["DoorSource"].ConnectionString;
            var conn = new SQLiteConnection(cs);
            conn.Open();
            var cmd = new SQLiteCommand(conn);
            cmd.CommandText = "SELECT DoorID, Location, Description, EventID FROM Doors WHERE DoorID = @DoorID LIMIT 1";
            cmd.Parameters.Add(new SQLiteParameter("@DoorID", DoorID));
            SQLiteDataReader res = null;
            try
            {
                res = cmd.ExecuteReader();
                if (res.HasRows && res.Read())
                {
                    ret.DoorID = DoorID;
                    ret.Location = res.GetString(1);
                    ret.Description = res.GetString(2);
                    ret.EventID = res.GetInt64(3);
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

