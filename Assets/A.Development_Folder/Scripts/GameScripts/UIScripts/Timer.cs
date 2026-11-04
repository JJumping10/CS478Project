using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public float GameTime { get; private set; }
    public bool IsRunning { get; private set; }
    const int BASE_POINTS = 60000000; // points for completing the level in one second

    public Timer()
    {
        GameTime = 0;
        IsRunning = false;
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(transform.root.gameObject);
        }
    }

    public void Start()
    {
        IsRunning = true;
        Debug.Log("GameTime: " + GameTime);
    }
    public void Stop()
    {
        IsRunning = false;
    }

    public void Reset()
    {
        IsRunning = false;
        GameTime = 0;
    }

    public void AddTime(float newTime)
    {
        GameTime += newTime;
    }

    public void CalculatePoints()
    {
        StartCoroutine(TimeToPoints());
    }

    private IEnumerator TimeToPoints()
    {
        IsRunning = false;
        float playerTime = Mathf.Max(0f,Mathf.FloorToInt(GameTime));
        while (GameTime > 0)
        {
            GameTime -= Mathf.Min(1, GameTime);
            GameManager.instance.AddScore((int)(BASE_POINTS/(playerTime*playerTime)));
            yield return new WaitForSecondsRealtime(0.033f);
        }

    }

    public void Tick(float deltaTime)
    {
        if (IsRunning)
        {
            GameTime += deltaTime;
        }
    }
}
