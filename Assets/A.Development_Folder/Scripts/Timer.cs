public class Timer
{
    public float GameTime { get; private set; }
    public bool IsRunning { get; private set; }
    const int BASE_POINTS = 1000000; // points for completing the level in one minute

    public Timer()
    {
        GameTime = 0;
        IsRunning = false;
    }

    public void Start()
    {
        IsRunning = true;
    }
    public void Stop()
    {
        IsRunning = false;
    }

    public void Reset()
    {
        GameTime = 0;
    }

    public void ConvertToPoints()
    {
        IsRunning = false;
        float playerTime = GameTime;
        while (GameTime > 0)
        {
            GameTime -= 1;
            GameManager.instance.AddScore((int)(BASE_POINTS*60/(playerTime*playerTime)));
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
