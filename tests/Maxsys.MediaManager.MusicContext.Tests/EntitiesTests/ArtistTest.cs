//using FluentValidation;
//using FluentValidation.TestHelper;
//using Maxsys.MediaManager.MusicContext.Domain.Entities;
//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Validation.ArtistValidators;
//using Maxsys.MediaManager.Tests.MockMusicContext;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;

//namespace Maxsys.MediaManager.Tests.EntitiesTests
//{
//    [TestClass]
//    public class ArtistTest
//    {
//        #region Fields
//        const string CATEGORY = "Domain.Entity: " + nameof(Artist);
//        static readonly IArtistRepository _repository = new ArtistMockRepository();
//        static readonly IValidator<Artist> _businessValidator = new ArtistBusinessValidator(); 
//        static readonly IValidator<Artist> _persistenceValidator = new ArtistPersistenceValidator(_repository);
//        #endregion

//        #region Business
//        [TestMethod][TestCategory(CATEGORY)]
//        public void MusicCatalog_cannot_be_null()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validMusicCatalog = _repository.GetAll(false).First().MusicCatalog;
//            var rightArtist = ArtistFactory.Create("Name", validMusicCatalog);
//            var wrongArtist = ArtistFactory.Create("Name", default(MusicCatalog));

//            // Act
//            var rightResult = validator.TestValidate(rightArtist);
//            var wrongResult = validator.TestValidate(wrongArtist);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.MusicCatalog);
//            wrongResult.ShouldHaveValidationErrorFor(x => x.MusicCatalog);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_cannot_be_null_or_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validMusicCatalog = _repository.GetAll(false).First().MusicCatalog;
//            var rightArtist = ArtistFactory.Create("123 right-name γικντόη", validMusicCatalog);
//            var wrongArtist_null = ArtistFactory.Create(null, validMusicCatalog);
//            var wrongArtist_empty = ArtistFactory.Create("   ", validMusicCatalog);


//            // Act
//            var right = validator.Validate(rightArtist).IsValid;
//            var wrongNull = validator.Validate(wrongArtist_null).IsValid;
//            var wrongEmpty = validator.Validate(wrongArtist_empty).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrongNull, "null value");
//            Assert.IsFalse(wrongEmpty, "empty value");
//        }

        
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_must_have_50chars_max_length()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var validMusicCatalog = _repository.GetAll(false).First().MusicCatalog;
//            var rightArtist = ArtistFactory.Create(StringHelper.GetWord(50), validMusicCatalog);
//            var wrongArtist = ArtistFactory.Create(StringHelper.GetWord(51), validMusicCatalog);


//            // Act
//            var right = validator.Validate(rightArtist).IsValid;
//            var wrong = validator.Validate(wrongArtist).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong);
//        }
//        #endregion

//        #region Persistence
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_must_be_unique_in_MusicCatalog()
//        {
//            // Arrange
//            var validator = _persistenceValidator;
//            var validMusicCatalog = _repository.GetAll(false).First().MusicCatalog;
//            var repeatedName = _repository.GetAll(false).First().Name;
//            var rightArtist = ArtistFactory.Create("Unique Name", validMusicCatalog);
//            var wrongArtist = ArtistFactory.Create(repeatedName, validMusicCatalog);


//            // Act
//            var right = validator.Validate(rightArtist).IsValid;
//            var wrong = validator.Validate(wrongArtist).IsValid;


//            // Assert
//            Assert.IsTrue(right);
//            Assert.IsFalse(wrong);
//        }
//        #endregion
//    }
//}

