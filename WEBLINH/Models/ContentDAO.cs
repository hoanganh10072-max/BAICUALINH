using System;
using System.Collections.Generic;
using System.Data;

namespace WEBLINH.Models
{
    public static class ContentDAO
    {
        public static HomeBanner GetBanner()
        {
            var r = DBHelper.ExecuteQuery("SELECT * FROM HomeBanner WHERE Id=1").Rows[0];
            return new HomeBanner { Eyebrow = r["Eyebrow"].ToString(), Title = r["Title"].ToString(), Highlight = r["Highlight"].ToString(), Description = r["Description"].ToString(), ImageName = r["ImageName"].ToString(), ImageAlt = r["ImageAlt"].ToString(), ButtonText = r["ButtonText"].ToString(), ButtonUrl = r["ButtonUrl"].ToString(), Visible = (bool)r["Visible"] };
        }

        public static void SaveBanner(HomeBanner b)
        {
            DBHelper.ExecuteNonQuery("UPDATE HomeBanner SET Eyebrow=@e,Title=@t,Highlight=@h,Description=@d,ImageName=@i,ImageAlt=@a,ButtonText=@bt,ButtonUrl=@bu,Visible=@v WHERE Id=1",
                DBHelper.NStr("@e", b.Eyebrow), DBHelper.NStr("@t", b.Title), DBHelper.NStr("@h", b.Highlight), DBHelper.NStr("@d", b.Description), DBHelper.NStr("@i", b.ImageName), DBHelper.NStr("@a", b.ImageAlt), DBHelper.NStr("@bt", b.ButtonText), DBHelper.NStr("@bu", b.ButtonUrl), DBHelper.Bit("@v", b.Visible));
        }

        private static BaiViet Read(DataRow r)
        {
            return new BaiViet { Id = (int)r["Id"], TieuDe = r["TieuDe"].ToString(), TomTat = r["TomTat"].ToString(), NoiDung = r["NoiDung"].ToString(), Anh = r["Anh"].ToString(), DaDang = (bool)r["DaDang"], NgayTao = (DateTime)r["NgayTao"], NgayDang = r.IsNull("NgayDang") ? (DateTime?)null : (DateTime)r["NgayDang"] };
        }

        public static List<BaiViet> GetPosts(bool publishedOnly = false, int limit = 1000)
        {
            var rows = DBHelper.ExecuteQuery("SELECT TOP (@limit) * FROM BaiViet" + (publishedOnly ? " WHERE DaDang=1" : "") + " ORDER BY COALESCE(NgayDang,NgayTao) DESC,Id DESC", DBHelper.Int("@limit", limit));
            var list = new List<BaiViet>();
            foreach (DataRow r in rows.Rows) list.Add(Read(r));
            return list;
        }

        public static BaiViet GetPost(int id, bool publishedOnly = false)
        {
            var rows = DBHelper.ExecuteQuery("SELECT * FROM BaiViet WHERE Id=@id" + (publishedOnly ? " AND DaDang=1" : ""), DBHelper.Int("@id", id));
            return rows.Rows.Count == 0 ? null : Read(rows.Rows[0]);
        }

        public static void SavePost(BaiViet b)
        {
            var sql = b.Id == 0
                ? "INSERT BaiViet(TieuDe,TomTat,NoiDung,Anh,DaDang,NgayDang) VALUES(@t,@s,@n,@a,@p,CASE WHEN @p=1 THEN GETDATE() ELSE NULL END)"
                : "UPDATE BaiViet SET TieuDe=@t,TomTat=@s,NoiDung=@n,Anh=@a,DaDang=@p,NgayDang=CASE WHEN @p=1 THEN COALESCE(NgayDang,GETDATE()) ELSE NgayDang END WHERE Id=@id";
            DBHelper.ExecuteNonQuery(sql, DBHelper.NStr("@t", b.TieuDe), DBHelper.NStr("@s", b.TomTat), DBHelper.NText("@n", b.NoiDung), DBHelper.NStr("@a", b.Anh), DBHelper.Bit("@p", b.DaDang), DBHelper.Int("@id", b.Id));
        }

        public static void DeletePost(int id)
        {
            DBHelper.ExecuteNonQuery("DELETE BaiViet WHERE Id=@id", DBHelper.Int("@id", id));
        }
    }
}
