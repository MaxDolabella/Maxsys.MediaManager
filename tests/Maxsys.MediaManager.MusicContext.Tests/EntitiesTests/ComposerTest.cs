//using FluentValidation;
//using FluentValidation.TestHelper;
//using Maxsys.MediaManager.MusicContext.Domain.Entities;
//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Validation.ComposerValidators;
//using Maxsys.MediaManager.Tests.MockMusicContext;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;

//namespace Maxsys.MediaManager.Tests.EntitiesTests
//{
//    [TestClass]
//    public class ComposerTest
//    {
//        #region Fields
//        const string CATEGORY = "Domain.Entity: " + nameof(Composer);
//        static readonly IComposerRepository _repository = new ComposerMockRepository();
//        static readonly IValidator<Composer> _businessValidator = new ComposerBusinessValidator(); 
//        static readonly IValidator<Composer> _persistenceValidator = new ComposerPersistenceValidator(_repository);
//        #endregion


//        #region Business
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_must_contains_only_letters_numbers_spaces_hyphens()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightComposer = ComposerFactory.Create("123 right-name γικντόη");
//            var wrongComposer = ComposerFactory.Create("(wrong_name){[]=-+.}");


//            // Act
//            var right = validator.TestValidate(rightComposer);
//            var wrong = validator.TestValidate(wrongComposer);


//            // Assert
//            right.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrong.ShouldHaveValidationErrorFor(x => x.Name);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_cannot_be_null_or_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightComposer = ComposerFactory.Create("123 right-name γικντόη");
//            var wrongComposerN = ComposerFactory.Create(null);
//            var wrongComposerE = ComposerFactory.Create("   ");


//            // Act
//            var right = validator.TestValidate(rightComposer);
//            var wrongN = validator.TestValidate(wrongComposerN);
//            var wrongE = validator.TestValidate(wrongComposerE);


//            // Assert
//            right.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrongN.ShouldHaveValidationErrorFor(x => x.Name);
//            wrongE.ShouldHaveValidationErrorFor(x => x.Name);
//        }


//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_must_have_30chars_max_length()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightComposer = ComposerFactory.Create(StringHelper.GetWord(30));
//            var wrongComposer = ComposerFactory.Create(StringHelper.GetWord(31));


//            // Act
//            var right = validator.TestValidate(rightComposer);
//            var wrong = validator.TestValidate(wrongComposer);


//            // Assert
//            right.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrong.ShouldHaveValidationErrorFor(x => x.Name);
//        }
//        #endregion

//        #region Persistence
//        [TestMethod][TestCategory(CATEGORY)]
//        public void Name_must_be_unique()
//        {
//            // Arrange
//            var validator = _persistenceValidator;
//            var repeatedName = _repository.GetAll(false).First().Name;
//            var rightComposer = ComposerFactory.Create("Unique Name");
//            var wrongComposer = ComposerFactory.Create(repeatedName);


//            // Act
//            var right = validator.TestValidate(rightComposer);
//            var wrong = validator.TestValidate(wrongComposer);


//            // Assert
//            right.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrong.ShouldHaveValidationErrorFor(x => x.Name);
//        }
//        #endregion
//    }
//}
