//using Maxsys.MediaManager.MusicContext.Domain.Entities;
//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Validators;
//using Maxsys.MediaManager.MusicContext.Tests.Mock;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.Tests.EntitiesTests
//{
//    [TestClass]
//    [TestCategory("Domain.Entity: " + nameof(Catalog))]
//    public class MusicCatalogTest
//    {
//        #region Fields

//        private const string VALID_NAME = "123 right-name γικντόη";
//        private static readonly Guid VALID_ID = Guid.NewGuid();

//        #endregion Fields

//        [TestMethod]
//        public void MusicCatalog_must_be_valid()
//        {
//            // Arrange
//            var musicCatalog = CatalogFactory.Create(VALID_ID, VALID_NAME);
//            var validator = new CatalogValidator(null);
//            validator.AddRuleForId();
//            validator.AddRuleForName();

//            // Act
//            var result = validator.Validate(musicCatalog);

//            // Assert
//            Assert.IsTrue(result.IsValid);
//        }

//        [TestMethod]
//        public void Name_cannot_be_null()
//        {
//            // Arrange
//            var musicCatalog = CatalogFactory.Create(VALID_ID, null);
//            var validator = new CatalogValidator(null);
//            validator.AddRuleForName();

//            // Act
//            var result = validator.Validate(musicCatalog);

//            // Assert
//            Assert.IsFalse(result.IsValid);
//        }

//        [TestMethod]
//        public void Name_cannot_be_empty()
//        {
//            // Arrange
//            var musicCatalog = CatalogFactory.Create(VALID_ID, "   ");
//            var validator = new CatalogValidator(null);
//            validator.AddRuleForName();

//            // Act
//            var result = validator.Validate(musicCatalog);

//            // Assert
//            Assert.IsFalse(result.IsValid);
//        }

//        [TestMethod]
//        public void Name_must_contains_only_letters_numbers_spaces_hyphens()
//        {
//            // Arrange
//            var musicCatalog = CatalogFactory.Create(VALID_ID, "(wrong_name){[]=-+.}");
//            var validator = new CatalogValidator(null);
//            validator.AddRuleForName();

//            // Act
//            var result = validator.Validate(musicCatalog);

//            // Assert
//            Assert.IsFalse(result.IsValid);
//        }

//        [TestMethod]
//        public void Name_must_have_50chars_max_length()
//        {
//            // Arrange
//            var rightMusicCatalog = CatalogFactory.Create(VALID_ID, StringHelper.GetWord(50));
//            var wrongMusicCatalog = CatalogFactory.Create(VALID_ID, StringHelper.GetWord(51));
//            var validator = new CatalogValidator(null);
//            validator.AddRuleForName();

//            // Act
//            var right = validator.Validate(rightMusicCatalog);
//            var wrong = validator.Validate(wrongMusicCatalog);

//            // Assert
//            Assert.IsTrue(right.IsValid);
//            Assert.IsFalse(wrong.IsValid);
//        }

//        [TestMethod]
//        public void Name_must_have_2chars_min_length()
//        {
//            // Arrange
//            var rightMusicCatalog = CatalogFactory.Create(VALID_ID, "AB");
//            var wrongMusicCatalog = CatalogFactory.Create(VALID_ID, "A");
//            var validator = new CatalogValidator(null);
//            validator.AddRuleForName();

//            // Act
//            var right = validator.Validate(rightMusicCatalog);
//            var wrong = validator.Validate(wrongMusicCatalog);

//            // Assert
//            Assert.IsTrue(right.IsValid);
//            Assert.IsFalse(wrong.IsValid);
//        }

//        [TestMethod]
//        public async Task Name_must_be_unique()
//        {
//            // Arrange
//            var repository = new MockContext().MusicCatalogs;
//            var validator = new CatalogValidator(repository);
//            var rightMusicCatalog = CatalogFactory.Create(Guid.NewGuid(), "CatalogName");
//            var wrongMusicCatalog = CatalogFactory.Create(Guid.NewGuid(), "CatalogName");
//            validator.AddRuleForUniqueName();

//            // Act
//            var right = await validator.ValidateAsync(rightMusicCatalog);
//            repository.Add(rightMusicCatalog);
//            var wrong = await validator.ValidateAsync(wrongMusicCatalog);

//            // Assert
//            Assert.IsTrue(right.IsValid);
//            Assert.IsFalse(wrong.IsValid);
//        }

//        [TestMethod]
//        public async Task Must_Not_Contains_Any_Artist()
//        {
//            // Arrange
//            var repository = new MockContext().MusicCatalogs;
//            var validator = new CatalogValidator(repository);
//            var rightMusicCatalog = CatalogFactory.Create(Guid.NewGuid(), "ABC");
//            var wrongMusicCatalog = CatalogFactory.Create(Guid.NewGuid(), "XYZ");
//            validator.AddRuleForMustNotContainsAnyArtist();

//            // Act
//            wrongMusicCatalog.Artists.Add(ArtistFactory.Create("Name", VALID_ID, VALID_ID));
//            repository.Add(rightMusicCatalog);
//            repository.Add(wrongMusicCatalog);
//            var right = await validator.ValidateAsync(rightMusicCatalog);
//            var wrong = await validator.ValidateAsync(wrongMusicCatalog);

//            // Assert
//            Assert.IsTrue(right.IsValid);
//            Assert.IsFalse(wrong.IsValid);
//        }
//    }
//}