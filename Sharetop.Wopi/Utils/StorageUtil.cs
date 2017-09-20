using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Sharetop.Wopi.Utils
{
    public static class StorageUtil
    {
        public static byte[] GetFile(string fileName, string container)
        {
            var filePath = string.Format("{0}\\{1}\\{2}", ServerUtil.StoragePath(), container, fileName);
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch(Exception ex)
            {
                ServerUtil.LogException(ex);
                throw ex;
            }
        }

        public static string Save(string fileName, string container, byte[] fileBytes)
        {
            var filePath = string.Format("{0}\\{1}\\{2}", ServerUtil.StoragePath(), container, fileName);
            if (File.Exists(filePath)) File.Delete(filePath);

            File.WriteAllBytes(filePath, fileBytes);

            return string.Format(".//{0}\\{1}", container, fileName);
        }

        public static bool Delete(string fileName, string container)
        {
            var filePath = string.Format("{0}\\{1}\\{2}", ServerUtil.StoragePath(), container, fileName);
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception e)
            {
                ServerUtil.LogException(e);
                return false;
            }
        }
    }
}