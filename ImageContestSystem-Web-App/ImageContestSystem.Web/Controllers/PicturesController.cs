﻿namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    
    [Authorize]
    public class PicturesController : BaseController
    {
        private const string PicturePath = "/Content/PictureFiles/";
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
                        return new JsonResult { Data = "error" };
                    }

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(this.Server.MapPath("~" + PicturePath), fileName);
                    file.SaveAs(path);

                    contest.Pictures.Add(new Picture
                    {
                        Url = PicturePath + fileName,
                        Uploader = this.UserProfile
                    });
                    count++;
                }

                this.ContestData.SaveChanges();

                return new JsonResult { Data = "Successfully " + count + " file(s) uploaded" };
            }

            return new JsonResult { Data = "error" };
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