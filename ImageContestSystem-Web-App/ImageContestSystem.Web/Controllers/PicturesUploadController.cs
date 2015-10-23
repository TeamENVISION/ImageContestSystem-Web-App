namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    
    public class PicturesUploadController : BaseController
    {
        private const string PicturePath = "~/Content/PictureFiles";
        
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
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, int contestId)
        {
            int count = 0;
            var contest = this.ContestData.Contest
                           .Find(contestId);

            if (files != null && contest != null)
            {
                foreach (var file in files)
                {
                    if (file == null || file.ContentLength == 0)
                    {
                        return new JsonResult { Data = "Unsuccessfull file(s) upload" };
                    }

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(this.Server.MapPath(PicturePath), fileName);
                    file.SaveAs(path);

                    contest.Pictures.Add(new Picture
                    {
                        Url = "/Content/PictureFiles/" + fileName,
                        Participant = this.UserProfile
                    });
                    count++;
                }

                this.ContestData.SaveChanges();
            }
            
            return new JsonResult { Data = "Successfully " + count + " file(s) uploaded" };
        }
    }
}