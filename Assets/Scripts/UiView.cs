using Core;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        EventContainer.Subscribe(Topics.UiReset, ResetUi);
        EventContainer<int>.Subscribe(Topics.ScoreUpdate, UpdateScore);
        EventContainer<int>.Subscribe(Topics.TimerUpdate, UpdateTime);
        EventContainer<bool>.Subscribe(Topics.ResetButtonHide, HideRestartButton);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(() => EventContainer.Raise(Topics.GameExit));
        pauseButton.onClick.AddListener(() => EventContainer.Raise(Topics.GamePaused));
        ResetUi();

    }

    private void RestartGame()
    {
        EventContainer.Raise(Topics.GameReload);
        EventContainer.Raise(Topics.GamePaused);
        ResetUi();
        HideRestartButton(false);
    }

    private void UpdateTime(int sec)
    {
        var time = Constants.MaxTime - sec;
        timerText.text = time.ToString();
    }

    private void UpdateScore(int score)
    { 
        scoreText.text = score.ToString();
    }

    private void ResetUi()
    {
        UpdateScore(0);
        UpdateTime(0);
    }

    private void HideRestartButton(bool isHide)
    {
        uiPanel.SetActive(isHide);
    }     
}