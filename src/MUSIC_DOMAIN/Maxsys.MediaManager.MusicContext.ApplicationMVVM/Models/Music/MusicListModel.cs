//using CommunityToolkit.Mvvm.ComponentModel;
//using System;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
//{
//    public sealed class MusicListModel : ObservableObject
//    {
//        [Browsable(false)]
//        public Guid MusicId { get; init; }

//        [Browsable(false)]
//        public string MusicFullPath { get; init; }

//        [Display(Name = "MUSIC CATALOG")]
//        public string MusicCatalogName { get; init; }

//        [Display(Name = "ARTIST")]
//        public string ArtistName { get; init; }

//        [Display(Name = "ALBUM")]
//        public string AlbumName { get; init; }

//        [Display(Name = "ALBUM TYPE")]
//        public string AlbumType { get; init; }

//        [Display(Name = "TRACK")]
//        public int? MusicTrackNumber { get; init; }

//        [Display(Name = "TITLE")]
//        public string MusicTitle { get; init; }

//        [Display(Name = "RATING")]
//        public int MusicRating { get; init; }

//        [Display(Name = "VOCAL GENDER")]
//        public string MusicVocalGender { get; init; }

//        [Display(Name = "COVERED ARTIST")]
//        public string MusicCoveredArtist { get; init; }

//        [Display(Name = "IS COVER?")]
//        public bool IsMusicCover { get; init; }

//        [Display(Name = "FEATURED BY")]
//        public string MusicFeaturedArtist { get; init; }
//    }
//}