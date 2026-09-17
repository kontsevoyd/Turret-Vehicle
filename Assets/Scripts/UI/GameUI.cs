using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Level UI")]
    [SerializeField]
    private LevelController levelController;

    [SerializeField]
    private GameObject finishGameUI;

    [SerializeField]
    private GameObject win;

    [SerializeField]
    private GameObject lose;

    [Header("Vehicle UI")]
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Health vehicleHealth;

    [SerializeField]
    private Image vehicleHealthFill;

    [SerializeField]
    private Image progressFill;

    private float startPosition;

    private void Start()
    {
        startPosition = target.position.z;

        finishGameUI.SetActive(false);

        vehicleHealth.Hit.AddListener(UpdateHealthBar);

        UpdateHealthBar();
        UpdateProgressBar();
    }

    private void Update()
    {
        UpdateProgressBar();
    }

    public void ShowWin()
    {
        finishGameUI.SetActive(true);

        win.SetActive(true);
        lose.SetActive(false);
    }

    public void ShowLose()
    {
        finishGameUI.SetActive(true);

        win.SetActive(false);
        lose.SetActive(true);
    }

    private void UpdateHealthBar()
    {
        vehicleHealthFill.fillAmount = vehicleHealth.CurrentHealth / vehicleHealth.MaxHealth;
    }

    private void UpdateProgressBar()
    {
        float travelledDistance = target.position.z - startPosition;

        float progress = travelledDistance / levelController.LevelLength;

        progressFill.fillAmount = Mathf.Clamp01(progress);
    }

    private void OnDestroy()
    {
        if (vehicleHealth != null)
            vehicleHealth.Hit.RemoveListener(UpdateHealthBar);
    }
}
