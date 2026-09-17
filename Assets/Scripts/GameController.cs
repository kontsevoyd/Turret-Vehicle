using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Idle,
        Playing,
        Won,
        Lost
    }

    [SerializeField]
    private LevelController levelController;

    [SerializeField]
    private VehicleController vehicleController;

    [SerializeField]
    private GameUI gameUI;

    public GameState CurrentState { get; private set; }

    void Awake()
    {
        CurrentState = GameState.Idle;
        vehicleController.Died.AddListener(LoseGame);
        levelController.Finished.AddListener(WinGame);
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        levelController.StartLevel();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Won;

        levelController.StopLevel();

        gameUI.ShowWin();
    }

    public void LoseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Lost;

        levelController.StopLevel();

        gameUI.ShowLose();
    }

    public void HandleTap()
    {
        switch (CurrentState)
        {
            case GameState.Idle:
                StartGame();
                break;

            case GameState.Won:
            case GameState.Lost:
                RestartGame();
                break;
        }
    }

    private void OnDestroy()
    {
        if (vehicleController != null)
            vehicleController.Died.RemoveListener(LoseGame);

        if (levelController != null)
            levelController.Finished.RemoveListener(WinGame);
    }
}
