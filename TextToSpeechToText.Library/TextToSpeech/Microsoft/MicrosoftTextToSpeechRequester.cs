using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using TextToSpeechToText.Library.TextToSpeech.Microsoft.CognitiveServicesTTS;

namespace TextToSpeechToText.Library.TextToSpeech.Microsoft
{
    public class MicrosoftTextToSpeechRequester : ITextToSpeechRequester
    {
        public const string Key = "00292cbee440481cb9f6fb4de1eb9451";
        public const string Url = "https://speech.platform.bing.com/synthesize";
        public string[] Languages = { "pt-PT", "en-US", "es-ES" };
        public Dictionary<string, string> Voices; 

        public MicrosoftTextToSpeechRequester()
        {
            Voices = new Dictionary<string, string>
            {
                {"pt-PT", "Microsoft Server Speech Text to Speech Voice (pt-PT, HeliaRUS)"},
                {"es-ES", "Microsoft Server Speech Text to Speech Voice (es-ES, HelenaRUS)"},
                {"en-US", "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)"}
            };
        }

        public Stream StreamFile { get; set; }

        public Stream Request(string text, string languageCode)
        {
            Authentication auth = new Authentication(Key);
            string accessToken = auth.GetAccessToken();

            try
            {
                accessToken = auth.GetAccessToken();
            }
            catch (Exception ex)
            {
            //Implement
            }

            var cortana = new Synthesize();

            cortana.OnAudioAvailable += PlayAudio;
            var languageCodeIso = GetLanguageCode(languageCode);
            var voiceName = Voices[languageCodeIso];

            // Reuse Synthesize object to minimize latency
            cortana.Speak(CancellationToken.None, new Synthesize.InputOptions()
            {
                RequestUri = new Uri(Url),
                // Text to be spoken.
                Text = text,
                VoiceType = Gender.Female,
                // Refer to the documentation for complete list of supported locales.
                Locale = languageCodeIso,
                // You can also customize the output voice. Refer to the documentation to view the different
                // voices that the TTS service can output.
                VoiceName = voiceName,
                // Service can return audio in different output format.
                OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm,
                AuthorizationToken = "Bearer " + accessToken,
            }).Wait();
            return StreamFile;
        }

        private void PlayAudio(object sender, GenericEventArgs<Stream> args)
        {
            StreamFile = (Stream) args.EventData;
        }

        private string GetLanguageCode(string languageCode)
        {
            return Languages.FirstOrDefault(p => p.ToLower().Contains(languageCode));
        }
    }
}
