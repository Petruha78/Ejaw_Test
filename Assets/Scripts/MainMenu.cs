using Core;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;


    private void Awake()
    {
        startButton.onClick.AddListener(Play);
    }

    private void Play()
    {
        EventContainer.Raise(Topics.PlayGame);
    }
}
