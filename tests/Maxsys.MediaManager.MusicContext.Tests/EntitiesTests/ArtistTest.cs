using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Factories;
using Maxsys.MediaManager.MusicContext.Domain.Validators;
using Maxsys.MediaManager.MusicContext.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.Tests.EntitiesTests
{
    [TestClass]
    [TestCategory("Domain.Entity: " + nameof(Artist))]
    public class ArtistTest
    {
        #region Fields

        private const string VALID_NAME = "123 right-name γικντόη";
        private static readonly Guid VALID_ID = Guid.NewGuid();
        private static readonly Catalog MUSIC_CATALOG = CatalogFactory.Create(VALID_ID, VALID_NAME);

        #endregion Fields

        [TestMethod]
        public void Artist_must_be_valid()
        {
            // Arrange
            var artist = ArtistFactory.Create(VALID_NAME, VALID_ID, VALID_ID);
            var validator = new ArtistValidator(null);
            validator.AddRuleForId();
            validator.AddRuleForName();
            validator.AddRuleForMusicCatalog();
            //validator.AddRuleForUniqueNameInMusicCatalog();

            // Act
            var result = validator.Validate(artist);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MusicCatalog_cannot_be_null()
        {
            // Arrange
            var validArtist = ArtistFactory.Create(VALID_NAME, VALID_ID, VALID_ID);
            var invalidArtist = ArtistFactory.Create(VALID_NAME, VALID_ID, default);
            var validator = new ArtistValidator(null).AddRuleForMusicCatalog();

            // Act
            var rightResult = validator.Validate(validArtist);
            var wrongResult = validator.Validate(invalidArtist);

            // Assert
            Assert.IsTrue(rightResult.IsValid);
            Assert.IsFalse(wrongResult.IsValid);
        }

        [TestMethod]
        public void Name_cannot_be_null()
        {
            // Arrange
            var artist = ArtistFactory.Create(null, VALID_ID, VALID_ID);
            var validator = new ArtistValidator(null).AddRuleForName();

            // Act
            var result = validator.Validate(artist);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Name_cannot_be_empty()
        {
            // Arrange
            var artist = ArtistFactory.Create("   ", VALID_ID, VALID_ID);
            var validator = new ArtistValidator(null).AddRuleForName();

            // Act
            var result = validator.Validate(artist);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Name_must_have_50chars_max_length()
        {
            // Arrange
            var rightArtist = ArtistFactory.Create(StringHelper.GetWord(50), VALID_ID, VALID_ID);
            var wrongArtist = ArtistFactory.Create(StringHelper.GetWord(51), VALID_ID, VALID_ID);
            var validator = new ArtistValidator(null).AddRuleForName();

            // Act
            var right = validator.Validate(rightArtist);
            var wrong = validator.Validate(wrongArtist);

            // Assert
            Assert.IsTrue(right.IsValid);
            Assert.IsFalse(wrong.IsValid);
        }

        [TestMethod]
        public void Name_must_contains_only_letters_numbers_spaces_hyphens()
        {
            // Arrange
            var artist = ArtistFactory.Create("(wrong_name){[]=-+.}", VALID_ID, VALID_ID);
            var validator = new ArtistValidator(null).AddRuleForName();

            // Act
            var result = validator.Validate(artist);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public async Task Name_must_be_unique_in_musicCatalog()
        {
            // Arrange
            var context = new MockContext();
            var repository = context.Artists;
            var musicCatalog = context.MusicCatalogs.GetAll().First();
            var rightArtist = ArtistFactory.Create("Unique Name", Guid.NewGuid(), default);
            var wrongArtist = ArtistFactory.Create("Unique Name", Guid.NewGuid(), default);
            rightArtist.SetMusicCatalog(musicCatalog);
            wrongArtist.SetMusicCatalog(musicCatalog);

            var validator = new ArtistValidator(repository);
            validator.AddRuleForUniqueNameInMusicCatalog();

            // Act
            var right = await validator.ValidateAsync(rightArtist);
            repository.Add(rightArtist);
            var wrong = await validator.ValidateAsync(wrongArtist);

            // Assert
            Assert.IsTrue(right.IsValid);
            Assert.IsFalse(wrong.IsValid);
        }

        [TestMethod]
        public async Task Must_Not_Contains_Any_Albums()
        {
            // Arrange

            // Act

            // Assert
            Assert.Inconclusive("Not implemented");
        }
    }
}