namespace TowerDefense;

public sealed class GameSession
{
    // Singleton: one shared instance tracks gold, lives, and waves for the game.
    public static GameSession Instance { get; } = new GameSession();

    public const int TotalWaves = 5;
    public const int WaveReward = 30;

    public int Gold { get; private set; }
    public int Lives { get; private set; }
    public int WaveNumber { get; private set; }
    public int DefeatedEnemies { get; private set; }
    public bool WaveInProgress { get; private set; }
    public bool HasWon { get; private set; }
    public bool IsGameOver => Lives == 0;
    public bool IsFinished => IsGameOver || HasWon;

    // The private constructor prevents calls to new GameSession() outside this class.
    private GameSession()
    {
        Reset();
    }

    public void Reset()
    {
        Gold = 180;
        Lives = 10;
        WaveNumber = 0;
        DefeatedEnemies = 0;
        WaveInProgress = false;
        HasWon = false;
    }

    public bool TrySpendGold(int amount)
    {
        if (amount <= 0 || amount > Gold || IsFinished)
            return false;

        Gold -= amount;
        return true;
    }

    public void RewardEnemy(int reward)
    {
        Gold += reward;
        DefeatedEnemies++;
    }

    public void LoseLife()
    {
        if (Lives > 0)
            Lives--;
    }

    public bool BeginWave()
    {
        if (WaveInProgress || IsFinished)
            return false;

        WaveNumber++;
        WaveInProgress = true;
        return true;
    }

    public void CompleteWave()
    {
        if (!WaveInProgress || IsFinished)
            return;

        WaveInProgress = false;
        Gold += WaveReward;
        HasWon = WaveNumber == TotalWaves;
    }
}
