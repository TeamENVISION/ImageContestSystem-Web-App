namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    using ImageContestSystem.Common.DropBox;
    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    
    [Authorize]
    public class PicturesController : BaseController
    {
        private const int ImageMinimumBytes = 512;

        public PicturesController(IImageContestSystemData data)
            : base(data)
        {
        }

        public PicturesController(IImageContestSystemData data, User userProfile)
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
                    if (file == null || file.ContentLength == 0 || !IsImage(file))
                    {

                        this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return this.Json(new { responseText = "Picture upload failed!" }, JsonRequestBehavior.AllowGet);
                    }

                    file.InputStream.Position = 0;
                    
                    var path = DropBoxRepository.Upload(file.FileName, this.UserProfile.UserName, file.InputStream);

                    contest.Pictures.Add(new Picture
                    {
                        Url = path,
                        Uploader = this.UserProfile
                    });
                    count++;
                }

                this.ContestData.SaveChanges();

                this.Response.StatusCode = (int)HttpStatusCode.OK;
                return this.Json(new { responseText = "Picture was successfuly uploaded!" }, JsonRequestBehavior.AllowGet);
            }

            this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return this.Json(new { responseText = "Picture upload failed!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int pictureId)
        {
            var picture = this.ContestData.Pictures
                .All()
                .FirstOrDefault(p => p.PictureId == pictureId);
            
            if (picture == null)
            {
                return this.HttpNotFound("You try to delete non existing picture.");
            }
            
            picture.IsDeleted = true;
            picture.IsDeletedFrom = this.UserProfile.Id;
            this.ContestData.SaveChanges();
            
            //return this.RedirectToAction("DetailsById", "Contests", new { contestid = picture.ContestId });
            this.Response.StatusCode = (int)HttpStatusCode.OK;
            return this.Json(new { success = true, responseText = "Picture was successfuly deleted!" }, JsonRequestBehavior.AllowGet);
        }

        private static bool IsImage(HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }

                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}