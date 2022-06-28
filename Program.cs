// Intended to be used with http://www.realmofdarkness.net/ and its various domains.

using System.Text.RegularExpressions;
using Newtonsoft.Json;

// 1 argument is required which is the soundboard URL, for example: http://www.realmofdarkness.net/sb/sw-lightsaber/
if(args.Length != 1)
{
    Console.WriteLine("Must include the soundboard URL, for example: http://www.realmofdarkness.net/sb/sw-lightsaber/");
    return;
}

if(!Uri.TryCreate(args[0], UriKind.Absolute, out var url))
{
    Console.WriteLine("URL is not valid.");
    return;
}

using var client = new HttpClient();

// Get the HTML of the page.
var html = await client.GetStringAsync(url);
var urlBuilder = new UriBuilder(url.Scheme, url.Host, url.Port); 

var soundsPath = Regex.Match(html, "<script.*src=\"(?<sounds>.*sounds.js)\".*<\\/script>").Groups["sounds"].Value;
urlBuilder.Path = soundsPath;

var soundsJavaScript = await client.GetStringAsync(urlBuilder.Uri);

// Find the starting [ and use everything up until the ;.
var soundsJson = soundsJavaScript[soundsJavaScript.IndexOf('[')..soundsJavaScript.IndexOf(';')];

// ReSharper disable once AssignNullToNotNullAttribute
var sounds = JsonConvert.DeserializeObject<List<string>>(soundsJson).Select(x => $"{x}.mp3").ToList();

var lastSegement = url.Segments[^1];
var folderPath = $"sounds/{lastSegement}";

// Create folder if it doesn't already exist.
Directory.CreateDirectory(folderPath);

Console.WriteLine($"Downloading {sounds.Count} sound files:");
for(var i = 0; i < sounds.Count; i++)
{
    var fileName = sounds[i];
    Console.Write($"    File #{i + 1}. Downloading {fileName}... ");

    urlBuilder.Path = $"/audio/{lastSegement.Replace('-', '/')}{fileName}";
    var soundData = await client.GetByteArrayAsync(urlBuilder.Uri);
    await File.WriteAllBytesAsync(folderPath + fileName, soundData);

    Console.WriteLine("done.");
}
