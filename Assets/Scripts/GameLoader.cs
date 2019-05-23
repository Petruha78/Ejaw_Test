using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Web;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private Text waitText;
    [SerializeField] private WebConnector connector;

    private string _path = "https://postman-echo.com/get?foo1=bar1&foo2=bar2";

    private void Start()
    {
        EventContainer.Subscribe(Topics.PlayGame, Play);
        connector.Get<object>(_path, (_) => StartCoroutine(Callback()) );
    }

    private IEnumerator Callback()
    {
        waitText.text = "Download complete";
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }

    private void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
