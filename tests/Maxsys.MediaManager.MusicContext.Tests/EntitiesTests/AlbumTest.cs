//using FluentValidation;
//using FluentValidation.TestHelper;
//using Maxsys.MediaManager.MusicContext.Domain.Entities;
//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Validation.AlbumValidators;
//using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
//using Maxsys.MediaManager.Tests.MockMusicContext;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;

//namespace Maxsys.MediaManager.Tests.EntitiesTests
//{
//    [TestClass]
//    public class AlbumTest
//    {
//        #region Fields
//        const string CATEGORY = "Domain.Entity: " + nameof(Album);
//        const string albumDir = nameof(albumDir);
//        static readonly IAlbumRepository _repository = new AlbumMockRepository();
//        static readonly IValidator<Album> _businessValidator = new AlbumBusinessValidator(); 
//        static readonly IValidator<Album> _persistenceValidator = new AlbumPersistenceValidator(_repository);
//        #endregion

//        #region Business
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Artist_cannot_be_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "Others", new byte[] { }, AlbumType.Studio, default(Artist));

//            // Act
//            var rightResult = validator.TestValidate(rightAlbum);
//            var wrongResult = validator.TestValidate(wrongAlbum);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.Artist);
//            wrongResult.ShouldHaveValidationErrorFor(x => x.Artist);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_cannot_be_null_or_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, "123 right-name.(γικ,ντόη)", 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbumN = AlbumFactory.Create(albumDir, null, 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbumE = AlbumFactory.Create(albumDir, "   ", 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);


//            // Act
//            var rightResult = validator.TestValidate(rightAlbum);
//            var wrongNResult = validator.TestValidate(wrongAlbumN);
//            var wrongEResult = validator.TestValidate(wrongAlbumE);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrongNResult.ShouldHaveValidationErrorFor(x => x.Name);
//            wrongEResult.ShouldHaveValidationErrorFor(x => x.Name);
//        }

        
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_must_have_50chars_max_length()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, StringHelper.GetWord(50), 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum = AlbumFactory.Create(albumDir, StringHelper.GetWord(51), 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);


//            // Act
//            var right = validator.Validate(rightAlbum).IsValid;
//            var wrong = validator.Validate(wrongAlbum).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void Year_must_be_between_1500_and_2100()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum1 = AlbumFactory.Create(albumDir, "Name", 1499, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum2 = AlbumFactory.Create(albumDir, "Name", 2101, "Others", new byte[] { }, AlbumType.Studio, validArtist);


//            // Act
//            var right = validator.Validate(rightAlbum).IsValid;
//            var wrong1 = validator.Validate(wrongAlbum1).IsValid;
//            var wrong2 = validator.Validate(wrongAlbum2).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong1);
//            Assert.IsFalse(wrong2);
//        }

        
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Genre_cannot_be_null_or_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "1 ok-name γικντόη", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum1 = AlbumFactory.Create(albumDir, "Name", 2020, null, new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum2 = AlbumFactory.Create(albumDir, "Name", 2020, "  ", new byte[] { }, AlbumType.Studio, validArtist);


//            // Act
//            var right = validator.Validate(rightAlbum).IsValid;
//            var wrong1 = validator.Validate(wrongAlbum1).IsValid;
//            var wrong2 = validator.Validate(wrongAlbum2).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong1, "null value");
//            Assert.IsFalse(wrong2, "empty value");
//        }

        
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Genre_must_have_50chars_length()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, "Name", 2020, StringHelper.GetWord(50), new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum = AlbumFactory.Create(albumDir, "Name", 2020, StringHelper.GetWord(51), new byte[] { }, AlbumType.Studio, validArtist);


//            // Act
//            var right = validator.Validate(rightAlbum).IsValid;
//            var wrong = validator.Validate(wrongAlbum).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong);
//        }

//        [TestMethod][TestCategory(CATEGORY)]
//        public void AlbumType_must_be_a_valid_enum()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "Others", new byte[] { }, (AlbumType)99, validArtist);


//            // Act
//            var right = validator.Validate(rightAlbum).IsValid;
//            var wrong = validator.Validate(wrongAlbum).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong);
//        }

        
//        [TestMethod][TestCategory(CATEGORY)]
//        public void AlbumCover_cannot_be_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validArtist = _repository.GetAll(false).First().Artist;
//            var rightAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum = AlbumFactory.Create(albumDir, "Name", 2020, "Others", null, AlbumType.Studio, validArtist);


//            // Act
//            var right = validator.Validate(rightAlbum).IsValid;
//            var wrong = validator.Validate(wrongAlbum).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong);
//        }
//        #endregion

//        #region Persistence
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_must_be_unique_from_Artist()
//        {
//            // Arrange
//            var validator = _persistenceValidator;
//            var validArtist = _repository.GetAll(true).First().Artist;
//            var repeatedAlbumNameFromArtist = _repository.GetAll(true).Where(x => x.Artist.Id == validArtist.Id).First().Name;
//            var rightAlbum = AlbumFactory.Create(albumDir, "Unique Name", 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);
//            var wrongAlbum = AlbumFactory.Create(albumDir, repeatedAlbumNameFromArtist, 2020, "Others", new byte[] { }, AlbumType.Studio, validArtist);


//            // Act
//            var right = validator.Validate(rightAlbum).IsValid;
//            var wrong = validator.Validate(wrongAlbum).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong);
//        }
//        #endregion
//    }
//}

