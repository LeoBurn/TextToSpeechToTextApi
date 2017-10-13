using System.IO;
using System.Net;

namespace TextToSpeechToText.Library.TextToSpeech.Google
{
    public class GoogleTextToSpeechRequester : ITextToSpeechRequester
    {
        public string UrlTemplate =
            "http://translate.google.com/translate_tts?ie=UTF-8&q={0}&tl={1}&client=tw-ob";


        public Stream Request(string text, string languageCode)
        {
            var url = string.Format(UrlTemplate, text, languageCode);
            HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Referer = "http://translate.google.com/";
            webRequest.UserAgent = "stagefright/1.2 (Linux;Android 5.0)";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            return receiveStream;
        }
    }
}
