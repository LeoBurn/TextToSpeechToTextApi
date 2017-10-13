using System.IO;

namespace TextToSpeechToText.Library.TextToSpeech
{
    public interface ITextToSpeechRequester
    {
        Stream Request(string text, string languageCode);
    }
}
