using TagLib;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Helpers;

public static class PictureCoverHelper
{
    /*
    TagLib.File tagFile = TagLib.File.Create(mp3FilePath);
    MemoryStream ms = new MemoryStream(tagFile.Tag.Pictures[0].Data.Data);
    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
    */

    public static byte[]? GetBytesFromPictures(IPicture[]? tagLibPictures)
    {
        return (tagLibPictures is null || tagLibPictures.Length == 0)
             ? null
             : tagLibPictures[0].Data.Data;
    }

    public static IPicture[]? GetPictureFromBytes(byte[]? picture)
    {
        if (picture is null)
            return null;

        var taglibVector = new ByteVector(picture);
        var taglibPicture = new Picture(taglibVector) { Type = PictureType.FrontCover };

        return new IPicture[] { taglibPicture };
    }

    public static IPicture[] GetPictureFromFile(string? jpgFilePath)
    {
        return System.IO.File.Exists(jpgFilePath)
            ? new IPicture[] { new Picture(jpgFilePath) }
            : null;
    }
}