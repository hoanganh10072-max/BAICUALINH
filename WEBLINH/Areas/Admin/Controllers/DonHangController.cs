using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class DonHangController : Controller
    {
        public ActionResult Index()
        {
            return View(DonHangDAO.GetAll());
        }

        public ActionResult Detail(int id)
        {
            var dh = DonHangDAO.GetById(id);
            if (dh == null) return HttpNotFound();
            return View(dh);
        }

        [HttpPost]
        public ActionResult CapNhatTrangThai(int id, string trangThai)
        {
            DonHangDAO.UpdateTrangThai(id, trangThai);
            TempData["Msg"] = "Đã cập nhật trạng thái đơn hàng!";
            return RedirectToAction("Detail", new { id = id });
        }

        public ActionResult Delete(int id)
        {
            DonHangDAO.Delete(id);
            TempData["Msg"] = "Đã xóa đơn hàng.";
            return RedirectToAction("Index");
        }
    }
}
