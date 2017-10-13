using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Microsoft.SqlServer.Server;
using TextToSpeechToText.Library.TextToSpeech;
using TextToSpeechToText.Library.TextToSpeech.Google;
using TextToSpeechToText.Library.TextToSpeech.Microsoft;

namespace TextToSpeechToTextApi.Helpers
{
    public class CacheHelper
    {
        public const string Folder = "MediaArchive";

        private static string IsFileCached(string fileName)
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath($"~/{Folder}/{fileName}.mp3");
            return File.Exists(path) ? path : string.Empty;
        }

        public static string GetMp3Url(string text, string languageCode)
        {
            var fileName = $"{text}{languageCode}".ToLower();
            var fileNameHashed = Utils.CalculateMD5Hash(fileName);
            var filePath = IsFileCached(fileNameHashed);
            if (!string.IsNullOrEmpty(filePath))
                return filePath;
            ITextToSpeechRequester requester = new MicrosoftTextToSpeechRequester();
            var stream = requester.Request(text, languageCode);
            filePath = SaveFile(stream, fileNameHashed);
            return filePath;

        }

        private static string SaveFile(Stream stream, string fileName)
        {
            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath($"~/{Folder}/{fileName}.mp3");
            byte[] buffer = new byte[32768];
            using (FileStream fileStream = File.Create(fullPath))
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        break;
                    fileStream.Write(buffer, 0, read);
                }
            }
            return fullPath;
        }
    }
}