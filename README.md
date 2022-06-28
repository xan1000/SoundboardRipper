# SoundboardRipper

A simple .NET 6 program intended to download sound files from http://www.realmofdarkness.net/ and its various domains.

Run the program by supplying a URL which will then have its sound files downloaded. The program will create a folder
for the files within the current working directory called sounds and place all the files within the folder it creates.

For example using the http://www.realmofdarkness.net/sb/sw-lightsaber/ URL a folder called sounds/sw-lightsaber is
created and the downloaded sound files are placed within the sw-lightsaber folder.

# Execute

**NOTE:** The following instructions assumes the .NET 6 runtime has been installed and is on the PATH.

To execute the program use the following command in the terminal:

`dotnet run <url>`

For example:

`dotnet run http://www.realmofdarkness.net/sb/sw-lightsaber/`

Sample output:

```
dotnet run http://www.realmofdarkness.net/sb/sw-lightsaber/
Downloading 22 sound files:
    File #1. Downloading 1-st.mp3... done.
    File #2. Downloading crash-2.mp3... done.
    File #3. Downloading crash-2a.mp3... done.
    File #4. Downloading crash-3.mp3... done.
    File #5. Downloading crash-4.mp3... done.
    File #6. Downloading crash.mp3... done.
    File #7. Downloading d-1.mp3... done.
    File #8. Downloading d-2.mp3... done.
    File #9. Downloading d-3.mp3... done.
    File #10. Downloading d-4.mp3... done.
    File #11. Downloading d-5.mp3... done.
    File #12. Downloading hum.mp3... done.
    File #13. Downloading powerdown-2.mp3... done.
    File #14. Downloading powerdown.mp3... done.
    File #15. Downloading powerup-2.mp3... done.
    File #16. Downloading powerup.mp3... done.
    File #17. Downloading swing-long-2.mp3... done.
    File #18. Downloading swing-long.mp3... done.
    File #19. Downloading swing-med-2.mp3... done.
    File #20. Downloading swing-med.mp3... done.
    File #21. Downloading swing-short-2.mp3... done.
    File #22. Downloading swing-short.mp3... done.
```
