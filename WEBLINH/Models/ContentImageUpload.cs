using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace WEBLINH.Models
{
    public static class ContentImageUpload
    {
        public static string Save(HttpPostedFileBase file, string directory)
        {
            if (file == null || file.ContentLength == 0) return null;
            if (file.ContentLength > 8 * 1024 * 1024) throw new ArgumentException("Ảnh tối đa 8 MB.");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png") throw new ArgumentException("Chỉ nhận ảnh JPG hoặc PNG.");
            using (var stream = file.InputStream)
            {
                try
                {
                    using (var img = Image.FromStream(stream, true, true))
                    {
                        if (img.Width > 8000 || img.Height > 8000 || (long)img.Width * img.Height > 32000000) throw new ArgumentException("Ảnh quá lớn, vui lòng dùng ảnh tối đa 32 megapixel.");
                        Directory.CreateDirectory(directory);
                        var name = "content_" + Guid.NewGuid().ToString("N") + (ext == ".png" ? ".png" : ".jpg");
                        // Re-encode instead of trusting the uploaded filename or embedded data.
                        using (var clean = new Bitmap(img)) clean.Save(Path.Combine(directory, name), ext == ".png" ? ImageFormat.Png : ImageFormat.Jpeg);
                        return name;
                    }
                }
                catch (ArgumentException) { throw new ArgumentException("Ảnh không hợp lệ hoặc quá lớn. Chọn ảnh JPG/PNG khác (tối đa 8 MB, 32 megapixel)."); }
            }
        }
    }
}
