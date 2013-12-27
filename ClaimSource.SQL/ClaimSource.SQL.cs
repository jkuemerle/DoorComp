using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel.Composition;
using System.Data.SqlClient;

using DoorComp.Common;

namespace ClaimSource.SQL
{
    [Export(typeof(IClaimSource))]
    public class ClaimSourceSQL : IClaimSource
    {

        private static SqlConnection _conn = null;
        private static object _connLock = new object();

        static ClaimSourceSQL()
        {
            if(null == _conn)
                InitConnection();
        }

        private static void InitConnection()
        {
            if(null == _conn)
                lock(_connLock)
                {
                    if(null == _conn)
                    {
                        _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClaimSource"].ConnectionString);
                    }
                }
        }

        public bool Claim(string DoorID, ClaimInfo Claim)
        {
            if (null == _conn)
                InitConnection();
            using (var cmd = new SqlCommand("MERGE INTO Claims AS A " +
                "USING (SELECT @DoorID AS DoorID) B ON (A.DoorID  = B.DoorID) " +
                "WHEN MATCHED THEN " +
                "UPDATE SET A.Name  = @Name, A.Email = @Email " +
                "WHEN NOT MATCHED THEN " +
                "INSERT (DoorID, Name, Email) VALUES(@DoorID, @Name, @Email);"))
            {
                cmd.Parameters.Add(new SqlParameter("@DoorID", DoorID));
                cmd.Parameters.Add(new SqlParameter("@Name", Claim.Name));
                cmd.Parameters.Add(new SqlParameter("@Email", Claim.Email));
                if (_conn.State != System.Data.ConnectionState.Open)
                    _conn.Open();
                cmd.Connection = _conn;
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                return false;
            }
        }

        public ClaimInfo GetClaim(string DoorID)
        {
            if (null == _conn)
                InitConnection();
            using (var cmd = new SqlCommand("SELECT TOP 1 DoorID, Name, Email FROM Claims WHERE DoorID = @DoorID"))
            {
                cmd.Parameters.Add(new SqlParameter("@DoorID", DoorID));
                if (_conn.State != System.Data.ConnectionState.Open)
                    _conn.Open();
                cmd.Connection = _conn;
                SqlDataReader res = null;
                try
                {
                    res = cmd.ExecuteReader();
                    if (res.HasRows && res.Read())
                        return new ClaimInfo()
                        {
                            DoorID = DoorID,
                            Name = res.GetString(1),
                            Email = res.GetString(2)
                        };
                    return null;
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
