using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WEBLINH.Models
{
    public static class DonHangDAO
    {
        public static int CreateOrder(DonHang dh, List<CartItem> cart)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(
                            @"INSERT INTO DonHang(MaND, HoTenNhan, SDTNhan, DiaChiGiao, GhiChu, TongTien, TrangThai)
                              VALUES(@nd,@h,@s,@d,@g,@t,@tt);
                              SELECT SCOPE_IDENTITY();", conn, tran);
                        cmd.Parameters.Add(new SqlParameter("@nd", SqlDbType.Int) { Value = dh.MaND });
                        cmd.Parameters.Add(new SqlParameter("@h", SqlDbType.NVarChar, 100) { Value = (object)dh.HoTenNhan ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@s", SqlDbType.NVarChar, 20)  { Value = (object)dh.SDTNhan ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@d", SqlDbType.NVarChar, 300) { Value = (object)dh.DiaChiGiao ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@g", SqlDbType.NVarChar, 500) { Value = (object)dh.GhiChu ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@t", SqlDbType.Decimal) { Value = dh.TongTien });
                        cmd.Parameters.Add(new SqlParameter("@tt", SqlDbType.NVarChar, 30) { Value = "Chờ thanh toán" });
                        int maDH = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (var item in cart)
                        {
                            var cmdCT = new SqlCommand(
                                @"INSERT INTO ChiTietDonHang(MaDH, MaSP, SoLuong, DonGia)
                                  VALUES(@dh,@sp,@sl,@dg)", conn, tran);
                            cmdCT.Parameters.Add(new SqlParameter("@dh", SqlDbType.Int) { Value = maDH });
                            cmdCT.Parameters.Add(new SqlParameter("@sp", SqlDbType.Int) { Value = item.MaSP });
                            cmdCT.Parameters.Add(new SqlParameter("@sl", SqlDbType.Int) { Value = item.SoLuong });
                            cmdCT.Parameters.Add(new SqlParameter("@dg", SqlDbType.Decimal) { Value = item.Gia });
                            cmdCT.ExecuteNonQuery();

                            var cmdTru = new SqlCommand(
                                "UPDATE SanPham SET SoLuong = SoLuong - @sl WHERE MaSP=@sp", conn, tran);
                            cmdTru.Parameters.Add(new SqlParameter("@sl", SqlDbType.Int) { Value = item.SoLuong });
                            cmdTru.Parameters.Add(new SqlParameter("@sp", SqlDbType.Int) { Value = item.MaSP });
                            cmdTru.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return maDH;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        private static DonHang MapHeader(DataRow r)
        {
            return new DonHang
            {
                MaDH = (int)r["MaDH"],
                MaND = (int)r["MaND"],
                TenNguoiDat = r.Table.Columns.Contains("HoTen") ? r["HoTen"].ToString() : "",
                NgayDat = Convert.ToDateTime(r["NgayDat"]),
                HoTenNhan = r["HoTenNhan"].ToString(),
                SDTNhan = r["SDTNhan"].ToString(),
                DiaChiGiao = r["DiaChiGiao"].ToString(),
                GhiChu = r["GhiChu"] == DBNull.Value ? "" : r["GhiChu"].ToString(),
                TongTien = Convert.ToDecimal(r["TongTien"]),
                TrangThai = r["TrangThai"].ToString(),
                AnhBill = r.Table.Columns.Contains("AnhBill") && r["AnhBill"] != DBNull.Value
                            ? r["AnhBill"].ToString() : ""
            };
        }

        public static List<DonHang> GetAll()
        {
            var list = new List<DonHang>();
            var dt = DBHelper.ExecuteQuery(
                @"SELECT dh.*, nd.HoTen
                  FROM DonHang dh INNER JOIN NguoiDung nd ON dh.MaND=nd.MaND
                  ORDER BY dh.MaDH DESC");
            foreach (DataRow r in dt.Rows) list.Add(MapHeader(r));
            return list;
        }

        public static List<DonHang> GetByUser(int maND)
        {
            var list = new List<DonHang>();
            var dt = DBHelper.ExecuteQuery(
                @"SELECT dh.*, nd.HoTen
                  FROM DonHang dh INNER JOIN NguoiDung nd ON dh.MaND=nd.MaND
                  WHERE dh.MaND=@id ORDER BY dh.MaDH DESC",
                DBHelper.Int("@id", maND));
            foreach (DataRow r in dt.Rows) list.Add(MapHeader(r));
            return list;
        }

        public static DonHang GetById(int id)
        {
            var dt = DBHelper.ExecuteQuery(
                @"SELECT dh.*, nd.HoTen
                  FROM DonHang dh INNER JOIN NguoiDung nd ON dh.MaND=nd.MaND
                  WHERE dh.MaDH=@id",
                DBHelper.Int("@id", id));
            if (dt.Rows.Count == 0) return null;
            var dh = MapHeader(dt.Rows[0]);
            dh.ChiTiet = GetChiTiet(id);
            return dh;
        }

        public static List<ChiTietDonHang> GetChiTiet(int maDH)
        {
            var list = new List<ChiTietDonHang>();
            var dt = DBHelper.ExecuteQuery(
                @"SELECT ct.MaDH, ct.MaSP, sp.TenSP, sp.Anh, ct.SoLuong, ct.DonGia
                  FROM ChiTietDonHang ct INNER JOIN SanPham sp ON ct.MaSP=sp.MaSP
                  WHERE ct.MaDH=@id",
                DBHelper.Int("@id", maDH));
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new ChiTietDonHang
                {
                    MaDH = (int)r["MaDH"],
                    MaSP = (int)r["MaSP"],
                    TenSP = r["TenSP"].ToString(),
                    Anh = r["Anh"] == DBNull.Value ? "" : r["Anh"].ToString(),
                    SoLuong = (int)r["SoLuong"],
                    DonGia = Convert.ToDecimal(r["DonGia"])
                });
            }
            return list;
        }

        public static int UpdateTrangThai(int maDH, string trangThai)
        {
            return DBHelper.ExecuteNonQuery(
                "UPDATE DonHang SET TrangThai=@t WHERE MaDH=@id",
                DBHelper.NStr("@t", trangThai),
                DBHelper.Int("@id", maDH));
        }

        public static int UpdateBill(int maDH, string anhBill)
        {
            return DBHelper.ExecuteNonQuery(
                "UPDATE DonHang SET AnhBill=@a, TrangThai=@t WHERE MaDH=@id",
                DBHelper.NStr("@a", anhBill),
                DBHelper.NStr("@t", "Đã chuyển khoản"),
                DBHelper.Int("@id", maDH));
        }

        public static int Delete(int maDH)
        {
            return DBHelper.ExecuteNonQuery(
                "DELETE FROM DonHang WHERE MaDH=@id",
                DBHelper.Int("@id", maDH));
        }

        public static int CountDonHang() =>
            Convert.ToInt32(DBHelper.ExecuteScalar("SELECT COUNT(*) FROM DonHang"));

        public static int CountSanPham() =>
            Convert.ToInt32(DBHelper.ExecuteScalar("SELECT COUNT(*) FROM SanPham"));

        public static int CountNguoiDung() =>
            Convert.ToInt32(DBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM NguoiDung WHERE VaiTro=@v",
                DBHelper.NStr("@v", "User")));

        public static decimal TongDoanhThu() =>
            Convert.ToDecimal(DBHelper.ExecuteScalar(
                "SELECT ISNULL(SUM(TongTien),0) FROM DonHang WHERE TrangThai=@t",
                DBHelper.NStr("@t", "Hoàn thành")));
    }
}
