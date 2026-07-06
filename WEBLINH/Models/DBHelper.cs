using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WEBLINH.Models
{
    public static class DBHelper
    {
        private static readonly string ConnStr =
            ConfigurationManager.ConnectionStrings["WEBLINH_CN"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnStr);
        }

        // Bat buoc NVarChar cho tieng Viet (khi value la null se van luu duoc)
        public static SqlParameter NStr(string name, string value)
        {
            return new SqlParameter(name, SqlDbType.NVarChar, 4000)
            {
                Value = (object)value ?? System.DBNull.Value
            };
        }

        // NVarChar(MAX) cho mo ta
        public static SqlParameter NText(string name, string value)
        {
            return new SqlParameter(name, SqlDbType.NVarChar, -1)
            {
                Value = (object)value ?? System.DBNull.Value
            };
        }

        public static SqlParameter Int(string name, int value)
        {
            return new SqlParameter(name, SqlDbType.Int) { Value = value };
        }

        public static SqlParameter Decimal(string name, decimal value)
        {
            return new SqlParameter(name, SqlDbType.Decimal) { Value = value };
        }

        public static SqlParameter Bit(string name, bool value)
        {
            return new SqlParameter(name, SqlDbType.Bit) { Value = value };
        }

        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
