using System.Collections.Generic;
using System.Data.SqlClient;

namespace WEBLINH.Models
{
    public static class DanhMucDAO
    {
        public static List<DanhMuc> GetAll()
        {
            var list = new List<DanhMuc>();
            var dt = DBHelper.ExecuteQuery("SELECT MaDM, TenDM, MoTa FROM DanhMuc ORDER BY MaDM");
            foreach (System.Data.DataRow r in dt.Rows)
            {
                list.Add(new DanhMuc
                {
                    MaDM = (int)r["MaDM"],
                    TenDM = r["TenDM"].ToString(),
                    MoTa = r["MoTa"] == System.DBNull.Value ? "" : r["MoTa"].ToString()
                });
            }
            return list;
        }

        public static DanhMuc GetById(int id)
        {
            var dt = DBHelper.ExecuteQuery(
                "SELECT MaDM, TenDM, MoTa FROM DanhMuc WHERE MaDM=@id",
                DBHelper.Int("@id", id));
            if (dt.Rows.Count == 0) return null;
            var r = dt.Rows[0];
            return new DanhMuc
            {
                MaDM = (int)r["MaDM"],
                TenDM = r["TenDM"].ToString(),
                MoTa = r["MoTa"] == System.DBNull.Value ? "" : r["MoTa"].ToString()
            };
        }

        public static int Insert(DanhMuc dm)
        {
            return DBHelper.ExecuteNonQuery(
                "INSERT INTO DanhMuc(TenDM, MoTa) VALUES(@t,@m)",
                DBHelper.NStr("@t", dm.TenDM),
                DBHelper.NStr("@m", dm.MoTa));
        }

        public static int Update(DanhMuc dm)
        {
            return DBHelper.ExecuteNonQuery(
                "UPDATE DanhMuc SET TenDM=@t, MoTa=@m WHERE MaDM=@id",
                DBHelper.NStr("@t", dm.TenDM),
                DBHelper.NStr("@m", dm.MoTa),
                DBHelper.Int("@id", dm.MaDM));
        }

        public static int Delete(int id)
        {
            return DBHelper.ExecuteNonQuery(
                "DELETE FROM DanhMuc WHERE MaDM=@id",
                DBHelper.Int("@id", id));
        }
    }
}
