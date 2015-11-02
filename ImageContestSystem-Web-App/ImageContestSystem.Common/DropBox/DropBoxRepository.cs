namespace ImageContestSystem.Common.DropBox
{
    using System;
    using System.IO;

    using DropNet;

    public static class DropBoxRepository
    {
        private const string AppKey = "l8pz0wymirv9rq1";

        private const string AppAccessToken = "6LdBohjMixQAAAAAAAAADmZ22D3lJxTr-4ozygoaOWihM3PJWRQJxDBSOSd_ajkj";

        private const string AppSecret = "o6lm7kthdfzb6ru";

        static DropBoxRepository()
        {
            Client = new DropNetClient(AppKey, AppSecret, AppAccessToken) { UseSandbox = true };
        }

        private static DropNetClient Client { get; set; }

        public static string Upload(string fileName, string author, Stream fileStream)
        {
            var fullFileName = string.Format("{0}_{1}_{2}", author, DateTime.Now.ToString("o"), fileName);
            Client.UploadFile("/", fullFileName, fileStream);

            return fullFileName;
        }

        public static string Download(string path)
        {
            var result = Client.GetMedia(path).Url;

            return result;
        }
    }
}