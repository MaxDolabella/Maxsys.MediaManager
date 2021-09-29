//using FluentValidation;
//using FluentValidation.TestHelper;
//using Maxsys.MediaManager.MusicContext.Domain.Entities;
//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Validation.PlaylistValidators;
//using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
//using Maxsys.MediaManager.Tests.MockMusicContext;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;

//namespace Maxsys.MediaManager.Tests.EntitiesTests
//{
//    [TestClass]
//    public class PlaylistTest
//    {
//        #region Fields
//        const string CATEGORY = "Domain.Entity: " + nameof(Playlist);
//        static readonly IPlaylistRepository _repository = new PlaylistMockRepository();
//        static readonly IValidator<Playlist> _businessValidator = new PlaylistBusinessValidator(); 
//        static readonly IValidator<Playlist> _persistenceValidator = new PlaylistPersistenceValidator(_repository);
//        const string VALID_NAME = "1-nãéêíôüç";
//        #endregion

//        #region Business
//        [TestMethod]
//        [TestCategory(CATEGORY)]
//        public void Name_must_contains_only_letters_numbers_spaces_hyphens()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightPlaylist = PlaylistFactory.Create(VALID_NAME);
//            var wrongPlaylist = PlaylistFactory.Create("({[=+._]})");


//            // Act
//            var rightResult = validator.TestValidate(rightPlaylist);
//            var wrongResult = validator.TestValidate(wrongPlaylist);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrongResult.ShouldHaveValidationErrorFor(x => x.Name);
//        }


//        [TestMethod]
//        [TestCategory(CATEGORY)]
//        public void Name_cannot_be_null_or_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightPlaylist = PlaylistFactory.Create(VALID_NAME);
//            var wrongPlaylistN = PlaylistFactory.Create(null);
//            var wrongPlaylistE = PlaylistFactory.Create("  ");


//            // Act
//            var rightResult = validator.TestValidate(rightPlaylist);
//            var wrongResultN = validator.TestValidate(wrongPlaylistN);
//            var wrongResultE = validator.TestValidate(wrongPlaylistE);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrongResultN.ShouldHaveValidationErrorFor(x => x.Name);
//            wrongResultE.ShouldHaveValidationErrorFor(x => x.Name);
//        }


//        [TestMethod]
//        [TestCategory(CATEGORY)]
//        public void Name_must_have_between_3_and_20chars_length()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightPlaylist1 = PlaylistFactory.Create(StringHelper.GetWord(3));
//            var rightPlaylist2 = PlaylistFactory.Create(StringHelper.GetWord(20));
//            var wrongPlaylist1 = PlaylistFactory.Create(StringHelper.GetWord(2));
//            var wrongPlaylist2 = PlaylistFactory.Create(StringHelper.GetWord(21));


//            // Act
//            var rightResult1 = validator.TestValidate(rightPlaylist1);
//            var rightResult2 = validator.TestValidate(rightPlaylist2);
//            var wrongResult1 = validator.TestValidate(wrongPlaylist1);
//            var wrongResult2 = validator.TestValidate(wrongPlaylist2);


//            // Assert
//            rightResult1.ShouldNotHaveValidationErrorFor(x => x.Name);
//            rightResult2.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrongResult1.ShouldHaveValidationErrorFor(x => x.Name);
//            wrongResult2.ShouldHaveValidationErrorFor(x => x.Name);
//        }


//        [TestMethod]
//        [TestCategory(CATEGORY)]
//        public void Items_must_not_be_empty()
//        {
//            // Arrange
//            var validator = _businessValidator;
//            var rightPlaylist = PlaylistFactory.Create(VALID_NAME);
//            var wrongPlaylist = PlaylistFactory.Create(VALID_NAME);


//            // Act
//            var musics = new MusicMockRepository().GetAll(false).Take(3);
//            foreach (var music in musics) 
//                rightPlaylist.Items.Add(music, 0);
//            var rightResult = validator.TestValidate(rightPlaylist);
//            var wrongResult = validator.TestValidate(wrongPlaylist);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.Items);
//            wrongResult.ShouldHaveValidationErrorFor(x => x.Items);
//        }
//        #endregion


//        #region Persistence
//        [TestMethod]
//        [TestCategory(CATEGORY)]
//        public void Name_must_be_unique()
//        {
//            // Arrange
//            var validator = _persistenceValidator;
//            var repeatedName = _repository.GetAll(false).First().Name;
//            var rightPlaylist = PlaylistFactory.Create("Unique Name");
//            var wrongPlaylist = PlaylistFactory.Create(repeatedName);


//            // Act
//            var rightResult = validator.TestValidate(rightPlaylist);
//            var wrongResult = validator.TestValidate(wrongPlaylist);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.Name);
//            wrongResult.ShouldHaveValidationErrorFor(x => x.Name);
//        }


//        [TestMethod]
//        [TestCategory(CATEGORY)]
//        public void Items_must_be_unique()
//        {
//            Assert.Inconclusive($"{nameof(PlaylistItemCollection)} manages entries allowing only unique items.");
            
//            /*/
//            // Arrange
//            var validator = _persistenceValidator;
//            var rightPlaylist = PlaylistFactory.Create(VALID_NAME);
//            var wrongPlaylist = PlaylistFactory.Create(VALID_NAME);


//            // Act
//            var musics = new MusicMockRepository().GetAll(false).Take(3).ToList();
//            var repeatedMusic = musics[0];
//            foreach (var music in musics)
//            {
//                rightPlaylist.Items.Add(music, 0);
//                wrongPlaylist.Items.Add(music, 0);
//            }
//            wrongPlaylist.Items.Add(repeatedMusic, 0);
//            var rightResult = validator.TestValidate(rightPlaylist);
//            var wrongResult = validator.TestValidate(wrongPlaylist);


//            // Assert
//            rightResult.ShouldNotHaveValidationErrorFor(x => x.Items);
//            wrongResult.ShouldHaveValidationErrorFor(x => x.Items);
//            //*/
//        }
//        #endregion
//    }
//}
