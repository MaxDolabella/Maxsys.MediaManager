using System;
using TagLib;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Helpers
{
    public static class PictureCoverHelper
    {
        /*
        TagLib.File tagFile = TagLib.File.Create(mp3FilePath);
        MemoryStream ms = new MemoryStream(tagFile.Tag.Pictures[0].Data.Data);
        System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
        */

        public static byte[] GetBytesFromPictures(IPicture[] tagLibPictures)
        {
            return tagLibPictures.Length > 0
                ? tagLibPictures[0].Data.Data
                : Array.Empty<byte>();
        }

        public static IPicture[] GetPictureFromFile(string jpgFilePath)
        {
            var taglibPictureFromFile = new Picture(jpgFilePath);

            return new IPicture[] { taglibPictureFromFile };
        }

        public static IPicture[] GetPictureFromBytes(byte[] picture)
        {
            if (picture is null) picture = new byte[] { };
            var taglibVector = new ByteVector(picture);
            var taglibPicture = new Picture(taglibVector) { Type = PictureType.FrontCover };

            return new IPicture[] { taglibPicture };
        }
    }
}