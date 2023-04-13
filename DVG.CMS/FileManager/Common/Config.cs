using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FileManager.Common
{
    public class Config
    {
        private const string APP_AES_KEY = "AES-Key";
        private const string AES_IV = "AES-IV";
        private const string APP_UPLOAD_DOMAIN = "Upload-Domain";
        private const string APP_VIEW_DOMAIN = "View-Domain";
        private const string APP_UPLOAD_HANDLER = "Upload-Handler";
        private const string APP_UPLOAD_PROJECT = "Upload-Project";
        private const string APP_UPLOAD_API = "Load-File-Api";

        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string AESKey
        {
            get
            {
                string value = GetValue(APP_AES_KEY);

                return value;
            }
        }

        public static string AESIV
        {
            get
            {
                string value = GetValue(AES_IV);

                return value;
            }
        }

        public static string UploadDomain
        {
            get
            {
                string value = GetValue(APP_UPLOAD_DOMAIN);

                return value;
            }
        }

        public static string ViewDomain
        {
            get
            {
                string value = GetValue(APP_VIEW_DOMAIN);

                return value;
            }
        }

        public static string UploadHandler
        {
            get
            {
                string value = GetValue(APP_UPLOAD_HANDLER);

                return value;
            }
        }

        public static string UploadProject
        {
            get
            {
                string value = GetValue(APP_UPLOAD_PROJECT);

                return value;
            }
        }

        public static string LoadFileApi
        {
            get
            {
                string value = GetValue(APP_UPLOAD_API);

                return value;
            }
        }

        public static string FullUploadHandler
        {
            get
            {
                return string.Concat(UploadDomain, UploadHandler);
            }
        }

        public static string FullLoadFileApi
        {
            get
            {
                return string.Concat(UploadDomain, LoadFileApi);
            }
        }
    }
}
