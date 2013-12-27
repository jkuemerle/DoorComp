using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Collections.Specialized;

using DoorComp.Common;

namespace VoteSource.SQL
{
    [Export(typeof(IVote))]
    public class VoteSource : IVote
    {
        private static SqlConnection _conn = null; 
        private static object _connLock = new object();

        static VoteSource()
        {
            if (null == _conn)
                InitConnection();
        }

        private static void InitConnection()
        {
            if(null == _conn)
                lock (_connLock)
                {
                    if (null == _conn)
                        _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["VoteSource"].ConnectionString);
                }
        }
        public bool PostVote(VoteInfo Vote)
        {
            AddVote(Vote);
            return true;
        }

        private void AddVote(VoteInfo Vote)
        {
            var id = CalculateVoterID(Vote);
            if(!string.IsNullOrEmpty(id))
            {
                if (null == _conn)
                    InitConnection();
                using (var cmd = new SqlCommand("MERGE INTO Votes AS A " +
                    "USING (SELECT @DoorID AS DoorID, @VoterID AS VoterID) B ON (A.DoorID  = B.DoorID) AND (A.VoterID = B.VoterID)" +
                    "WHEN NOT MATCHED THEN " +
                    "INSERT (DoorID, VoterID) VALUES(@DoorID, @VoterID);"))
                {
                    cmd.Parameters.Add(new SqlParameter("@DoorID", Vote.DoorID));
                    cmd.Parameters.Add(new SqlParameter("@VoterID", id));
                    if (_conn.State != System.Data.ConnectionState.Open)
                        _conn.Open();
                    cmd.Connection = _conn;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private string CalculateVoterID(VoteInfo Vote)
        {
            if (!string.IsNullOrEmpty(Vote.VoterID))
                return HashValue(Vote.VoterID, Vote.DoorID);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Vote.Payload.IP);
            foreach (var h in Vote.Payload.Headers.AllKeys)
                sb.AppendFormat("{0}:{1}\r\n", h, Vote.Payload.Headers[h]);
            foreach (var c in Vote.Payload.Cookies)
                sb.AppendFormat("{0}:{1}\r\n", c.Key, c.Value);
            if (sb.Length > 0)
                return HashValue(sb.ToString(), Vote.DoorID);
            return string.Empty;
        }

        private string HashValue(string Value, string Salt)
        {
            var payload = Encoding.Unicode.GetBytes(Value);
            payload = payload.Concat(Encoding.Unicode.GetBytes(Salt)).ToArray();
            var hasher = SHA1Managed.Create();
            var hash = hasher.ComputeHash(payload);
            return Convert.ToBase64String(hash);
        }
    }
}
