using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;

namespace Maxsys.MediaManager.MusicContext.Domain.Extensions;

// TODO apagar?
//public static class SongExtensions
//{
//    public static async Task UpdateSongPropertiesFromAsync(this Song song, ISongPropertiesReader songPropertiesReader, Uri fileToUpdateFrom)
//    {
//        // Media File
//        await song.UpdateFilePropertiesFromAsync(songPropertiesReader, fileToUpdateFrom);

//        // SongProperties
//        var duration = await songPropertiesReader.GetMusicDurationAsync(fileToUpdateFrom);
//        var bitrate = await songPropertiesReader.GetMusicBitrateAsync(fileToUpdateFrom);

//        song.SongProperties.Update(duration, bitrate);
//    }
//}