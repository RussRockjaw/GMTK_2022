using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateManager : MonoBehaviour
{

    public GameObject titleUI;
    public GameObject gameUI;
    public TextMeshProUGUI nameText;
    public ScriptableBool isPaused;
    public PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameUI.SetActive(false);
        isPaused.value = true;
        titleUI.SetActive(true);
    }

    public void StartGame()
    {
        player.gameStarted = true;
        gameUI.SetActive(true);
        player.UnPause();
        titleUI.SetActive(false);
        player.SetPlayerName(nameText.text);
    }
}
