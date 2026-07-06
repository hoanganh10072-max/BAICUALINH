using System.Web.Mvc;
using WEBLINH.Models;

namespace WEBLINH.Areas.User.Controllers
{
    public class SanPhamController : Controller
    {
        public ActionResult DanhSach(int? maDM)
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.MaDMHienTai = maDM;
            var list = maDM.HasValue ? SanPhamDAO.GetByDanhMuc(maDM.Value) : SanPhamDAO.GetAll();
            if (maDM.HasValue)
            {
                var dm = DanhMucDAO.GetById(maDM.Value);
                ViewBag.TenDM = dm != null ? dm.TenDM : "";
            }
            return View(list);
        }

        public ActionResult ChiTiet(int id)
        {
            var sp = SanPhamDAO.GetById(id);
            if (sp == null) return HttpNotFound();
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.LienQuan = SanPhamDAO.GetByDanhMuc(sp.MaDM);
            return View(sp);
        }

        public ActionResult TimKiem(string kw)
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.Keyword = kw;
            var list = SanPhamDAO.Search(kw);
            return View(list);
        }
    }
}
