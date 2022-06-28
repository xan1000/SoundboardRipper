// Intended to be used with http://www.realmofdarkness.net/ and its various domains.
using SoundboardRipper;

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

var soundboard = new Soundboard(url, client);

var soundFileUrls = await soundboard.GetSoundFileUrlsAsync();
var folderPath = soundboard.GetFolderPath();

// Create folder if it doesn't already exist.
Directory.CreateDirectory(folderPath);

Console.WriteLine($"Downloading {soundFileUrls.Length} sound files:");
for(var i = 0; i < soundFileUrls.Length; i++)
{
    var fileUrl = soundFileUrls[i];
    var fileName = soundboard.GetFileName(fileUrl);
    Console.Write($"    #{i + 1}. {fileName} ... ");

    var soundData = await client.GetByteArrayAsync(fileUrl);
    await File.WriteAllBytesAsync(Path.Join(folderPath + fileName), soundData);

    Console.WriteLine("done.");
}
