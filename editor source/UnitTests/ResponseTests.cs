using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPNATI_Character_Editor;

namespace UnitTests
{
	[TestClass]
	/// <summary>
	/// Test for making sure the right response tag is given to a spoken tag
	/// </summary>
	public class ResponseTests
	{
		private static Character _female;
		private static Character _male;
		private static Character _bifemale;
		private static Character _bimale;
		private static Character _femaleMajor;
		private static Character _maleMajor;
		private static Character _bifemaleMajor;
		private static Character _bimaleMajor;

		[TestInitialize]
		public void Init()
		{
			TriggerDatabase.Load();

			_female = new Character() { Gender = "female", FolderName = "female" };
			_male = new Character() { Gender = "male", FolderName = "male" };
			_bifemale = new Character() { Gender = "female", FolderName = "nifemale" };
			_bimale = new Character() { Gender = "male", FolderName = "nimale" };
			_femaleMajor = new Character() { Gender = "female", FolderName = "femaleMajor" };
			_maleMajor = new Character() { Gender = "male", FolderName = "maleMajor" };
			_bifemaleMajor = new Character() { Gender = "female", FolderName = "bifemaleMajor" };
			_bimaleMajor = new Character() { Gender = "male", FolderName = "bimaleMajor" };
			foreach (Character c in new Character[] { _female, _male, _bifemale, _bimale })
			{
				c.AddLayer(new Clothing() { Type = "extra" });
				c.AddLayer(new Clothing() { Type = "minor" });
				c.AddLayer(new Clothing() { Type = "major", Position = "legs" });
				c.AddLayer(new Clothing() { Type = "important", Position = "upper" });
				c.AddLayer(new Clothing() { Type = "important", Position = "lower" });
			}
			foreach (Character c in new Character[] { _femaleMajor, _maleMajor, _bifemaleMajor, _bimaleMajor })
			{
				c.AddLayer(new Clothing() { Type = "extra" });
				c.AddLayer(new Clothing() { Type = "minor" });
				c.AddLayer(new Clothing() { Type = "major", Position = "both" });
				c.AddLayer(new Clothing() { Type = "major", Position = "upper" });
				c.AddLayer(new Clothing() { Type = "major", Position = "lower" });
			}
			_bimale.Metadata.CrossGender = true;
			_bifemale.Metadata.CrossGender = true;
			_bimaleMajor.Metadata.CrossGender = true;
			_bifemaleMajor.Metadata.CrossGender = true;
			CharacterDatabase.Add(_female);
			CharacterDatabase.Add(_male);
			CharacterDatabase.Add(_bifemale);
			CharacterDatabase.Add(_bimale);
			CharacterDatabase.Add(_femaleMajor);
			ExpressionTests.AddVariableFunction("costume");
			ExpressionTests.AddVariableFunction("marker");
		}

		[TestCleanup]
		public void CleanUp()
		{
			TriggerDatabase.Clear();
			VariableDatabase.Clear();
			CharacterDatabase.Clear();
		}

		[TestMethod]
		public void SwapCards()
		{
			Case c = new Case("swap_cards");
			c.Stages.Add(0);
			Assert.AreEqual("swap_cards", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void MustStrip_Winning_Female()
		{
			Case c = new Case("must_strip_winning");
			c.Stages.Add(0);
			Assert.AreEqual("female_must_strip", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void MustStrip_Normal_Female()
		{
			Case c = new Case("must_strip_normal");
			c.Stages.Add(0);
			Assert.AreEqual("female_must_strip", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void MustStrip_Losing_Female()
		{
			Case c = new Case("must_strip_losing");
			c.Stages.Add(0);
			Assert.AreEqual("female_must_strip", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void MustStrip_Winning_Male()
		{
			Case c = new Case("must_strip_winning");
			c.Stages.Add(0);
			Assert.AreEqual("male_must_strip", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void MustStrip_Normal_Male()
		{
			Case c = new Case("must_strip_normal");
			c.Stages.Add(0);
			Assert.AreEqual("male_must_strip", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void MustStrip_Losing_Male()
		{
			Case c = new Case("must_strip_losing");
			c.Stages.Add(0);
			Assert.AreEqual("male_must_strip", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripping_Extra_Female()
		{
			Case c = new Case("stripping");
			c.Stages.Add(0);
			Assert.AreEqual("female_removing_accessory", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripping_Minor_Female()
		{
			Case c = new Case("stripping");
			c.Stages.Add(1);
			Assert.AreEqual("female_removing_minor", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripping_Major_Female()
		{
			Case c = new Case("stripping");
			c.Stages.Add(2);
			Assert.AreEqual("female_removing_major", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripping_Upper_Female()
		{
			Case c = new Case("stripping");
			c.Stages.Add(3);
			Assert.AreEqual("female_chest_will_be_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripping_Lower_Female()
		{
			Case c = new Case("stripping");
			c.Stages.Add(4);
			Assert.AreEqual("female_crotch_will_be_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripping_Upper_Female_AsImportant()
		{
			Case c = new Case("stripping");
			c.Stages.Add(3);
			Assert.AreEqual("female_chest_will_be_visible", c.GetResponseTag(_femaleMajor, _male));
		}

		[TestMethod]
		public void Stripping_Lower_Female_AsImportant()
		{
			Case c = new Case("stripping");
			c.Stages.Add(4);
			Assert.AreEqual("female_crotch_will_be_visible", c.GetResponseTag(_femaleMajor, _male));
		}

		[TestMethod]
		public void Male_Stripping_Extra_Male()
		{
			Case c = new Case("stripping");
			c.Stages.Add(0);
			Assert.AreEqual("male_removing_accessory", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripping_Minor_Male()
		{
			Case c = new Case("stripping");
			c.Stages.Add(1);
			Assert.AreEqual("male_removing_minor", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripping_Major_Male()
		{
			Case c = new Case("stripping");
			c.Stages.Add(2);
			Assert.AreEqual("male_removing_major", c.GetResponseTag(_male, _female));
		}


		[TestMethod]
		public void Stripping_Major_Male_Both()
		{
			Case c = new Case("stripping");
			c.Stages.Add(2);
			_male.Wardrobe[2].Position = "both";
			Assert.AreEqual("male_removing_major", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripping_Upper_Male()
		{
			Case c = new Case("stripping");
			c.Stages.Add(3);
			Assert.AreEqual("male_chest_will_be_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripping_Lower_Male()
		{
			Case c = new Case("stripping");
			c.Stages.Add(4);
			Assert.AreEqual("male_crotch_will_be_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripping_Upper_Male_AsImportant()
		{
			Case c = new Case("stripping");
			c.Stages.Add(3);
			Assert.AreEqual("male_chest_will_be_visible", c.GetResponseTag(_maleMajor, _female));
		}

		[TestMethod]
		public void Stripping_Lower_Male_AsImportant()
		{
			Case c = new Case("stripping");
			c.Stages.Add(4);
			Assert.AreEqual("male_crotch_will_be_visible", c.GetResponseTag(_maleMajor, _female));
		}

		[TestMethod]
		public void Stripped_Extra_Female()
		{
			Case c = new Case("stripped");
			c.Stages.Add(1);
			Assert.AreEqual("female_removed_accessory", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Minor_Female()
		{
			Case c = new Case("stripped");
			c.Stages.Add(2);
			Assert.AreEqual("female_removed_minor", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Major_Female()
		{
			Case c = new Case("stripped");
			c.Stages.Add(3);
			Assert.AreEqual("female_removed_major", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Upper_Large_Female()
		{
			_female.Size = "large";
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("female_large_chest_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Upper_Medium_Female()
		{
			_female.Size = "medium";
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("female_medium_chest_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Upper_Female()
		{
			_female.Size = "small";
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("female_small_chest_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Lower_Female()
		{
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("female_crotch_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Upper_Large_Female_AsImportant()
		{
			_femaleMajor.Size = "large";
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("female_large_chest_is_visible", c.GetResponseTag(_femaleMajor, _male));
		}

		[TestMethod]
		public void Stripped_Upper_Medium_Female_AsImportant()
		{
			_femaleMajor.Size = "medium";
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("female_medium_chest_is_visible", c.GetResponseTag(_femaleMajor, _male));
		}

		[TestMethod]
		public void Stripped_Upper_Female_AsImportant()
		{
			_femaleMajor.Size = "small";
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("female_small_chest_is_visible", c.GetResponseTag(_femaleMajor, _male));
		}

		[TestMethod]
		public void Stripped_Lower_Female_AsImportant()
		{
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("female_crotch_is_visible", c.GetResponseTag(_femaleMajor, _male));
		}

		[TestMethod]
		public void Stripped_Extra_Male()
		{
			Case c = new Case("stripped");
			c.Stages.Add(1);
			Assert.AreEqual("male_removed_accessory", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripped_Minor_Male()
		{
			Case c = new Case("stripped");
			c.Stages.Add(2);
			Assert.AreEqual("male_removed_minor", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripped_Major_Male()
		{
			Case c = new Case("stripped");
			c.Stages.Add(3);
			Assert.AreEqual("male_removed_major", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripped_Upper_Male()
		{
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("male_chest_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Large_Male()
		{
			_male.Size = "large";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("male_large_crotch_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Medium_Male()
		{
			_male.Size = "medium";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("male_medium_crotch_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Small_Male()
		{
			_male.Size = "small";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("male_small_crotch_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Stripped_Upper_Male_AsImportant()
		{
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("male_chest_is_visible", c.GetResponseTag(_maleMajor, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Large_Male_AsImportant()
		{
			_maleMajor.Size = "large";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("male_large_crotch_is_visible", c.GetResponseTag(_maleMajor, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Medium_Male_AsImportant()
		{
			_maleMajor.Size = "medium";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("male_medium_crotch_is_visible", c.GetResponseTag(_maleMajor, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Small_Male_AsImportant()
		{
			_maleMajor.Size = "small";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("male_small_crotch_is_visible", c.GetResponseTag(_maleMajor, _female));
		}

		[TestMethod]
		public void OpponentLost_Is_Target()
		{
			Case c = new Case("opponent_lost");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("must_strip", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void OpponentLost_Is_Not_Target()
		{
			Case c = new Case("opponent_lost");
			Assert.AreEqual("opponent_lost", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripping_Opponent_Is_Target()
		{
			Case c = new Case("opponent_stripping");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripping_Opponent_Is_Not_Target()
		{
			Case c = new Case("opponent_stripping");
			Assert.AreEqual("opponent_stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Opponent_Is_Target()
		{
			Case c = new Case("opponent_stripped");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Opponent_Is_Not_Target()
		{
			Case c = new Case("opponent_stripped");
			Assert.AreEqual("opponent_stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Stripped_Extra_Opponent()
		{
			Case c = new Case("stripped");
			c.Stages.Add(1);
			Assert.AreEqual("opponent_stripped", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Stripped_Minor_Opponent()
		{
			Case c = new Case("stripped");
			c.Stages.Add(2);
			Assert.AreEqual("opponent_stripped", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Stripped_Major_Opponent()
		{
			Case c = new Case("stripped");
			c.Stages.Add(3);
			Assert.AreEqual("opponent_stripped", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Stripped_Upper_Opponent()
		{
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("opponent_chest_is_visible", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Opponent()
		{
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bifemale, _male));
		}

		[TestMethod]
		public void Stripped_Lower_Large_Opponent()
		{
			_bimale.Size = "large";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Medium_Opponent()
		{
			_bimale.Size = "medium";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Small_Opponent()
		{
			_bimale.Size = "small";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Stripped_Upper_Opponent_AsImportant()
		{
			Case c = new Case("stripped");
			c.Stages.Add(4);
			Assert.AreEqual("opponent_chest_is_visible", c.GetResponseTag(_bimaleMajor, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Opponent_AsImportant()
		{
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bifemaleMajor, _male));
		}

		[TestMethod]
		public void Stripped_Lower_Large_Opponent_AsImportant()
		{
			_bimaleMajor.Size = "large";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bimaleMajor, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Medium_Opponent_AsImportant()
		{
			_bimaleMajor.Size = "medium";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bimaleMajor, _female));
		}

		[TestMethod]
		public void Stripped_Lower_Small_Opponent_AsImportant()
		{
			_bimaleMajor.Size = "small";
			Case c = new Case("stripped");
			c.Stages.Add(5);
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_bimaleMajor, _female));
		}

		[TestMethod]
		public void MustMasturbateFirst_Female()
		{
			Case c = new Case("must_masturbate_first");
			Assert.AreEqual("female_must_masturbate", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void MustMasturbate_Female()
		{
			Case c = new Case("must_masturbate");
			Assert.AreEqual("female_must_masturbate", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void StartMasturbating_Female()
		{
			Case c = new Case("start_masturbating");
			Assert.AreEqual("female_start_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Masturbating_Female()
		{
			Case c = new Case("masturbating");
			Assert.AreEqual("female_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Heavy_Masturbating_Female()
		{
			Case c = new Case("heavy_masturbating");
			Assert.AreEqual("female_heavy_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Finished_Masturbating_Female()
		{
			Case c = new Case("finished_masturbating");
			Assert.AreEqual("female_finished_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void After_Masturbating_Female()
		{
			Case c = new Case("after_masturbating");
			Assert.AreEqual("hand", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void MustMasturbateFirst_Male()
		{
			Case c = new Case("must_masturbate_first");
			Assert.AreEqual("male_must_masturbate", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void MustMasturbate_Male()
		{
			Case c = new Case("must_masturbate");
			Assert.AreEqual("male_must_masturbate", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void StartMasturbating_Male()
		{
			Case c = new Case("start_masturbating");
			Assert.AreEqual("male_start_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Masturbating_Male()
		{
			Case c = new Case("masturbating");
			Assert.AreEqual("male_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Heavy_Masturbating_Male()
		{
			Case c = new Case("heavy_masturbating");
			Assert.AreEqual("male_heavy_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Finished_Masturbating_Male()
		{
			Case c = new Case("finished_masturbating");
			Assert.AreEqual("male_finished_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void MustMasturbateFirst_Opponent()
		{
			Case c = new Case("must_masturbate_first");
			Assert.AreEqual("opponent_lost", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void MustMasturbate_Opponent()
		{
			Case c = new Case("must_masturbate");
			Assert.AreEqual("opponent_lost", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void StartMasturbating_Opponent()
		{
			Case c = new Case("start_masturbating");
			Assert.AreEqual("opponent_start_masturbating", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Masturbating_Opponent()
		{
			Case c = new Case("masturbating");
			Assert.AreEqual("opponent_masturbating", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Heavy_Masturbating_Opponent()
		{
			Case c = new Case("heavy_masturbating");
			Assert.AreEqual("opponent_heavy_masturbating", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void Finished_Masturbating_Opponent()
		{
			Case c = new Case("finished_masturbating");
			Assert.AreEqual("opponent_finished_masturbating", c.GetResponseTag(_bimale, _female));
		}

		[TestMethod]
		public void GameOver_Victory()
		{
			Case c = new Case("game_over_victory");
			Assert.AreEqual("game_over_defeat", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void GameOver_Defeat()
		{
			Case c = new Case("game_over_defeat");
			Assert.AreEqual("game_over_defeat", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void GameOver_Defeat_TargetingWinner()
		{
			Case c = new Case("game_over_defeat");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("game_over_victory", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Hand_Good()
		{
			Case c = new Case("good_hand");
			Assert.AreEqual("hand", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Hand_Bad()
		{
			Case c = new Case("bad_hand");
			Assert.AreEqual("hand", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Hand_Okay()
		{
			Case c = new Case("okay_hand");
			Assert.AreEqual("hand", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Hand_Any()
		{
			Case c = new Case("hand");
			Assert.AreEqual("hand", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_MustStrip_Other()
		{
			Case c = new Case("male_must_strip");
			Assert.AreEqual("male_must_strip", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_MustStrip_Target()
		{
			Case c = new Case("male_must_strip");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("must_strip", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingAccessory_Other()
		{
			Case c = new Case("male_removing_accessory");
			Assert.AreEqual("male_removing_accessory", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingAccessory_Target()
		{
			Case c = new Case("male_removing_accessory");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingMinor_Other()
		{
			Case c = new Case("male_removing_minor");
			Assert.AreEqual("male_removing_minor", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingMinor_Target()
		{
			Case c = new Case("male_removing_minor");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingMajor_Other()
		{
			Case c = new Case("male_removing_major");
			Assert.AreEqual("male_removing_major", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingMajor_Target()
		{
			Case c = new Case("male_removing_major");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_MustStrip_Other()
		{
			Case c = new Case("female_must_strip");
			Assert.AreEqual("female_must_strip", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_MustStrip_Target()
		{
			Case c = new Case("female_must_strip");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("must_strip", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingAccessory_Other()
		{
			Case c = new Case("female_removing_accessory");
			Assert.AreEqual("female_removing_accessory", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingAccessory_Target()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingMinor_Other()
		{
			Case c = new Case("female_removing_minor");
			Assert.AreEqual("female_removing_minor", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingMinor_Target()
		{
			Case c = new Case("female_removing_minor");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingMajor_Other()
		{
			Case c = new Case("female_removing_major");
			Assert.AreEqual("female_removing_major", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingMajor_Target()
		{
			Case c = new Case("female_removing_major");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_ChestWillBeVisible_Other()
		{
			Case c = new Case("male_chest_will_be_visible");
			Assert.AreEqual("male_chest_will_be_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_ChestWillBeVisible_Target()
		{
			Case c = new Case("male_chest_will_be_visible");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_ChestWillBeVisible_Other()
		{
			Case c = new Case("female_chest_will_be_visible");
			Assert.AreEqual("female_chest_will_be_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_ChestWillBeVisible_Target()
		{
			Case c = new Case("female_chest_will_be_visible");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_ChestIsVisible_Other()
		{
			Case c = new Case("male_chest_is_visible");
			Assert.AreEqual("male_chest_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_ChestIsVisible_Target()
		{
			Case c = new Case("male_chest_is_visible");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_SmallChestIsVisible_Other()
		{
			Case c = new Case("female_small_chest_is_visible");
			Assert.AreEqual("female_small_chest_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_SmallChestIsVisible_Target()
		{
			Case c = new Case("female_small_chest_is_visible");
			c.LegacyTarget = _female.FolderName;
			_female.Size = "small";
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_MediumChestIsVisible_Other()
		{
			Case c = new Case("female_medium_chest_is_visible");
			Assert.AreEqual("female_medium_chest_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_MediumChestIsVisible_Target()
		{
			Case c = new Case("female_medium_chest_is_visible");
			c.LegacyTarget = _female.FolderName;
			_female.Size = "medium";
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_LargeChestIsVisible_Other()
		{
			Case c = new Case("female_large_chest_is_visible");
			Assert.AreEqual("female_large_chest_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_LargeChestIsVisible_Target()
		{
			Case c = new Case("female_large_chest_is_visible");
			c.LegacyTarget = _female.FolderName;
			_female.Size = "large";
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_ChestIsVisible_Other()
		{
			Case c = new Case("female_chest_is_visible");
			Assert.AreEqual("female_chest_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_ChestIsVisible_Target()
		{
			Case c = new Case("female_chest_is_visible");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_ChestIsVisible_Other()
		{
			Case c = new Case("opponent_chest_is_visible");
			Assert.AreEqual("opponent_chest_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Opponent_ChestIsVisible_Target()
		{
			Case c = new Case("opponent_chest_is_visible");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_CrotchWillBeVisible_Other()
		{
			Case c = new Case("male_crotch_will_be_visible");
			Assert.AreEqual("male_crotch_will_be_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_CrotchWillBeVisible_Target()
		{
			Case c = new Case("male_crotch_will_be_visible");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_CrotchWillBeVisible_Other()
		{
			Case c = new Case("female_crotch_will_be_visible");
			Assert.AreEqual("female_crotch_will_be_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_CrotchWillBeVisible_Target()
		{
			Case c = new Case("female_crotch_will_be_visible");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_CrotchWillBeVisible_Other()
		{
			Case c = new Case("opponent_crotch_will_be_visible");
			Assert.AreEqual("opponent_crotch_will_be_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Opponent_CrotchWillBeVisible_Target()
		{
			Case c = new Case("opponent_crotch_will_be_visible");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_CrotchIsVisible_Other()
		{
			Case c = new Case("female_crotch_is_visible");
			Assert.AreEqual("female_crotch_is_visible", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_CrotchIsVisible_Target()
		{
			Case c = new Case("female_crotch_is_visible");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_SmallCrotchIsVisible_Other()
		{
			Case c = new Case("male_small_crotch_is_visible");
			Assert.AreEqual("male_small_crotch_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_SmallCrotchIsVisible_Target()
		{
			Case c = new Case("male_small_crotch_is_visible");
			c.LegacyTarget = _male.FolderName;
			_male.Size = "small";
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_MediumCrotchIsVisible_Other()
		{
			Case c = new Case("male_medium_crotch_is_visible");
			Assert.AreEqual("male_medium_crotch_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_MediumCrotchIsVisible_Target()
		{
			Case c = new Case("male_medium_crotch_is_visible");
			c.LegacyTarget = _male.FolderName;
			_male.Size = "medium";
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_LargeCrotchIsVisible_Other()
		{
			Case c = new Case("male_large_crotch_is_visible");
			Assert.AreEqual("male_large_crotch_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_LargeCrotchIsVisible_Target()
		{
			Case c = new Case("male_large_crotch_is_visible");
			c.LegacyTarget = _male.FolderName;
			_male.Size = "large";
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_CrotchIsVisible_Other()
		{
			Case c = new Case("male_crotch_is_visible");
			Assert.AreEqual("male_crotch_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_CrotchIsVisible_Target()
		{
			Case c = new Case("male_crotch_is_visible");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Opponent_CrotchIsVisible_Other()
		{
			Case c = new Case("opponent_crotch_is_visible");
			Assert.AreEqual("opponent_crotch_is_visible", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Opponent_CrotchIsVisible_Target()
		{
			Case c = new Case("opponent_crotch_is_visible");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_MustMasturbate_Other()
		{
			Case c = new Case("male_must_masturbate");
			Assert.AreEqual("male_must_masturbate", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_MustMasturbate_Target()
		{
			Case c = new Case("male_must_masturbate");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("must_masturbate", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_MustMasturbate_Other()
		{
			Case c = new Case("female_must_masturbate");
			Assert.AreEqual("female_must_masturbate", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_MustMasturbate_Target()
		{
			Case c = new Case("female_must_masturbate");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("must_masturbate", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_StartMasturbating_Other()
		{
			Case c = new Case("male_start_masturbating");
			Assert.AreEqual("male_start_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_StartMasturbating_Target()
		{
			Case c = new Case("male_start_masturbating");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("start_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_StartMasturbating_Other()
		{
			Case c = new Case("female_start_masturbating");
			Assert.AreEqual("female_start_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_StartMasturbating_Target()
		{
			Case c = new Case("female_start_masturbating");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("start_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_StartMasturbating_Other()
		{
			Case c = new Case("opponent_start_masturbating");
			Assert.AreEqual("opponent_start_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Opponent_StartMasturbating_Target()
		{
			Case c = new Case("opponent_start_masturbating");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("start_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_Masturbating_Other()
		{
			Case c = new Case("male_masturbating");
			Assert.AreEqual("male_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_Masturbating_Target()
		{
			Case c = new Case("male_masturbating");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_Masturbating_Other()
		{
			Case c = new Case("female_masturbating");
			Assert.AreEqual("female_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_Masturbating_Target()
		{
			Case c = new Case("female_masturbating");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Masturbating_Other()
		{
			Case c = new Case("opponent_masturbating");
			Assert.AreEqual("opponent_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Masturbating_Target()
		{
			Case c = new Case("opponent_masturbating");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_Heavy_Masturbating_Other()
		{
			Case c = new Case("male_heavy_masturbating");
			Assert.AreEqual("male_heavy_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_Heavy_Masturbating_Target()
		{
			Case c = new Case("male_heavy_masturbating");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("heavy_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_Heavy_Masturbating_Other()
		{
			Case c = new Case("female_heavy_masturbating");
			Assert.AreEqual("female_heavy_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_Heavy_Masturbating_Target()
		{
			Case c = new Case("female_heavy_masturbating");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("heavy_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Heavy_Masturbating_Other()
		{
			Case c = new Case("opponent_heavy_masturbating");
			Assert.AreEqual("opponent_heavy_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Heavy_Masturbating_Target()
		{
			Case c = new Case("opponent_heavy_masturbating");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("heavy_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_Finished_Masturbating_Other()
		{
			Case c = new Case("male_finished_masturbating");
			Assert.AreEqual("male_finished_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_Finished_Masturbating_Target()
		{
			Case c = new Case("male_finished_masturbating");
			c.LegacyTarget = _male.FolderName;
			Assert.AreEqual("finished_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_Finished_Masturbating_Other()
		{
			Case c = new Case("female_finished_masturbating");
			Assert.AreEqual("female_finished_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_Finished_Masturbating_Target()
		{
			Case c = new Case("female_finished_masturbating");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("finished_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Finished_Masturbating_Other()
		{
			Case c = new Case("opponent_finished_masturbating");
			Assert.AreEqual("opponent_finished_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Finished_Masturbating_Target()
		{
			Case c = new Case("opponent_finished_masturbating");
			c.LegacyTarget = _female.FolderName;
			Assert.AreEqual("finished_masturbating", c.GetResponseTag(_male, _female));
		}

		#region Role targets
		[TestMethod]
		public void Male_MustStrip_RoleTarget()
		{
			Case c = new Case("male_must_strip");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("must_strip", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingAccessory_RoleTarget()
		{
			Case c = new Case("male_removing_accessory");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingMinor_RoleTarget()
		{
			Case c = new Case("male_removing_minor");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_RemovingMajor_RoleTarget()
		{
			Case c = new Case("male_removing_major");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_MustStrip_RoleTarget()
		{
			Case c = new Case("female_must_strip");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("must_strip", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingAccessory_RoleTarget()
		{
			Case c = new Case("female_removing_accessory");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingMinor_RoleTarget()
		{
			Case c = new Case("female_removing_minor");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_RemovingMajor_RoleTarget()
		{
			Case c = new Case("female_removing_major");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_ChestWillBeVisible_RoleTarget()
		{
			Case c = new Case("male_chest_will_be_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_ChestWillBeVisible_RoleTarget()
		{
			Case c = new Case("female_chest_will_be_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_ChestIsVisible_RoleTarget()
		{
			Case c = new Case("male_chest_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_SmallChestIsVisible_RoleTarget()
		{
			Case c = new Case("female_small_chest_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			_female.Size = "small";
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_MediumChestIsVisible_RoleTarget()
		{
			Case c = new Case("female_medium_chest_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			_female.Size = "medium";
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_LargeChestIsVisible_RoleTarget()
		{
			Case c = new Case("female_large_chest_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			_female.Size = "large";
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Female_ChestIsVisible_RoleTarget()
		{
			Case c = new Case("female_chest_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_ChestIsVisible_RoleTarget()
		{
			Case c = new Case("opponent_chest_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_CrotchWillBeVisible_RoleTarget()
		{
			Case c = new Case("male_crotch_will_be_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_CrotchWillBeVisible_RoleTarget()
		{
			Case c = new Case("female_crotch_will_be_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_CrotchWillBeVisible_RoleTarget()
		{
			Case c = new Case("opponent_crotch_will_be_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripping", c.GetResponseTag(_male, _female));
		}
		[TestMethod]
		public void Female_CrotchIsVisible_RoleTarget()
		{
			Case c = new Case("female_crotch_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("stripped", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_SmallCrotchIsVisible_RoleTarget()
		{
			Case c = new Case("male_small_crotch_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			_male.Size = "small";
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_MediumCrotchIsVisible_RoleTarget()
		{
			Case c = new Case("male_medium_crotch_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			_male.Size = "medium";
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_LargeCrotchIsVisible_RoleTarget()
		{
			Case c = new Case("male_large_crotch_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			_male.Size = "large";
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_CrotchIsVisible_RoleTarget()
		{
			Case c = new Case("male_crotch_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Opponent_CrotchIsVisible_RoleTarget()
		{
			Case c = new Case("opponent_crotch_is_visible");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("stripped", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_MustMasturbate_RoleTarget()
		{
			Case c = new Case("male_must_masturbate");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("must_masturbate", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_MustMasturbate_RoleTarget()
		{
			Case c = new Case("female_must_masturbate");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("must_masturbate", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_StartMasturbating_RoleTarget()
		{
			Case c = new Case("male_start_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("start_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_StartMasturbating_RoleTarget()
		{
			Case c = new Case("female_start_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("start_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_StartMasturbating_RoleTarget()
		{
			Case c = new Case("opponent_start_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("start_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Male_Masturbating_RoleTarget()
		{
			Case c = new Case("male_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_Masturbating_RoleTarget()
		{
			Case c = new Case("female_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Masturbating_RoleTarget()
		{
			Case c = new Case("opponent_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_Heavy_Masturbating_RoleTarget()
		{
			Case c = new Case("male_heavy_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("heavy_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_Heavy_Masturbating_RoleTarget()
		{
			Case c = new Case("female_heavy_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("heavy_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Heavy_Masturbating_RoleTarget()
		{
			Case c = new Case("opponent_heavy_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("heavy_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Male_Finished_Masturbating_RoleTarget()
		{
			Case c = new Case("male_finished_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _male.FolderName, Role = "target" });
			Assert.AreEqual("finished_masturbating", c.GetResponseTag(_female, _male));
		}

		[TestMethod]
		public void Female_Finished_Masturbating_RoleTarget()
		{
			Case c = new Case("female_finished_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("finished_masturbating", c.GetResponseTag(_male, _female));
		}

		[TestMethod]
		public void Opponent_Finished_Masturbating_RoleTarget()
		{
			Case c = new Case("opponent_finished_masturbating");
			c.Conditions.Add(new TargetCondition() { Character = _female.FolderName, Role = "target" });
			Assert.AreEqual("finished_masturbating", c.GetResponseTag(_male, _female));
		}
		#endregion

		[TestMethod]
		public void StageLimit_RemovingExtra()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(0, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_RemovedExtra()
		{
			Case c = new Case("female_removed_accessory");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(1, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_RemovingMinor()
		{
			Case c = new Case("female_removing_minor");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(1, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_RemovedMinor()
		{
			Case c = new Case("female_removed_minor");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(2, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_RemovingMajor()
		{
			Case c = new Case("female_removing_major");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(2, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_RemovedMajor()
		{
			Case c = new Case("female_removed_major");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(3, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_ChestWillBeVisible()
		{
			Case c = new Case("female_chest_will_be_visible");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(3, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_ChestIsVisible()
		{
			Case c = new Case("female_large_chest_is_visible");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(4, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_CrotchWillBeVisible()
		{
			Case c = new Case("female_crotch_will_be_visible");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(4, response.Stages[0]);
		}

		[TestMethod]
		public void StageLimit_CrotchIsVisible()
		{
			Case c = new Case("female_crotch_is_visible");
			c.LegacyTarget = _female.FolderName;
			Case response = c.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Stages.Count);
			Assert.AreEqual(5, response.Stages[0]);
		}

		#region Testing the various response transfers. Too lazy to make individual property tests, so doing everything in bulk for all major combinations
		[TestMethod]
		public void NotTargetable_NoTarget_WithAlsoPlaying()
		{
			Case c = new Case("good_hand");
			c.LegacyAlsoPlaying = "other";
			Case response = c.CreateResponse(_male, _female);
			Assert.IsTrue(response.Conditions.Count == 1);
			Assert.AreEqual("other", response.Conditions[0].FilterTag);
		}

		[TestMethod]
		public void Targetable_NoTarget_WithAlsoPlaying()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyAlsoPlaying = "other";
			Case response = c.CreateResponse(_male, _female);
			Assert.IsTrue(response.Conditions.Count == 1);
			Assert.AreEqual("other", response.Conditions[0].FilterTag);
		}

		[TestMethod]
		public void Targetable_TargetResponder_WithAlsoPlaying()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTarget = _female.FolderName;
			c.LegacyAlsoPlaying = "other";
			Case response = c.CreateResponse(_male, _female);
			Assert.IsTrue(response.Conditions.Count == 1);
			Assert.AreEqual("other", response.Conditions[0].FilterTag);
		}

		[TestMethod]
		public void Targetable_Target_WithAlsoPlaying()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTarget = "other1";
			c.LegacyAlsoPlaying = "other2";
			Case response = c.CreateResponse(_male, _female);
			Assert.IsTrue(response.Conditions.Count == 1);
			Assert.AreEqual("other2", response.Conditions[0].FilterTag);
		}

		[TestMethod]
		public void Targetable_NoTarget_NoAlsoPlaying()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTotalExposed = "5";
			c.LegacyTotalFemales = "4";
			c.LegacyTotalMales = "3-5";
			c.LegacyTotalFinished = "2";
			Case response = c.CreateResponse(_male, _female);

			Assert.AreEqual(_female.Layers + Clothing.ExtraStages, response.Stages.Count);
			Assert.AreEqual(_male.FolderName, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyTotalFinished, response.LegacyTotalFinished);
			Assert.AreEqual(c.LegacyTotalFemales, response.LegacyTotalFemales);
			Assert.AreEqual(c.LegacyTotalMales, response.LegacyTotalMales);
			Assert.AreEqual(c.LegacyTotalExposed, response.LegacyTotalExposed);
		}

		[TestMethod]
		public void NotTargetable_NoTarget_NoAlsoPlaying()
		{
			Case c = new Case("good_hand");
			c.LegacyTotalExposed = "5";
			c.LegacyTotalFemales = "4";
			c.LegacyTotalMales = "3-5";
			c.LegacyTotalFinished = "2";
			Case response = c.CreateResponse(_male, _female);

			Assert.AreEqual(_female.Layers + 1, response.Stages.Count);
			Assert.AreEqual(_male.FolderName, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyTotalFinished, response.LegacyTotalFinished);
			Assert.AreEqual(c.LegacyTotalFemales, response.LegacyTotalFemales);
			Assert.AreEqual(c.LegacyTotalMales, response.LegacyTotalMales);
			Assert.AreEqual(c.LegacyTotalExposed, response.LegacyTotalExposed);
		}

		[TestMethod]
		public void Targetable_NoTarget_AlsoPlayingResponder()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTimeInStage = "5";
			c.Stages.Add(2);
			c.Stages.Add(3);
			c.LegacySaidMarker = "foo";
			c.LegacyNotSaidMarker = "bar";
			c.LegacyHasHand = "hand";

			c.LegacyAlsoPlaying = _female.FolderName;
			c.LegacyAlsoPlayingHand = "good";
			c.LegacyAlsoPlayingStage = "2-4";
			c.LegacyAlsoPlayingSaidMarker = "foo2";
			c.LegacyAlsoPlayingNotSaidMarker = "bar2";
			c.LegacyAlsoPlayingTimeInStage = "5";

			Case response = c.CreateResponse(_male, _female);

			Assert.AreEqual(_male.FolderName, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyAlsoPlayingTimeInStage);
			Assert.AreEqual("2-3", response.LegacyAlsoPlayingStage);
			Assert.AreEqual(c.LegacySaidMarker, response.LegacyAlsoPlayingSaidMarker);
			Assert.AreEqual(c.LegacyNotSaidMarker, response.LegacyAlsoPlayingNotSaidMarker);
			Assert.AreEqual(c.LegacyHasHand, response.LegacyAlsoPlayingHand);

			Assert.AreEqual(3, response.Stages.Count);
			Assert.IsTrue(response.Stages.Contains(2));
			Assert.IsTrue(response.Stages.Contains(3));
			Assert.IsTrue(response.Stages.Contains(4));
			Assert.AreEqual(c.LegacyAlsoPlayingHand, response.LegacyHasHand);
			Assert.AreEqual(c.LegacyAlsoPlayingSaidMarker, response.LegacySaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingNotSaidMarker, response.LegacyNotSaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingTimeInStage, response.LegacyTimeInStage);
		}

		[TestMethod]
		public void NotTargetable_NoTarget_AlsoPlayingResponder()
		{
			Case c = new Case("good_hand");
			c.LegacyTimeInStage = "5";
			c.Stages.Add(2);
			c.Stages.Add(3);
			c.LegacySaidMarker = "foo";
			c.LegacyNotSaidMarker = "bar";
			c.LegacyHasHand = "hand";

			c.LegacyAlsoPlaying = _female.FolderName;
			c.LegacyAlsoPlayingHand = "good";
			c.LegacyAlsoPlayingStage = "2-4";
			c.LegacyAlsoPlayingSaidMarker = "foo2";
			c.LegacyAlsoPlayingNotSaidMarker = "bar2";
			c.LegacyAlsoPlayingTimeInStage = "5";

			Case response = c.CreateResponse(_male, _female);

			Assert.AreEqual(_male.FolderName, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyAlsoPlayingTimeInStage);
			Assert.AreEqual("2-3", response.LegacyAlsoPlayingStage);
			Assert.AreEqual(c.LegacySaidMarker, response.LegacyAlsoPlayingSaidMarker);
			Assert.AreEqual(c.LegacyNotSaidMarker, response.LegacyAlsoPlayingNotSaidMarker);
			Assert.AreEqual(c.LegacyHasHand, response.LegacyAlsoPlayingHand);

			Assert.AreEqual(3, response.Stages.Count);
			Assert.IsTrue(response.Stages.Contains(2));
			Assert.IsTrue(response.Stages.Contains(3));
			Assert.IsTrue(response.Stages.Contains(4));
			Assert.AreEqual(c.LegacyAlsoPlayingHand, response.LegacyHasHand);
			Assert.AreEqual(c.LegacyAlsoPlayingSaidMarker, response.LegacySaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingNotSaidMarker, response.LegacyNotSaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingTimeInStage, response.LegacyTimeInStage);
		}

		[TestMethod]
		public void NotTargetable_TargetableResponse_NoAlsoPlaying()
		{
			Case c = new Case("must_strip_normal");
			c.LegacyTimeInStage = "5";
			c.Stages.Add(2);
			c.Stages.Add(3);
			c.LegacySaidMarker = "foo";
			c.LegacyNotSaidMarker = "bar";
			c.LegacyHasHand = "hand";

			Case response = c.CreateResponse(_male, _female);

			Assert.AreEqual(_male.FolderName, response.LegacyTarget);
			Assert.AreEqual(_female.Layers + Clothing.ExtraStages, response.Stages.Count);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyTargetTimeInStage);
			Assert.AreEqual("2-3", response.LegacyTargetStage);
			Assert.AreEqual(c.LegacySaidMarker, response.LegacyTargetSaidMarker);
			Assert.AreEqual(c.LegacyNotSaidMarker, response.LegacyTargetNotSaidMarker);
			Assert.AreEqual(c.LegacyHasHand, response.LegacyTargetHand);

			Assert.AreEqual(_female.Layers + Clothing.ExtraStages, response.Stages.Count);
			Assert.IsNull(response.LegacyAlsoPlaying);
		}

		[TestMethod]
		public void NotTargetable_TargetableResponse_WithAlsoPlaying()
		{
			Case c = new Case("must_strip_normal");
			c.LegacyTimeInStage = "5";
			c.Stages.Add(2);
			c.Stages.Add(3);
			c.LegacySaidMarker = "foo";
			c.LegacyNotSaidMarker = "bar";
			c.LegacyHasHand = "hand";

			c.LegacyAlsoPlaying = "other";
			c.LegacyAlsoPlayingHand = "good";
			c.LegacyAlsoPlayingStage = "2-4";
			c.LegacyAlsoPlayingSaidMarker = "foo2";
			c.LegacyAlsoPlayingNotSaidMarker = "bar2";
			c.LegacyAlsoPlayingTimeInStage = "5";

			Case response = c.CreateResponse(_male, _female);

			Assert.AreEqual(_male.FolderName, response.LegacyTarget);
			Assert.AreEqual(_female.Layers + Clothing.ExtraStages, response.Stages.Count);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyTargetTimeInStage);
			Assert.AreEqual("2-3", response.LegacyTargetStage);
			Assert.AreEqual(c.LegacySaidMarker, response.LegacyTargetSaidMarker);
			Assert.AreEqual(c.LegacyNotSaidMarker, response.LegacyTargetNotSaidMarker);
			Assert.AreEqual(c.LegacyHasHand, response.LegacyTargetHand);

			Assert.AreEqual(c.LegacyAlsoPlaying, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyAlsoPlayingStage, response.LegacyAlsoPlayingStage);
			Assert.AreEqual(c.LegacyAlsoPlayingHand, response.LegacyAlsoPlayingHand);
			Assert.AreEqual(c.LegacyAlsoPlayingSaidMarker, response.LegacyAlsoPlayingSaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingNotSaidMarker, response.LegacyAlsoPlayingNotSaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingTimeInStage, response.LegacyAlsoPlayingTimeInStage);
		}

		[TestMethod]
		public void Targetable_Target_NoAlsoPlaying()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTimeInStage = "5";
			c.Stages.Add(2);
			c.Stages.Add(3);
			c.LegacySaidMarker = "foo";
			c.LegacyNotSaidMarker = "bar";
			c.LegacyHasHand = "hand";

			c.LegacyTarget = "other";
			c.LegacyTargetHand = "good";
			c.LegacyTargetStage = "2-4";
			c.LegacyTargetSaidMarker = "foo2";
			c.LegacyTargetNotSaidMarker = "bar2";
			c.LegacyTargetTimeInStage = "5";
			c.LegacyTargetStatus = "blah";
			c.LegacyTargetLayers = "2-4";
			c.LegacyTargetStartingLayers = "1-3";

			Case response = c.CreateResponse(_male, _female);

			Assert.AreEqual(_female.Layers + Clothing.ExtraStages, response.Stages.Count);

			Assert.AreEqual(_male.FolderName, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyAlsoPlayingTimeInStage);
			Assert.AreEqual("2-3", response.LegacyAlsoPlayingStage);
			Assert.AreEqual(c.LegacySaidMarker, response.LegacyAlsoPlayingSaidMarker);
			Assert.AreEqual(c.LegacyNotSaidMarker, response.LegacyAlsoPlayingNotSaidMarker);
			Assert.AreEqual(c.LegacyHasHand, response.LegacyAlsoPlayingHand);

			Assert.AreEqual(c.LegacyTarget, response.LegacyTarget);
			Assert.AreEqual(c.LegacyTargetStage, response.LegacyTargetStage);
			Assert.AreEqual(c.LegacyTargetHand, response.LegacyTargetHand);
			Assert.AreEqual(c.LegacyTargetSaidMarker, response.LegacyTargetSaidMarker);
			Assert.AreEqual(c.LegacyTargetNotSaidMarker, response.LegacyTargetNotSaidMarker);
			Assert.AreEqual(c.LegacyTargetTimeInStage, response.LegacyTargetTimeInStage);
			Assert.AreEqual(c.LegacyTargetStatus, response.LegacyTargetStatus);
			Assert.AreEqual(c.LegacyTargetLayers, response.LegacyTargetLayers);
			Assert.AreEqual(c.LegacyTargetStartingLayers, response.LegacyTargetStartingLayers);
		}

		[TestMethod]
		public void Targetable_TargetResponder_NoAlsoPlaying()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTimeInStage = "5";
			c.Stages.Add(2);
			c.Stages.Add(3);
			c.LegacySaidMarker = "foo";
			c.LegacyNotSaidMarker = "bar";
			c.LegacyHasHand = "hand";

			c.LegacyTarget = _female.FolderName;
			c.LegacyTargetHand = "good";
			c.LegacyTargetStage = "2-4";
			c.LegacyTargetSaidMarker = "foo2";
			c.LegacyTargetNotSaidMarker = "bar2";
			c.LegacyTargetTimeInStage = "5";
			c.LegacyTargetStatus = "blah";
			c.LegacyTargetLayers = "2-4";
			c.LegacyTargetStartingLayers = "1-3";

			Case response = c.CreateResponse(_male, _female);
			
			Assert.AreEqual(3, response.Stages.Count);
			Assert.IsTrue(response.Stages.Contains(2));
			Assert.IsTrue(response.Stages.Contains(3));
			Assert.IsTrue(response.Stages.Contains(4));
			Assert.AreEqual(c.LegacyTargetHand, response.LegacyHasHand);
			Assert.AreEqual(c.LegacyTargetSaidMarker, response.LegacySaidMarker);
			Assert.AreEqual(c.LegacyTargetNotSaidMarker, response.LegacyNotSaidMarker);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyTimeInStage);

			Assert.AreEqual(_male.FolderName, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyAlsoPlayingTimeInStage);
			Assert.AreEqual("2-3", response.LegacyAlsoPlayingStage);
			Assert.AreEqual(c.LegacySaidMarker, response.LegacyAlsoPlayingSaidMarker);
			Assert.AreEqual(c.LegacyNotSaidMarker, response.LegacyAlsoPlayingNotSaidMarker);
			Assert.AreEqual(c.LegacyHasHand, response.LegacyAlsoPlayingHand);
		}

		[TestMethod]
		public void Targetable_Target_AlsoPlayingResponder()
		{
			Case c = new Case("female_removing_accessory");
			c.LegacyTimeInStage = "5";
			c.Stages.Add(2);
			c.Stages.Add(3);
			c.LegacySaidMarker = "foo";
			c.LegacyNotSaidMarker = "bar";
			c.LegacyHasHand = "hand";

			c.LegacyTarget = "other";
			c.LegacyTargetHand = "good";
			c.LegacyTargetStage = "2-4";
			c.LegacyTargetSaidMarker = "foo2";
			c.LegacyTargetNotSaidMarker = "bar2";
			c.LegacyTargetTimeInStage = "5";
			c.LegacyTargetStatus = "blah";
			c.LegacyTargetLayers = "2-4";
			c.LegacyTargetStartingLayers = "1-3";

			c.LegacyAlsoPlaying = _female.FolderName;
			c.LegacyAlsoPlayingHand = "good";
			c.LegacyAlsoPlayingStage = "2-4";
			c.LegacyAlsoPlayingSaidMarker = "foo2";
			c.LegacyAlsoPlayingNotSaidMarker = "bar2";
			c.LegacyAlsoPlayingTimeInStage = "5";

			Case response = c.CreateResponse(_male, _female);
			
			Assert.AreEqual(_male.FolderName, response.LegacyAlsoPlaying);
			Assert.AreEqual(c.LegacyTimeInStage, response.LegacyAlsoPlayingTimeInStage);
			Assert.AreEqual("2-3", response.LegacyAlsoPlayingStage);
			Assert.AreEqual(c.LegacySaidMarker, response.LegacyAlsoPlayingSaidMarker);
			Assert.AreEqual(c.LegacyNotSaidMarker, response.LegacyAlsoPlayingNotSaidMarker);
			Assert.AreEqual(c.LegacyHasHand, response.LegacyAlsoPlayingHand);

			Assert.AreEqual(c.LegacyTarget, response.LegacyTarget);
			Assert.AreEqual(c.LegacyTargetStage, response.LegacyTargetStage);
			Assert.AreEqual(c.LegacyTargetHand, response.LegacyTargetHand);
			Assert.AreEqual(c.LegacyTargetSaidMarker, response.LegacyTargetSaidMarker);
			Assert.AreEqual(c.LegacyTargetNotSaidMarker, response.LegacyTargetNotSaidMarker);
			Assert.AreEqual(c.LegacyTargetTimeInStage, response.LegacyTargetTimeInStage);
			Assert.AreEqual(c.LegacyTargetStatus, response.LegacyTargetStatus);
			Assert.AreEqual(c.LegacyTargetLayers, response.LegacyTargetLayers);
			Assert.AreEqual(c.LegacyTargetStartingLayers, response.LegacyTargetStartingLayers);

			Assert.AreEqual(3, response.Stages.Count);
			Assert.IsTrue(response.Stages.Contains(2));
			Assert.IsTrue(response.Stages.Contains(3));
			Assert.IsTrue(response.Stages.Contains(4));
			Assert.AreEqual(c.LegacyAlsoPlayingHand, response.LegacyHasHand);
			Assert.AreEqual(c.LegacyAlsoPlayingSaidMarker, response.LegacySaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingNotSaidMarker, response.LegacyNotSaidMarker);
			Assert.AreEqual(c.LegacyAlsoPlayingTimeInStage, response.LegacyTimeInStage);
		}
		#endregion

		[TestMethod]
		public void ExpressionTest_Self()
		{
			Case response = new Case("hand")
			{
				Expressions =
				{
					new ExpressionTest("self.costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("male.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_ImplicitSelf()
		{
			Case response = new Case("hand")
			{
				Expressions =
				{
					new ExpressionTest("costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("male.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_SelfToTarget()
		{
			Case response = new Case("must_strip_normal")
			{
				Expressions =
				{
					new ExpressionTest("self.costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("target.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_ImplicitSelfToTarget()
		{
			Case response = new Case("must_strip_normal")
			{
				Expressions =
				{
					new ExpressionTest("costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("target.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_Target()
		{
			Case response = new Case("opponent_lost")
			{
				LegacyTarget = _female.FolderName,
				Expressions =
				{
					new ExpressionTest("target.costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("self.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_OtherTarget()
		{
			Case response = new Case("opponent_lost")
			{
				LegacyTarget = _bifemale.FolderName,
				Expressions =
				{
					new ExpressionTest("target.costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("target.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_AlsoPlaying()
		{
			Case response = new Case("hand")
			{
				Expressions =
				{
					new ExpressionTest("bimale.costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("bimale.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_AlsoPlayingSelf()
		{
			Case response = new Case("hand")
			{
				Expressions =
				{
					new ExpressionTest("female.costume", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("self.costume", test.Expression);
		}

		[TestMethod]
		public void ExpressionTest_Misc()
		{
			Case response = new Case("hand")
			{
				Expressions =
				{
					new ExpressionTest("background", "blah")
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Expressions.Count);
			ExpressionTest test = response.Expressions[0];
			Assert.AreEqual("background", test.Expression);
		}

		[TestMethod]
		public void TargetCondition_Self_To_Target()
		{
			Case response = new Case("must_strip_normal")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "self"
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("target", cond.Role);
			Assert.AreEqual(_male.FolderName, cond.Character);
		}

		[TestMethod]
		public void TargetCondition_SelfCharacter()
		{
			Case response = new Case("must_strip_normal")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Character = _male.FolderName
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.IsNull(cond.Role);
			Assert.AreEqual(_male.FolderName, cond.Character);
		}

		[TestMethod]
		public void TargetCondition_Target_To_Self()
		{
			Case response = new Case("opponent_lost")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "target",
						SaidMarker = "bob",
						Character = _female.FolderName
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("self", cond.Role);
			Assert.IsNull(cond.Character);
		}

		[TestMethod]
		public void TargetCondition_TargetCharacter_To_Self()
		{
			Case response = new Case("opponent_lost")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Character = _female.FolderName,
						SaidMarker = "bob"
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("self", cond.Role);
			Assert.IsNull(cond.Character);
		}

		[TestMethod]
		public void TargetCondition_Self_To_AlsoPlaying()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "self"
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("other", cond.Role);
			Assert.AreEqual(cond.Character, _male.FolderName);
		}

		[TestMethod]
		public void TargetCondition_SelfCharacter_To_AlsoPlaying()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Character = _male.FolderName
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.IsNull(cond.Role);
			Assert.AreEqual(cond.Character, _male.FolderName);
		}

		[TestMethod]
		public void TargetCondition_AlsoPlaying_To_AlsoPlaying()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "other",
						Character = "bob",
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("other", cond.Role);
			Assert.AreEqual("bob", cond.Character);
		}

		[TestMethod]
		public void TargetCondition_AlsoPlaying_To_Self()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "other",
						SaidMarker = "bob",
						Character = _female.FolderName,
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("self", cond.Role);
			Assert.IsNull(cond.Character);
		}

		/// <summary>
		/// Also Playing > Self with no conditions, so it's useless and gets deleted
		/// </summary>
		[TestMethod]
		public void TargetCondition_AlsoPlaying_Delete()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "other",
						Character = _female.FolderName,
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(0, response.Conditions.Count);
		}

		[TestMethod]
		public void TargetCondition_Roleless_To_Self()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Character = _female.FolderName,
						SaidMarker = "bob",
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("self", cond.Role);
			Assert.IsNull(cond.Character);
		}

		[TestMethod]
		public void TargetCondition_Opponent_To_Self()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "opp",
						SaidMarker = "bob",
						Character = _female.FolderName,
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("self", cond.Role);
			Assert.IsNull(cond.Character);
		}

		[TestMethod]
		public void TargetCondition_Opponent_To_Opponent()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "other",
						Character = "bob",
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("other", cond.Role);
			Assert.AreEqual("bob", cond.Character);
		}

		[TestMethod]
		public void TargetCondition_Roleless_To_Roleless()
		{
			Case response = new Case("hand")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Character = "bob",
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.IsNull(cond.Role);
			Assert.AreEqual("bob", cond.Character);
		}

		[TestMethod]
		public void TargetCondition_Target_To_Target()
		{
			Case response = new Case("male_must_strip")
			{
				Conditions =
				{
					new TargetCondition()
					{
						Role = "target",
						Character = "bob",
					}
				}
			}.CreateResponse(_male, _female);
			Assert.AreEqual(1, response.Conditions.Count);
			TargetCondition cond = response.Conditions[0];
			Assert.AreEqual("target", cond.Role);
			Assert.AreEqual("bob", cond.Character);
		}
	}
}
