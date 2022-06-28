using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SoundboardRipper;

public class Soundboard
{
    private readonly Uri _url;
    private readonly HttpClient _client;
    private readonly UriBuilder _urlBuilder;

    public Soundboard(Uri url, HttpClient client)
    {
        _url = url;
        _client = client;
        _urlBuilder = new UriBuilder(url.Scheme, url.Host, url.Port);
    }

    public string GetFolderPath() => $"sounds/{_url.Segments[^1]}";

    public string GetFileName(Uri soundFileUrl) => soundFileUrl.Segments[^1];
    
    public async Task<Uri[]> GetSoundFileUrlsAsync()
    {
        // Get the HTML of the page.
        var html = await _client.GetStringAsync(_url);

        var soundboard = await GetSoundboardFolderPathAndFileExtensionAsync(html);
        var soundFileNames = await GetSoundFileNamesAsync(html);

        return soundFileNames.Select(fileName =>
        {
            _urlBuilder.Path = soundboard.folderPath + fileName + soundboard.fileExtension;

            return _urlBuilder.Uri;
        }).ToArray();
    }

    private async Task<string[]> GetSoundFileNamesAsync(string html)
    {
        // Retrieve the sound file names from the sounds.js file.
        _urlBuilder.Path =
            Regex.Match(html, "<script.*src=\"(?<sounds>.*sounds.js)\".*<\\/script>").Groups["sounds"].Value;

        var soundsJavaScript = await _client.GetStringAsync(_urlBuilder.Uri);

        // Find the starting [ and use everything up until the ;.
        var soundsJson = soundsJavaScript[soundsJavaScript.IndexOf('[')..soundsJavaScript.IndexOf(';')];

        return JsonConvert.DeserializeObject<string[]>(soundsJson);
    }

    private async Task<(string folderPath, string fileExtension)> GetSoundboardFolderPathAndFileExtensionAsync(
        string html)
    {
        // Retrieve the soundboard sb.js file to extract the folder path and file extension to build the URL for the
        // sound files.
        _urlBuilder.Path =
            Regex.Match(html, "<script.*src=\"(?<soundboard>.*sb.js)\".*<\\/script>").Groups["soundboard"].Value;

        var soundboardJavaScript = await _client.GetStringAsync(_urlBuilder.Uri);

        // Extract the folder path and file extension from the javascript.
        var match = Regex.Match(soundboardJavaScript, "src:.*\\[\"(?<folderPath>.*)\".*\"(?<fileExtension>.*)\"]");

        return (match.Groups["folderPath"].Value, match.Groups["fileExtension"].Value);
    }
}
