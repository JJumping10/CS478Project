using UnityEngine.SceneManagement;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UI_Test
{

    private GameManager gameManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return SceneManager.LoadSceneAsync("LevelUI");

        gameManager = GameManager.instance;
        Assert.IsNotNull(gameManager, "GameManager was not initialised.");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (GameManager.instance != null)
        {
            Object.Destroy(GameManager.instance.gameObject);
        }

        yield return null;
    }

    // A Test behaves as an ordinary method
    [Test]
    public void GameManagerExists()
    {
        Assert.IsNotNull(gameManager);
        Debug.Log("PASS: The game manager exists.");

    }

    [Test]
    public void ScoreTesting()
    {
        Assert.AreEqual(0,gameManager.score);
        Debug.Log("PASS: Score initialised to zero.");
        gameManager.AddScore(500000);
        Assert.AreEqual(500000,gameManager.score);
        Debug.Log("PASS: Points added.");
        gameManager.AddScore(-400000);
        Assert.AreEqual(100000,gameManager.score);
        Debug.Log("PASS: Points subtracted.");
        gameManager.AddScore(-400000);
        Assert.AreEqual(0,gameManager.score);
        Debug.Log("PASS: Points cannot go below zero.");
        gameManager.AddScore(9000000);
        Assert.AreEqual(9000000,gameManager.score);
        Debug.Log("PASS: Points added.");
        gameManager.AddScore(1000000);
        Assert.AreEqual(9999999,gameManager.score);
        Debug.Log("PASS: Points cannot go above maximum.");
    }

    [Test]
    public void AttemptsInitialisation()
    {
        Assert.AreEqual(1,gameManager.attempts);
        Debug.Log("PASS: Attempts start at 1.");
    }

    [Test]
    public void AttemptsTesting()
    {
        for (int i = 2; i <= 10; i++)
        {
            gameManager.AddAttempt();
            Assert.AreEqual(i,gameManager.attempts);
            Debug.Log($"PASS: Attempts incremented to {i}.");
        }
    }

    [Test]
    public void AttemptsReset()
    {
        gameManager.AddAttempt();
        Assert.AreEqual(2,gameManager.attempts);
        Debug.Log($"PASS: Attempts incremented to 2.");
        gameManager.ResetAttempts();
        Assert.AreEqual(1,gameManager.attempts);
        Debug.Log($"PASS: Attempts reset to 1.");
    }

    [Test]
    public void LevelTesting()
    {
        for (int i = 1; i <= 5; i++)
        {
            gameManager.SetLevel(i);
            Assert.AreEqual(i,gameManager.level);
            Debug.Log($"PASS: Level set to {i}.");
        }
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(10)]
    public void RejectInvalidLevels(int level)
    {
        gameManager.SetLevel(1);
        gameManager.SetLevel(level);
        Assert.AreEqual(1,gameManager.level);
        Debug.Log($"PASS: Invalid level {level} rejected.");
    }

    [UnityTest]
    public IEnumerator TimerTest()
    {
        //testing timer starting automatically
        Assert.IsTrue(gameManager.GameTimer.IsRunning);
        float savedTime = gameManager.GameTimer.GameTime;
        Debug.Log($"PASS: Timer running.");
        yield return new WaitForSecondsRealtime(2f);
        Assert.AreNotEqual(savedTime, gameManager.GameTimer.GameTime);
        Debug.Log($"PASS: Time has passed.");

        //testing stop
        gameManager.GameTimer.Stop();
        Assert.IsFalse(gameManager.GameTimer.IsRunning);
        Debug.Log($"PASS: Timer not running.");
        savedTime = gameManager.GameTimer.GameTime;
        yield return new WaitForSecondsRealtime(2f);
        Assert.AreEqual(gameManager.GameTimer.GameTime, savedTime);
        Debug.Log($"PASS: Time has not passed.");

        //testing start
        gameManager.GameTimer.Start();
        yield return new WaitForSecondsRealtime(2f);
        Assert.IsTrue(gameManager.GameTimer.IsRunning);
        Debug.Log($"PASS: Timer running.");
        Assert.AreNotEqual(savedTime, gameManager.GameTimer.GameTime);
        Debug.Log($"PASS: Time has passed.");

        //testing reset
        gameManager.GameTimer.Reset();
        Assert.AreEqual(0,gameManager.GameTimer.GameTime);
        Debug.Log($"PASS: Time has been reset to 0.");
        Assert.IsFalse(gameManager.GameTimer.IsRunning);
        Debug.Log($"PASS: Timer not running.");
        yield return new WaitForSecondsRealtime(2f);
        Assert.AreEqual(0,gameManager.GameTimer.GameTime);
        Debug.Log($"PASS: Timer still 0.");

        yield return null;
    }

    [Test]
    public void AddingTimeTest()
    {
        gameManager.GameTimer.Stop();
        float savedTime = gameManager.GameTimer.GameTime;
        gameManager.GameTimer.AddTime(60);
        Assert.AreEqual(savedTime+60, gameManager.GameTimer.GameTime);
        Debug.Log($"PASS: 60 seconds added.");
        gameManager.GameTimer.AddTime(-30);
        Assert.AreEqual(savedTime+30, gameManager.GameTimer.GameTime);
        Debug.Log($"PASS: 30 seconds subtracted.");
    }

    [UnityTest]
    public IEnumerator AttemptPointsTest()
    {
        int attempts;
        int points;
        int start = 1000000;
        string plural = "";
        Assert.AreEqual(1,gameManager.attempts);
        Debug.Log($"PASS: Attempts start at 1.");
        gameManager.AddScore(0 - gameManager.score);
        Assert.AreEqual(0,gameManager.score);
        Debug.Log($"PASS: Score is 0.");
        gameManager.AddScore(start);
        Debug.Log($"PASS: Score is {start}.");

        for (attempts = 1; attempts <= 10; attempts++)
        {
            if (plural == "" && attempts > 1) { plural = "s"; }
            points = 10000 * (4 - attempts);
            Assert.AreEqual(attempts,gameManager.attempts);
            gameManager.AddAttemptsPoints();
            yield return new WaitForSecondsRealtime(2f);
            Assert.AreEqual(0,gameManager.attempts);
            Assert.AreEqual(points+start,gameManager.score);
            Debug.Log($"PASS: {points} points added for {attempts} attempt{plural}.");
            gameManager.score -= gameManager.score;
            gameManager.AddScore(start);
            for (int i = 0; i <= attempts; i++)
            {
                gameManager.AddAttempt();
            }
        }
    }

    [UnityTest]
    public IEnumerator TimePointsTest()
    {
        gameManager.GameTimer.Reset();
        gameManager.GameTimer.AddTime(60);
        Assert.AreEqual(60,gameManager.GameTimer.GameTime);
        Debug.Log($"PASS: Time is 60 seconds.");
        gameManager.GameTimer.CalculatePoints();
        yield return new WaitForSecondsRealtime(4f);
        Assert.AreEqual(1000000,(int)gameManager.score);
        Debug.Log($"PASS: Score is 1000000 for 60 seconds.");
    }

    [Test]
    public void z_AttemptsInitialisation()
    {
        Assert.AreEqual(2,gameManager.attempts);
        Debug.Log("PASS: Attempts start at 1.");
    }

}
