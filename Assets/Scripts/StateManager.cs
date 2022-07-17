using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateManager : MonoBehaviour
{

    public GameObject titleUI;
    public GameObject gameUI;
    public GameObject gameOverUI;

    public TextMeshProUGUI nameText;
    public ScriptableBool isPaused;
    public PlayerController player;
    public EnemyManager em;


    // Start is called before the first frame update
    void Start()
    {
        Title();
    }

    public void Title()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameUI.SetActive(false);
        isPaused.value = true;
        titleUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    public void BackToTitle()
    {
        player.ResetGame();
        Title();
    }

    public void StartGame()
    {
        gameUI.SetActive(true);
        titleUI.SetActive(false);
        gameOverUI.SetActive(false);
        player.SetPlayerName(nameText.text);
        em.Restart();
        em.Resume();
        player.StartGame();
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverUI.SetActive(true);
        titleUI.SetActive(false);
    }

    public void Restart()
    {
        player.ResetGame();
        StartGame();
    }
}
