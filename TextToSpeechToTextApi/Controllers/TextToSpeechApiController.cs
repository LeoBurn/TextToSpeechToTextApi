using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TextToSpeechToText.Library.TextToSpeech;
using TextToSpeechToTextApi.Helpers;

namespace TextToSpeechToTextApi.Controllers
{
    public class TextToSpeechController : ApiController
    {

        // GET api/<controller>/5
        public string Get(string text,string languageCode = "pt")
        {
            if (!string.IsNullOrEmpty(text))
            {
                var filePath = CacheHelper.GetMp3Url(text, languageCode);
                return Url.Content($"~/{CacheHelper.Folder}/{Path.GetFileName(filePath)}");
            }
            return String.Empty;
        }
    }
}