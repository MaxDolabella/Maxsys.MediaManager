using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Factories;
using Maxsys.MediaManager.MusicContext.Domain.Validators;
using Maxsys.MediaManager.MusicContext.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.Tests.EntitiesTests
{
    [TestClass]
    [TestCategory("Domain.Entity: " + nameof(Composer))]
    public class ComposerTest
    {
        #region Fields

        private const string VALID_NAME = "123 right-name γικντόη";
        private static readonly Guid VALID_ID = Guid.NewGuid();

        #endregion Fields

        [TestMethod]
        public void Composer_must_be_valid()
        {
            // Arrange
            var composer = ComposerFactory.Create(VALID_ID, VALID_NAME);
            var validator = new ComposerValidator(null);
            validator.AddRuleForId();
            validator.AddRuleForName();

            // Act
            var result = validator.Validate(composer);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Name_cannot_be_null()
        {
            // Arrange
            var composer = ComposerFactory.Create(VALID_ID, null);
            var validator = new ComposerValidator(null);
            validator.AddRuleForName();

            // Act
            var result = validator.Validate(composer);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Name_cannot_be_empty()
        {
            // Arrange
            var composer = ComposerFactory.Create(VALID_ID, "   ");
            var validator = new ComposerValidator(null);
            validator.AddRuleForName();

            // Act
            var result = validator.Validate(composer);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Name_must_contains_only_letters_numbers_spaces_hyphens()
        {
            // Arrange
            var composer = ComposerFactory.Create(VALID_ID, "(wrong_name){[]=-+.}");
            var validator = new ComposerValidator(null);
            validator.AddRuleForName();

            // Act
            var result = validator.Validate(composer);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Name_must_have_30chars_max_length()
        {
            // Arrange
            var rightComposer = ComposerFactory.Create(VALID_ID, StringHelper.GetWord(30));
            var wrongComposer = ComposerFactory.Create(VALID_ID, StringHelper.GetWord(31));
            var validator = new ComposerValidator(null);
            validator.AddRuleForName();

            // Act
            var right = validator.Validate(rightComposer);
            var wrong = validator.Validate(wrongComposer);

            // Assert
            Assert.IsTrue(right.IsValid);
            Assert.IsFalse(wrong.IsValid);
        }

        [TestMethod]
        public void Name_must_have_2chars_min_length()
        {
            // Arrange
            var rightComposer = ComposerFactory.Create(VALID_ID, "AB");
            var wrongComposer = ComposerFactory.Create(VALID_ID, "A");
            var validator = new ComposerValidator(null);
            validator.AddRuleForName();

            // Act
            var right = validator.Validate(rightComposer);
            var wrong = validator.Validate(wrongComposer);

            // Assert
            Assert.IsTrue(right.IsValid);
            Assert.IsFalse(wrong.IsValid);
        }

        [TestMethod]
        public async Task Name_must_be_unique()
        {
            // Arrange
            var repository = new MockContext().Composers;
            var rightComposer = ComposerFactory.Create(Guid.NewGuid(), "CatalogName");
            var wrongComposer = ComposerFactory.Create(Guid.NewGuid(), "CatalogName");
            var validator = new ComposerValidator(repository);
            validator.AddRuleForUniqueName();

            // Act
            var right = await validator.ValidateAsync(rightComposer);
            repository.Add(rightComposer);
            var wrong = await validator.ValidateAsync(wrongComposer);

            // Assert
            Assert.IsTrue(right.IsValid);
            Assert.IsFalse(wrong.IsValid);
        }

        [TestMethod]
        public async Task Must_Not_Be_Present_In_Any_Music()
        {
            // Arrange

            // Act

            // Assert
            Assert.Inconclusive("Not implemented yet.");
        }
    }
}