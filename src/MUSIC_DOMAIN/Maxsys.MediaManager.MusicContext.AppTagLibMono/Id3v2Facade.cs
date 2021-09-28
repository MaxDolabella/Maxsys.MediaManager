using Maxsys.MediaManager.MusicContext.AppTagLibMono.Helpers;
using TagLib;
using TagLib.Id3v2;
using Tag = TagLib.Id3v2.Tag;

namespace TaglibCore
{
    public class Id3v2Facade
    {
        #region Members

        private readonly Tag _tags;
        private const string TRACK_ID_OWNER = "maxsys";

        /// <summary>
        /// Since there might be a lot of people contributing to an audio file in various ways,
        /// such as musicians and technicians, the 'Text information frames' are often insufficient
        /// to list everyone involved in a project. The 'Involved people list' is a frame containing
        /// the names of those involved, and how they were involved. The body simply contains a
        /// terminated string with the involvement directly followed by a terminated string with
        /// the involvee followed by a new involvement and so on. There may only be one "IPLS" frame
        /// in each tag.
        /// </summary>
        private static readonly ReadOnlyByteVector IPLS = "IPLS";

        /// <summary>
        /// The 'Original artist(s)/performer(s)' frame is intended for the performer(s) of the
        /// original recording, if for example the music in the file should be a cover of a
        /// previously released song. The performers are seperated with the "/" character.
        /// </summary>
        private static readonly ReadOnlyByteVector TOPE = "TOPE";

        #endregion Members

        #region Ctor & Factory

        private Id3v2Facade(Tag tags) => _tags = tags;

        public static Id3v2Facade Create(File file, bool create)
        {
            var tags = (Tag)file.GetTag(TagTypes.Id3v2, create);
            tags.Version = 3;

            return new Id3v2Facade(tags);
        }

        #endregion Ctor & Factory

        #region Private Methods

        /// <summary>
        /// Gets the text from a particular UFID frame, referenced by the owner field
        /// </summary>
        /// <param name="owner">String containing the "Owner" data</param>
        /// <returns>String containing the text from the UFID frame, or null</returns>
        private string GetUfidText(string owner)
        {
            //Get the UFID frame, frame will be null if nonexistant
            var frame = UniqueFileIdentifierFrame.Get(_tags, owner, false);

            //If the frame existed: frame.Identifier is a bytevector, get a string
            string result = frame?.Identifier.ToString();
            return string.IsNullOrEmpty(result) ? null : result;
        }

        /// <summary>
		/// Creates and/or sets the text for a UFID frame, referenced by owner
		/// </summary>
		/// <param name="owner">String containing the Owner field</param>
		/// <param name="text">String containing the text to set for the frame</param>
		private void SetUfidText(string owner, string text)
        {
            //Get a UFID frame, create if necessary
            var frame = UniqueFileIdentifierFrame.Get(_tags, owner, true);

            //If we have a real string, convert to ByteVector and apply to frame
            if (!string.IsNullOrEmpty(text))
            {
                var identifier = ByteVector.FromString(text, StringType.UTF8);
                frame.Identifier = identifier;
            }
            else
            {
                //String was null or empty, remove the frame to prevent empties
                _tags.RemoveFrame(frame);
            }
        }

        #endregion Private Methods

        #region Public Methods

        public void SetInfoTag() => _tags.SetInfoTag();

        #endregion Public Methods

        #region Properties

        #region Maxsys Tags

        /// <summary>
        ///    Gets and sets the Maxsys ID
        /// </summary>
        /// <value>
        ///    A <see cref="string" /> containing the Id
        ///    for the media described by the current
        ///    instance, or null if no value is present.
        /// </value>
        /// <remarks>
        ///    This property is implemented using the "UFID:maxsys" frame.
        /// </remarks>
        public string TrackId
        {
            get => GetUfidText(TRACK_ID_OWNER);
            set => SetUfidText(TRACK_ID_OWNER, value);
        }

        /// <summary>
        /// Alias to FeaturedArtist<br/>
		///    Gets and sets the an involved people artists who performed
		///    in the media described by the current instance.
		///    Involved people artists also known as "featured artist"
		/// </summary>
		/// <value>
		///    A <see cref="string" /> containing the involved people artists
		///    who performed in the media described by the current
		///    instance or <see langword="null" /> if  no value is present.
		/// </value>
		/// <remarks>
		///    This property is implemented using the "IPLS" Text
		///    Information Frame.
		/// </remarks>
		public string InvolvedPeople
        {
            get => _tags.GetTextAsString(IPLS);
            set => _tags.SetTextFrame(IPLS, value);
        }

        /// <summary>
        /// Alias to CoveredArtist<br/>
        ///    Gets and sets the original recording performers or artists who performed
        ///    in the media described by the current instance.
        ///    The media should be a cover of a previously released song
        /// </summary>
        /// <value>
        ///    A <see cref="string" /> containing the original performers
        ///    or artists who performed in the media described by the
        ///    current instance or <see langword="null" /> if  no value is present.
        /// </value>
        /// <remarks>
        ///    This property is implemented using the "TOPE" Text
        ///    Information Frame.
        /// </remarks>
        public string OriginalArtist
        {
            get => _tags.GetTextAsString(TOPE);
            set => _tags.SetTextFrame(TOPE, value);
        }

        #endregion Maxsys Tags

        #region Native Tags

        public string Title
        {
            get => _tags.Title;
            set => _tags.Title = value;
        }

        public string Artist
        {
            get => _tags.FirstPerformer;
            set
            {
                var artist = new string[] { value };
                _tags.Performers = artist;
                _tags.AlbumArtists = artist;
            }
        }

        public string Album
        {
            get => _tags.Album;
            set => _tags.Album = value;
        }

        public string Genre
        {
            get => _tags.FirstGenre;
            set => _tags.Genres = new string[] { value };
        }

        public byte Stars10
        {
            get => PopularimeterHelper.GetStars10FromTags(_tags);
            set => PopularimeterHelper.WriteFromStars10(_tags, value);
        }

        public string[] Composers
        {
            get => _tags.Composers;
            set => _tags.Composers = value;
        }

        public byte[] CoverPicture
        {
            get => PictureCoverHelper.GetBytesFromPictures(_tags.Pictures);
            set => _tags.Pictures = PictureCoverHelper.GetPictureFromBytes(value);
        }

        public int? TrackNumber
        {
            get => _tags.Track == 0 ? null : (int?)_tags.Track;
            set
            {
                if (value.HasValue)
                    _tags.Track = (uint)value.Value;
            }
        }

        public int? Year
        {
            get => _tags.Year == 0 ? null : (int?)_tags.Year;
            set
            {
                if (value.HasValue)
                    _tags.Year = (uint)value.Value;
            }
        }

        public string Comments
        {
            get => _tags.Subtitle;
            set
            {
                _tags.Comment = value;
                _tags.Subtitle = value;
            }
        }

        public string Lyrics
        {
            get => _tags.Lyrics;
            set => _tags.Lyrics = value;
        }

        #endregion Native Tags

        #endregion Properties
    }
}