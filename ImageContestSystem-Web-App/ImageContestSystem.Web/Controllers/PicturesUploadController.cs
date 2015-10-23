namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    
    public class PicturesUploadController : BaseController
    {
        public const int ImageMinimumBytes = 512;
        
        public PicturesUploadController(IImageContestSystemData data)
            : base(data)
        {
        }

        public PicturesUploadController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            int count = 0;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                        file.SaveAs(path);
                        count++;
                    }
                }
            }
            
            return new JsonResult { Data = "Successfully " + count + " file(s) uploaded" };
        }
    }
}