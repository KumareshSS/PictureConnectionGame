using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryHandler : MonoBehaviour
{
    [SerializeField] private Button StartGame,ExitGame;
    [SerializeField] private GameObject EntryGame;
    [SerializeField] SceneLoader sceneLoader;
    void Start()
    {
        StartGame.onClick.AddListener(OpenLoginPanel);
        ExitGame.onClick.AddListener(QuitGame);

    }

    private void OpenLoginPanel()
    {
        if (sceneLoader != null)
            StartGame.gameObject.SetActive(false);
        EntryGame.SetActive(false);
        sceneLoader.LoadSceneWithLoading("Gameplay", 2f);

    }
    private void QuitGame()
    {
        Application.Quit();
    }

}
