using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewEditModeTest {

	[Test]
	public void NewEditModeTestSimplePasses() {
		// Use the Assert class to test conditions.
	}

    [Test]
    public void GetQuestTest()
    {
        DailyQuestSingleData dailyQuestData = new DailyQuestSingleData();
        dailyQuestData.action = QuestAction.Play;
        dailyQuestData.rewardCoin = new MinMaxDataInt(10, 20);
        dailyQuestData.rewardStar = new MinMaxDataInt(10, 20);
        dailyQuestData.actionGap = 5;
        dailyQuestData.actionCount = new MinMaxDataInt(30, 60);

        Quest quest = QuestDispenser.GetQuestFromDailyQuestSingleData(dailyQuestData);

        Assert.That(quest.targetCount % 5 == 0);
        Assert.That(quest.currentCount == 0);
    }
    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
	public IEnumerator NewEditModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
