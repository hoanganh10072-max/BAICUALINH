using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WEBLINH.Models
{
    public static class SanPhamDAO
    {
        private const string SELECT_COLS = @"
            SELECT sp.MaSP, sp.TenSP, sp.MaDM, dm.TenDM, sp.Gia, sp.SoLuong,
                   sp.MoTa, sp.Anh, sp.NgayTao, sp.NoiBat
            FROM SanPham sp
            INNER JOIN DanhMuc dm ON sp.MaDM = dm.MaDM";

        private static SanPham Map(DataRow r)
        {
            return new SanPham
            {
                MaSP = (int)r["MaSP"],
                TenSP = r["TenSP"].ToString(),
                MaDM = (int)r["MaDM"],
                TenDM = r["TenDM"].ToString(),
                Gia = Convert.ToDecimal(r["Gia"]),
                SoLuong = Convert.ToInt32(r["SoLuong"]),
                MoTa = r["MoTa"] == DBNull.Value ? "" : r["MoTa"].ToString(),
                Anh = r["Anh"] == DBNull.Value ? "" : r["Anh"].ToString(),
                NgayTao = Convert.ToDateTime(r["NgayTao"]),
                NoiBat = Convert.ToBoolean(r["NoiBat"])
            };
        }

        public static List<SanPham> GetAll()
        {
            var list = new List<SanPham>();
            var dt = DBHelper.ExecuteQuery(SELECT_COLS + " ORDER BY sp.MaSP DESC");
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public static SanPham GetById(int id)
        {
            var dt = DBHelper.ExecuteQuery(SELECT_COLS + " WHERE sp.MaSP=@id",
                DBHelper.Int("@id", id));
            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public static List<SanPham> GetByDanhMuc(int maDM)
        {
            var list = new List<SanPham>();
            var dt = DBHelper.ExecuteQuery(SELECT_COLS + " WHERE sp.MaDM=@id ORDER BY sp.MaSP DESC",
                DBHelper.Int("@id", maDM));
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public static List<SanPham> GetNoiBat(int top = 8)
        {
            var list = new List<SanPham>();
            var dt = DBHelper.ExecuteQuery(
                "SELECT TOP (" + top + ") sp.MaSP, sp.TenSP, sp.MaDM, dm.TenDM, sp.Gia, sp.SoLuong, sp.MoTa, sp.Anh, sp.NgayTao, sp.NoiBat " +
                "FROM SanPham sp INNER JOIN DanhMuc dm ON sp.MaDM=dm.MaDM " +
                "WHERE sp.NoiBat=1 ORDER BY sp.MaSP DESC");
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public static List<SanPham> GetMoiNhat(int top = 8)
        {
            var list = new List<SanPham>();
            var dt = DBHelper.ExecuteQuery(
                "SELECT TOP (" + top + ") sp.MaSP, sp.TenSP, sp.MaDM, dm.TenDM, sp.Gia, sp.SoLuong, sp.MoTa, sp.Anh, sp.NgayTao, sp.NoiBat " +
                "FROM SanPham sp INNER JOIN DanhMuc dm ON sp.MaDM=dm.MaDM " +
                "ORDER BY sp.NgayTao DESC");
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public static List<SanPham> Search(string keyword)
        {
            var list = new List<SanPham>();
            var dt = DBHelper.ExecuteQuery(
                SELECT_COLS + " WHERE sp.TenSP LIKE @kw OR sp.MoTa LIKE @kw ORDER BY sp.MaSP DESC",
                DBHelper.NStr("@kw", "%" + (keyword ?? "") + "%"));
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public static int Insert(SanPham sp)
        {
            return DBHelper.ExecuteNonQuery(
                @"INSERT INTO SanPham(TenSP, MaDM, Gia, SoLuong, MoTa, Anh, NoiBat)
                  VALUES(@t,@dm,@g,@sl,@mt,@a,@nb)",
                DBHelper.NStr("@t", sp.TenSP),
                DBHelper.Int("@dm", sp.MaDM),
                DBHelper.Decimal("@g", sp.Gia),
                DBHelper.Int("@sl", sp.SoLuong),
                DBHelper.NText("@mt", sp.MoTa),
                DBHelper.NStr("@a", sp.Anh),
                DBHelper.Bit("@nb", sp.NoiBat));
        }

        public static int Update(SanPham sp)
        {
            return DBHelper.ExecuteNonQuery(
                @"UPDATE SanPham SET TenSP=@t, MaDM=@dm, Gia=@g, SoLuong=@sl,
                                     MoTa=@mt, Anh=@a, NoiBat=@nb
                  WHERE MaSP=@id",
                DBHelper.NStr("@t", sp.TenSP),
                DBHelper.Int("@dm", sp.MaDM),
                DBHelper.Decimal("@g", sp.Gia),
                DBHelper.Int("@sl", sp.SoLuong),
                DBHelper.NText("@mt", sp.MoTa),
                DBHelper.NStr("@a", sp.Anh),
                DBHelper.Bit("@nb", sp.NoiBat),
                DBHelper.Int("@id", sp.MaSP));
        }

        public static int Delete(int id)
        {
            return DBHelper.ExecuteNonQuery("DELETE FROM SanPham WHERE MaSP=@id",
                DBHelper.Int("@id", id));
        }

        public static int TruSoLuong(int maSP, int soLuong)
        {
            return DBHelper.ExecuteNonQuery(
                "UPDATE SanPham SET SoLuong = SoLuong - @sl WHERE MaSP=@id AND SoLuong >= @sl",
                DBHelper.Int("@sl", soLuong),
                DBHelper.Int("@id", maSP));
        }
    }
}
