using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;

namespace Maxsys.MediaManager.MusicContext.Domain.Extensions;

// TODO apagar?
public static class SongExtensions
{
    public static void UpdateSongPropertiesFrom(this Song song, ISongPropertiesReader songPropertiesReader, string fileToUpdateFrom)
    {
        // Media File
        song.UpdateFilePropertiesFrom(songPropertiesReader, fileToUpdateFrom);

        // SongProperties
        var duration = songPropertiesReader.GetMusicDuration(fileToUpdateFrom);
        var bitrate = songPropertiesReader.GetMusicBitrate(fileToUpdateFrom);

        var newSongProps = new SongProperties(duration, bitrate);

        song.SongProperties.Update(newSongProps!);
    }
}