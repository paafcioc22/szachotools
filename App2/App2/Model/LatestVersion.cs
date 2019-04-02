//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace App2.Model
//{
//    public class LatestVersion
//    {
//        public async Task<string> GetLatestVersionNumber(string appName)
//        {
//            if (string.IsNullOrWhiteSpace(appName))
//            {
//                throw new ArgumentNullException(nameof(appName));
//            }

//            var version = string.Empty;
//            var url = $"https://play.google.com/store/apps/details?id={appName}&hl=en";

//            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
//            {
//                using (var handler = new HttpClientHandler())
//                {
//                    using (var client = new HttpClient(handler))
//                    {
//                        using (var responseMsg = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
//                        {
//                            if (!responseMsg.IsSuccessStatusCode)
//                            {
//                                return ($"Error connecting to the Play Store. Url={url}.");
//                            }

//                            try
//                            {
//                                var content = responseMsg.Content == null ? null : await responseMsg.Content.ReadAsStringAsync();

//                                var versionMatch = Regex.Match(content, "<div[^>]*>Current Version</div><span[^>]*><div[^>]*><span[^>]*>(.*?)<").Groups[1];

//                                if (versionMatch.Success)
//                                {
//                                    version = versionMatch.Value.Trim();
//                                }
//                            }
//                            catch (Exception e)
//                            {
//                                return ($"Error parsing content from the Play Store. Url={url}.");
//                            }
//                        }
//                    }
//                }
//            }

//            return version;
//        }
         

//    }
//}
