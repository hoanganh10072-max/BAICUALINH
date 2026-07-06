using System.Collections.Generic;
using System.Data;

namespace WEBLINH.Models
{
    public static class MenuDAO
    {
        private static Menu FromRow(DataRow r)
        {
            return new Menu
            {
                MaMenu = (int)r["MaMenu"],
                TenMenu = r["TenMenu"].ToString(),
                Url = r["Url"].ToString(),
                ThuTu = (int)r["ThuTu"],
                HienThi = (bool)r["HienThi"]
            };
        }

        public static List<Menu> GetAll()
        {
            var list = new List<Menu>();
            var dt = DBHelper.ExecuteQuery(
                "SELECT MaMenu, TenMenu, Url, ThuTu, HienThi FROM Menu ORDER BY ThuTu, MaMenu");
            foreach (DataRow r in dt.Rows) list.Add(FromRow(r));
            return list;
        }

        public static List<Menu> GetVisible()
        {
            var list = new List<Menu>();
            var dt = DBHelper.ExecuteQuery(
                "SELECT MaMenu, TenMenu, Url, ThuTu, HienThi FROM Menu WHERE HienThi=1 ORDER BY ThuTu, MaMenu");
            foreach (DataRow r in dt.Rows) list.Add(FromRow(r));
            return list;
        }

        public static Menu GetById(int id)
        {
            var dt = DBHelper.ExecuteQuery(
                "SELECT MaMenu, TenMenu, Url, ThuTu, HienThi FROM Menu WHERE MaMenu=@id",
                DBHelper.Int("@id", id));
            if (dt.Rows.Count == 0) return null;
            return FromRow(dt.Rows[0]);
        }

        public static int Insert(Menu m)
        {
            return DBHelper.ExecuteNonQuery(
                "INSERT INTO Menu(TenMenu, Url, ThuTu, HienThi) VALUES(@t, @u, @tt, @h)",
                DBHelper.NStr("@t", m.TenMenu),
                DBHelper.NStr("@u", m.Url),
                DBHelper.Int("@tt", m.ThuTu),
                DBHelper.Bit("@h", m.HienThi));
        }

        public static int Update(Menu m)
        {
            return DBHelper.ExecuteNonQuery(
                "UPDATE Menu SET TenMenu=@t, Url=@u, ThuTu=@tt, HienThi=@h WHERE MaMenu=@id",
                DBHelper.NStr("@t", m.TenMenu),
                DBHelper.NStr("@u", m.Url),
                DBHelper.Int("@tt", m.ThuTu),
                DBHelper.Bit("@h", m.HienThi),
                DBHelper.Int("@id", m.MaMenu));
        }

        public static int Delete(int id)
        {
            return DBHelper.ExecuteNonQuery(
                "DELETE FROM Menu WHERE MaMenu=@id",
                DBHelper.Int("@id", id));
        }
    }
}
