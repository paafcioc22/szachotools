using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace App2.Model
{
    public static class FilesHelper
    {
        public static async Task<string> SaveToCacheAsync(Stream data, string fileName)
        {
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            await using var stream = File.Create(filePath);
            await data.CopyToAsync(stream);
            stream.Close();

            return filePath;
        }

        public static string ConvertUrlToOtherSize(string url, string twrkod , OtherSize otherSize )
        {
            string rtrnUrl = "";
            if (!string.IsNullOrEmpty(url))
            {
                if (url.Contains("http://szachownica.com"))
                {
                    rtrnUrl = url.Replace("large", otherSize.ToString());
                }
                else
                {
                    //replace(twr_url,replace(twr_kod,'/','-')+'.JPG','')+'Miniatury/'+replace(twr_kod,'/','-')+'.JPG' as Twr_Url_Small
                    rtrnUrl = url.Replace(twrkod.Replace('/', '-') + ".JPG", "") + string.Concat("Miniatury/",twrkod.Replace('/', '-') ,".JPG");
                }
            }

            return rtrnUrl;
        }

        public enum OtherSize
        {
            large,
            home,
            small
        }
    }
}
