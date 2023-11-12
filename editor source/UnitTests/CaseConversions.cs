using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPNATI_Character_Editor;

namespace UnitTests
{
	[TestClass]
	public class CaseConversions
	{
		[TestMethod]
		public void TransfersTarget()
		{
			Case c = new Case();
			c.LegacyTarget = "bob";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTarget);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("bob", c.Conditions[0].Character);
		}

		[TestMethod]
		public void TransfersTargetStage()
		{
			Case c = new Case();
			c.LegacyTargetStage = "0-5";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetStage);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("0-5", c.Conditions[0].Stage);
		}

		[TestMethod]
		public void TransfersTargetHand()
		{
			Case c = new Case();
			c.LegacyTargetHand = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetHand);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].Hand);
		}

		[TestMethod]
		public void TransfersTargetLayers()
		{
			Case c = new Case();
			c.LegacyTargetLayers = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetLayers);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].Layers);
		}

		[TestMethod]
		public void TransfersTargetStartingLayers()
		{
			Case c = new Case();
			c.LegacyTargetStartingLayers = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetStartingLayers);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].StartingLayers);
		}

		[TestMethod]
		public void TransfersTargetStatus()
		{
			Case c = new Case();
			c.LegacyTargetStatus = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetStatus);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].Status);
		}

		[TestMethod]
		public void TransfersTargetSaidMarker()
		{
			Case c = new Case();
			c.LegacyTargetSaidMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetSaidMarker);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].SaidMarker);
		}

		[TestMethod]
		public void TransfersTargetSayingMarker()
		{
			Case c = new Case();
			c.LegacyTargetSayingMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetSayingMarker);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].SayingMarker);
		}

		[TestMethod]
		public void TransfersTargetNotSaidMarker()
		{
			Case c = new Case();
			c.LegacyTargetNotSaidMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetNotSaidMarker);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].NotSaidMarker);
		}

		[TestMethod]
		public void TransfersTargetSaying()
		{
			Case c = new Case();
			c.LegacyTargetSaying = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetSaying);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].Saying);
		}

		[TestMethod]
		public void TransfersTargetTimeInStage()
		{
			Case c = new Case();
			c.LegacyTargetTimeInStage = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTargetTimeInStage);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].TimeInStage);
		}

		[TestMethod]
		public void TransfersTargetConsecutiveLosses()
		{
			Case c = new Case();
			TriggerDatabase.AddTrigger(new TriggerDefinition("opponent_lost", "opponent_lost") { HasTarget = true });
			c.Tag = "opponent_lost";
			c.LegacyConsecutiveLosses = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyConsecutiveLosses);
			Assert.AreEqual("target", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].ConsecutiveLosses);
		}

		[TestMethod]
		public void TransfersSelfHand()
		{
			Case c = new Case();
			c.LegacyHasHand = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyHasHand);
			Assert.AreEqual("self", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].Hand);
		}

		[TestMethod]
		public void TransfersSelfSaidMarker()
		{
			Case c = new Case();
			c.LegacySaidMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacySaidMarker);
			Assert.AreEqual("self", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].SaidMarker);
		}

		[TestMethod]
		public void TransfersSelfNotSaidMarker()
		{
			Case c = new Case();
			c.LegacyNotSaidMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyNotSaidMarker);
			Assert.AreEqual("self", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].NotSaidMarker);
		}

		[TestMethod]
		public void TransfersSelfTimeInStage()
		{
			Case c = new Case();
			c.LegacyTimeInStage = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyTimeInStage);
			Assert.AreEqual("self", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].TimeInStage);
		}

		[TestMethod]
		public void TransfersSelfConsecutiveLosses()
		{
			Case c = new Case();
			c.LegacyConsecutiveLosses = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyConsecutiveLosses);
			Assert.AreEqual("self", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].ConsecutiveLosses);
		}

		[TestMethod]
		public void TransfersAlsoPlaying()
		{
			Case c = new Case();
			c.LegacyAlsoPlaying = "bob";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlaying);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("bob", c.Conditions[0].Character);
		}

		[TestMethod]
		public void TransfersAlsoPlayingStage()
		{
			Case c = new Case();
			c.LegacyAlsoPlayingStage = "0-5";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlayingStage);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("0-5", c.Conditions[0].Stage);
		}

		[TestMethod]
		public void TransfersAlsoPlayingHand()
		{
			Case c = new Case();
			c.LegacyAlsoPlayingHand = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlayingHand);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].Hand);
		}

		[TestMethod]
		public void TransfersAlsoPlayingSaidMarker()
		{
			Case c = new Case();
			c.LegacyAlsoPlayingSaidMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlayingSaidMarker);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].SaidMarker);
		}

		[TestMethod]
		public void TransfersAlsoPlayingSayingMarker()
		{
			Case c = new Case();
			c.LegacyAlsoPlayingSayingMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlayingSayingMarker);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].SayingMarker);
		}

		[TestMethod]
		public void TransfersAlsoPlayingNotSaidMarker()
		{
			Case c = new Case();
			c.LegacyAlsoPlayingNotSaidMarker = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlayingNotSaidMarker);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].NotSaidMarker);
		}

		[TestMethod]
		public void TransfersAlsoPlayingSaying()
		{
			Case c = new Case();
			c.LegacyAlsoPlayingSaying = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlayingSaying);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].Saying);
		}

		[TestMethod]
		public void TransfersAlsoPlayingTimeInStage()
		{
			Case c = new Case();
			c.LegacyAlsoPlayingTimeInStage = "blah";
			DataConversions.ConvertCase5_2(c);
			Assert.AreEqual(1, c.Conditions.Count);
			Assert.IsNull(c.LegacyAlsoPlayingTimeInStage);
			Assert.AreEqual("other", c.Conditions[0].Role);
			Assert.AreEqual("blah", c.Conditions[0].TimeInStage);
		}
	}
}
