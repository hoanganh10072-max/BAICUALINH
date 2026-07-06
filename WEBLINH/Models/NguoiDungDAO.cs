using System;
using System.Data;

namespace WEBLINH.Models
{
    public static class NguoiDungDAO
    {
        private static NguoiDung Map(DataRow r)
        {
            return new NguoiDung
            {
                MaND = (int)r["MaND"],
                TenDN = r["TenDN"].ToString(),
                MatKhau = r["MatKhau"].ToString(),
                HoTen = r["HoTen"].ToString(),
                Email = r["Email"] == DBNull.Value ? "" : r["Email"].ToString(),
                SDT = r["SDT"] == DBNull.Value ? "" : r["SDT"].ToString(),
                DiaChi = r["DiaChi"] == DBNull.Value ? "" : r["DiaChi"].ToString(),
                VaiTro = r["VaiTro"].ToString(),
                NgayTao = Convert.ToDateTime(r["NgayTao"])
            };
        }

        public static NguoiDung Login(string tenDN, string matKhau)
        {
            var dt = DBHelper.ExecuteQuery(
                "SELECT * FROM NguoiDung WHERE TenDN=@u AND MatKhau=@p",
                DBHelper.NStr("@u", tenDN),
                DBHelper.NStr("@p", matKhau));
            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public static NguoiDung GetById(int id)
        {
            var dt = DBHelper.ExecuteQuery(
                "SELECT * FROM NguoiDung WHERE MaND=@id",
                DBHelper.Int("@id", id));
            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public static bool ExistsUsername(string tenDN)
        {
            var v = DBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM NguoiDung WHERE TenDN=@u",
                DBHelper.NStr("@u", tenDN));
            return Convert.ToInt32(v) > 0;
        }

        public static int Register(NguoiDung nd)
        {
            return DBHelper.ExecuteNonQuery(
                @"INSERT INTO NguoiDung(TenDN, MatKhau, HoTen, Email, SDT, DiaChi, VaiTro)
                  VALUES(@u,@p,@h,@e,@s,@d,@v)",
                DBHelper.NStr("@u", nd.TenDN),
                DBHelper.NStr("@p", nd.MatKhau),
                DBHelper.NStr("@h", nd.HoTen),
                DBHelper.NStr("@e", nd.Email),
                DBHelper.NStr("@s", nd.SDT),
                DBHelper.NStr("@d", nd.DiaChi),
                DBHelper.NStr("@v", "User"));
        }

        public static int UpdateThongTin(NguoiDung nd)
        {
            return DBHelper.ExecuteNonQuery(
                @"UPDATE NguoiDung SET HoTen=@h, Email=@e, SDT=@s, DiaChi=@d
                  WHERE MaND=@id",
                DBHelper.NStr("@h", nd.HoTen),
                DBHelper.NStr("@e", nd.Email),
                DBHelper.NStr("@s", nd.SDT),
                DBHelper.NStr("@d", nd.DiaChi),
                DBHelper.Int("@id", nd.MaND));
        }
    }
}
