//using FluentValidation;
//using FluentValidation.TestHelper;
//using Maxsys.MediaManager.CoreDomain.Services;
//using Maxsys.MediaManager.MusicContext.AppTagLib.Services;
//using Maxsys.MediaManager.MusicContext.Domain.Entities;
//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services.Mp3;
//using Maxsys.MediaManager.MusicContext.Domain.Validation.MusicValidators;
//using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
//using Maxsys.MediaManager.Tests.MockMusicContext;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.IO;
//using System.Linq;

//namespace Maxsys.MediaManager.Tests.EntitiesTests
//{
//    [TestClass]
//    public class MusicTest
//    {
//        #region Fields
//        const string CATEGORY = "Domain.Entity: " + nameof(Music);
//        static readonly string _sampleFileMp3 = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "SampleFile.mp3");
//        static readonly string _uniqueFileMp3 = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "UniqueFile.mp3");
//        static readonly string _0Duration64BitrateMp3 = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "ZeroDuration64BitrateFile.mp3");
//        static readonly IMusicRepository _repository = new MusicMockRepository();
//        static readonly IValidator<Music> _businessValidator = new MusicBusinessValidator();
//        static readonly IValidator<Music> _persistenceValidator = new MusicPersistenceValidator(_repository);
//        IMusicPropertiesReader _propsReader = new TagLibMusicPropertiesReader(new FilePropertiesReader());
//        #endregion

//        #region Business
//        [TestMethod] [TestCategory(CATEGORY)]
//        public void Path_cannot_be_null_or_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightMusic = MusicFactory.Create(_sampleFileMp3, _sampleFileMp3, "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongMusicN = MusicFactory.Create(_sampleFileMp3, null, "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongMusicE = MusicFactory.Create(_sampleFileMp3, " ", "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight = validator.TestValidate(rightMusic);
//            var resultWrongN = validator.TestValidate(wrongMusicN);
//            var resultWrongE = validator.TestValidate(wrongMusicE);


//            // Assert
//            resultRight.ShouldNotHaveValidationErrorFor(x => x.FullPath);
//            resultWrongN.ShouldHaveValidationErrorFor(x => x.FullPath);
//            resultWrongE.ShouldHaveValidationErrorFor(x => x.FullPath);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void Path_must_have_260chars_max_length()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightMusic = MusicFactory.Create(_sampleFileMp3, $@"D:\{StringHelper.GetWord(260 - 7)}.mp3", "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongMusic = MusicFactory.Create(_sampleFileMp3, $@"D:\{StringHelper.GetWord(261 - 7)}.mp3", "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var rightErrorMessages = validator.Validate(rightMusic).Errors.Select(e => e.ErrorMessage);
//            var wrongErrorMessages = validator.Validate(wrongMusic).Errors.Select(e => e.ErrorMessage);
//            var expectedErrorMessage = "Path lenght must be lower than 260.";


//            // Act
//            var right = rightErrorMessages.Contains(expectedErrorMessage);
//            var wrong = wrongErrorMessages.Contains(expectedErrorMessage);


//            // Assert
//            Assert.IsFalse(right);
//            Assert.IsTrue(wrong);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void Title_cannot_be_null_or_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightMusic = MusicFactory.Create("", "", "Title", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongMusic_null = MusicFactory.Create("", "", null, 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongMusic_empty = MusicFactory.Create("", "", " ", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight = validator.TestValidate(rightMusic);
//            var resultWrongNull = validator.TestValidate(wrongMusic_null);
//            var resultWrongEmpty = validator.TestValidate(wrongMusic_empty);


//            // Assert
//            resultRight.ShouldNotHaveValidationErrorFor(x => x.Title);
//            resultWrongNull.ShouldHaveValidationErrorFor(x => x.Title);
//            resultWrongEmpty.ShouldHaveValidationErrorFor(x => x.Title);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void Title_must_have_100chars_max_length()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightMusic = MusicFactory.Create("", "", StringHelper.GetWord(100), 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongMusic = MusicFactory.Create("", "", StringHelper.GetWord(101), 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight = validator.TestValidate(rightMusic);
//            var resultWrong = validator.TestValidate(wrongMusic);


//            // Assert
//            resultRight.ShouldNotHaveValidationErrorFor(x => x.Title);
//            resultWrong.ShouldHaveValidationErrorFor(x => x.Title);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void TrackNumber_must_be_greater_than_zero_when_not_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var right1 = MusicFactory.Create("", "", "", null, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var right2 = MusicFactory.Create("", "", "", 1, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrong1 = MusicFactory.Create("", "", "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrong2 = MusicFactory.Create("", "", "", -1, "", "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight1 = validator.TestValidate(right1);
//            var resultRight2 = validator.TestValidate(right2);
//            var resultWrong1 = validator.TestValidate(wrong1);
//            var resultWrong2 = validator.TestValidate(wrong2);


//            // Assert
//            resultRight1.ShouldNotHaveValidationErrorFor(x => x.TrackNumber);
//            resultRight2.ShouldNotHaveValidationErrorFor(x => x.TrackNumber);
//            resultWrong1.ShouldHaveValidationErrorFor(x => x.TrackNumber);
//            resultWrong2.ShouldHaveValidationErrorFor(x => x.TrackNumber);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void Lyrics_must_have_5000chars_max_length_when_not_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightNull = MusicFactory.Create("", "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var right5000 = MusicFactory.Create("", "", "", 0, StringHelper.GetWord(5000), "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrong = MusicFactory.Create("", "", "", 0, StringHelper.GetWord(5001), "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRightNull = validator.TestValidate(rightNull);
//            var resultRight5000 = validator.TestValidate(right5000);
//            var resultWrong = validator.TestValidate(wrong);


//            // Assert
//            resultRightNull.ShouldNotHaveValidationErrorFor(x => x.Lyrics);
//            resultRight5000.ShouldNotHaveValidationErrorFor(x => x.Lyrics);
//            resultWrong.ShouldHaveValidationErrorFor(x => x.Lyrics);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void Comments_must_have_300chars_max_length_when_not_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightNull = MusicFactory.Create("", "", "", 0, null, null, false, 0, null, null, 0, _propsReader, default(Album));
//            var right300 = MusicFactory.Create("", "", "", 0, null, StringHelper.GetWord(300), false, 0, null, null, 0, _propsReader, default(Album));
//            var wrong = MusicFactory.Create("", "", "", 0, null, StringHelper.GetWord(301), false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRightNull = validator.TestValidate(rightNull);
//            var resultRight300 = validator.TestValidate(right300);
//            var resultWrong = validator.TestValidate(wrong);


//            // Assert
//            resultRightNull.ShouldNotHaveValidationErrorFor(x => x.Comments);
//            resultRight300.ShouldNotHaveValidationErrorFor(x => x.Comments);
//            resultWrong.ShouldHaveValidationErrorFor(x => x.Comments);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void VocalGender_must_be_a_valid_enum()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var right = MusicFactory.Create("", "", "", 0, "", "", false, VocalGender.Undefined, null, null, 0, _propsReader, default(Album));
//            var wrong = MusicFactory.Create("", "", "", 0, "", "", false, (VocalGender)99, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight = validator.TestValidate(right);
//            var resultWrong = validator.TestValidate(wrong);


//            // Assert
//            resultRight.ShouldNotHaveValidationErrorFor(x => x.MusicDetails.VocalGender);
//            resultWrong.ShouldHaveValidationErrorFor(x => x.MusicDetails.VocalGender);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void CoveredArtist_must_have_50chars_max_length_when_not_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightNull = MusicFactory.Create("", "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var right50 = MusicFactory.Create("", "", "", 0, null, "", false, 0, StringHelper.GetWord(50), null, 0, _propsReader, default(Album));
//            var wrong51 = MusicFactory.Create("", "", "", 0, null, "", false, 0, StringHelper.GetWord(51), null, 0, _propsReader, default(Album));


//            // Act
//            var resultRightNull = validator.TestValidate(rightNull);
//            var resultRight50 = validator.TestValidate(right50);
//            var resultWrong51 = validator.TestValidate(wrong51);


//            // Assert
//            resultRightNull.ShouldNotHaveValidationErrorFor(x => x.MusicDetails.CoveredArtist);
//            resultRight50.ShouldNotHaveValidationErrorFor(x => x.MusicDetails.CoveredArtist);
//            resultWrong51.ShouldHaveValidationErrorFor(x => x.MusicDetails.CoveredArtist);
//        }


//        [TestMethod] [TestCategory(CATEGORY)]
//        public void FeaturedArtist_must_have_50chars_max_length_when_not_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightNull = MusicFactory.Create("", "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var right50 = MusicFactory.Create("", "", "", 0, null, "", false, 0, null, StringHelper.GetWord(50), 0, _propsReader, default(Album));
//            var wrong51 = MusicFactory.Create("", "", "", 0, null, "", false, 0, null, StringHelper.GetWord(51), 0, _propsReader, default(Album));


//            // Act
//            var resultRightNull = validator.TestValidate(rightNull);
//            var resultRight50 = validator.TestValidate(right50);
//            var resultWrong51 = validator.TestValidate(wrong51);


//            // Assert
//            resultRightNull.ShouldNotHaveValidationErrorFor(x => x.MusicDetails.FeaturedArtist);
//            resultRight50.ShouldNotHaveValidationErrorFor(x => x.MusicDetails.FeaturedArtist);
//            resultWrong51.ShouldHaveValidationErrorFor(x => x.MusicDetails.FeaturedArtist);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void Duration_must_be_greater_than_zero()
//        {
//            // Obs. If a file path doesn't exists, then Duration will return zero
//            // Arrange
//            var validator = _businessValidator;
//            var right = MusicFactory.Create(_sampleFileMp3, "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongFile = MusicFactory.Create(_0Duration64BitrateMp3, "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongNull = MusicFactory.Create("", null, "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongEmpty = MusicFactory.Create("", " ", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight = validator.TestValidate(right);
//            var resultWrongFile = validator.TestValidate(wrongFile);
//            var resultWrongNull = validator.TestValidate(wrongNull);
//            var resultWrongEmpty = validator.TestValidate(wrongEmpty);


//            // Assert
//            resultRight.ShouldNotHaveValidationErrorFor(x => x.MusicProperties.Duration);
//            resultWrongFile.ShouldHaveValidationErrorFor(x => x.MusicProperties.Duration);
//            resultWrongNull.ShouldHaveValidationErrorFor(x => x.MusicProperties.Duration);
//            resultWrongEmpty.ShouldHaveValidationErrorFor(x => x.MusicProperties.Duration);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void BitRate_must_be_greater_than_96kbps()
//        {
//            // Obs. If a file path doesn't exists, then Duration will return zero
//            // Arrange
//            var validator = _businessValidator;
//            var right = MusicFactory.Create(_sampleFileMp3, "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongFile = MusicFactory.Create(_0Duration64BitrateMp3, "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongNull = MusicFactory.Create(null, "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrongEmpty = MusicFactory.Create(" ", "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight = validator.TestValidate(right);
//            var resultWrongFile = validator.TestValidate(wrongFile);
//            var resultWrongNull = validator.TestValidate(wrongNull);
//            var resultWrongEmpty = validator.TestValidate(wrongEmpty);


//            // Assert
//            resultRight.ShouldNotHaveValidationErrorFor(x => x.MusicProperties.BitRate);
//            resultWrongFile.ShouldHaveValidationErrorFor(x => x.MusicProperties.BitRate);
//            resultWrongNull.ShouldHaveValidationErrorFor(x => x.MusicProperties.BitRate);
//            resultWrongEmpty.ShouldHaveValidationErrorFor(x => x.MusicProperties.BitRate);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void Album_cannot_be_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validAlbum = _repository.GetAll(false).First().Album;
//            var right = MusicFactory.Create("", "", "", 0, null, "", false, 0, null, null, 0, _propsReader, validAlbum);
//            var wrong = MusicFactory.Create("", "", "", 0, null, "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var resultRight = validator.TestValidate(right);
//            var resultWrong = validator.TestValidate(wrong);


//            // Assert
//            resultRight.ShouldNotHaveValidationErrorFor(x => x.Album);
//            resultWrong.ShouldHaveValidationErrorFor(x => x.Album);
//        }

//        #endregion

//        #region Persistence
//        [TestMethod][TestCategory(CATEGORY)]
//        public void FullPath_must_be_unique()
//        {
//            // Arrange
//            var validator = _persistenceValidator;
//            var repeatedPath = _repository.GetAll(false).First().FullPath;
//            var right = MusicFactory.Create("", _uniqueFileMp3, "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));
//            var wrong = MusicFactory.Create("", repeatedPath, "", 0, "", "", false, 0, null, null, 0, _propsReader, default(Album));


//            // Act
//            var rightResult = validator.TestValidate(right);
//            var wrongResult = validator.TestValidate(wrong);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.FullPath);
//            wrongResult.ShouldHaveValidationErrorFor(x => x.FullPath);
//        }
//        #endregion
//    }
//}

