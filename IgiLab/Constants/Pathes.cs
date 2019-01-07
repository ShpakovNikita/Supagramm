using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IgiLab.Constants
{
    public static class Pathes
    {
        // public static string BASE_DIR = AppDomain.CurrentDomain.BaseDirectory;
        public static string BASE_DIR = @"D:\AppRes\";
        public static string RESOURCES_FOLDER = "resources";

        public static string DEFAULT_AVATAR_FILE = "profile.png";

        public static string RESOURCES_DIR = Path.Combine(BASE_DIR, RESOURCES_FOLDER);
        public static string DEFAULT_AVATAR_DIR = Path.Combine(RESOURCES_DIR, DEFAULT_AVATAR_FILE);

        public static string WAB_STATICFILES_DIR = "/StaticFiles";
    }
}
