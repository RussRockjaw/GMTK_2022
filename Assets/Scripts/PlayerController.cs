using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private Gun gun;
    [SerializeField]private Camera cam;
    [SerializeField]private int maxhealth = 20;
    [SerializeField]private int health;
    [SerializeField]private float speed;
    [SerializeField]private float cameraSpeed;
    private float horizontalRotation, verticalRotation;
    private float vert, hori;
    private Vector3 dir;
    private List<Dice> dice;
    private StateManager sm;

    public GameObject spawnPoint;
    public ScriptableBool isPaused;
    public Shahtzee shahtzee;
    public float pickupDistance = 1.0f;
    public int score = 0;
    public GameObject pauseMenu;
    public List<GameObject> checkMarks;
    public string playerName;
    public bool gameStarted;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public List<TextMeshProUGUI> diceUI;

    public EnemyManager em;
    
    void Start()
    {
        Respawn();
        UpdateScore();
        health = maxhealth;
        dice = new List<Dice>();
        shahtzee = new Shahtzee();
        Pause();
        pauseMenu.SetActive(false);
        gameStarted = false;
    }

    void Update()
    {
        // cash in dice if all dice are on the ground
        if(gameStarted && (Input.GetKeyDown("space") || Input.GetKeyDown("escape")))
        {
            if(isPaused.value)
                UnPause();
            else
                Pause();
        } 

        if(isPaused.value) 
            return;

        vert = Input.GetAxisRaw("Vertical");
        hori = Input.GetAxisRaw("Horizontal");
        dir = new Vector3(hori, 0, vert).normalized;
        transform.Translate(dir * speed * Time.deltaTime);

        Aim();
        if(Input.GetMouseButtonDown(0))
        {
            GameObject g = gun.Shoot(); 
            if(g != null)
            {
                Dice d = g.GetComponent<Dice>();
                if(d != null)
                    dice.Add(d);
            }
        }

        if(Input.GetKeyDown("l"))
        {
            em.Restart();
        }


        // check if we are looking at dice and if they are close enough to pick up
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit))
        {
            // TODO/feature: highlight the selected dice somehow?
            if(hit.collider.gameObject.tag == "Dice" && Input.GetKeyDown("e"))
                PickUpDice(hit.collider.gameObject);
        }

        for(int i = 0; i < diceUI.Count; i++)
        {
            if(i < dice.Count)
                diceUI[i].text = dice[i].GetValue().ToString();
            else
                diceUI[i].text = "";
        }
    }

    private void Aim()
    {
        horizontalRotation += cameraSpeed * Input.GetAxis("Mouse X");
        verticalRotation -= cameraSpeed * Input.GetAxis("Mouse Y");
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        this.transform.eulerAngles = new Vector3(0, horizontalRotation, 0);
        cam.transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
    }


    public List<int> GetDiceValues()
    { 
        List<int> diceRolls = new List<int>();
        foreach(Dice d in dice)
        {
            diceRolls.Add(d.GetValue());
        }
        return diceRolls;
    }

    public void ScoreAces()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Aces))
            return;

        AddToScore(shahtzee.Aces(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[0].SetActive(true);
        CheckForCardComplete();
    }
    public void ScoreTwos()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Twos))
            return;

        AddToScore(shahtzee.Twos(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[1].SetActive(true);
        CheckForCardComplete();
    }
    public void ScoreThrees()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Threes))
            return;

        AddToScore(shahtzee.Threes(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[2].SetActive(true);
        CheckForCardComplete();
    }
    public void ScoreFours()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Fours))
            return;

        AddToScore(shahtzee.Fours(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[3].SetActive(true);
        CheckForCardComplete();
    }
    public void ScoreFives()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Fives))
            return;

        AddToScore(shahtzee.Fives(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[4].SetActive(true);
        CheckForCardComplete();
    }
    public void ScoreSixes()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Sixes))
            return;

        AddToScore(shahtzee.Sixes(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[5].SetActive(true);
        CheckForCardComplete();
    }

    public void ScoreThreeOfAKind()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.ThreeOfAKind))
            return;

        AddToScore(shahtzee.ThreeOfAKind(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[6].SetActive(true);
        CheckForCardComplete();
    }

    public void ScoreFourOfAKind()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.FourOfAKind))
            return;

        AddToScore(shahtzee.FourOfAKind(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[7].SetActive(true);
        CheckForCardComplete();
    }

    public void ScoreFullHouse()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.FullHouse))
            return;

        AddToScore(shahtzee.FullHouse(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[8].SetActive(true);
        CheckForCardComplete();
    }

    public void ScoreSmStraight()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.SmallStraight))
            return;

        AddToScore(shahtzee.SmallStraight(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[9].SetActive(true);
        CheckForCardComplete();
    }

    public void ScoreLgStraight()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.LargeStraight))
            return;

        AddToScore(shahtzee.LargeStraight(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[10].SetActive(true);
        CheckForCardComplete();
    }

    public void ScoreChance()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Chance))
            return;

        AddToScore(shahtzee.Chance(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[11].SetActive(true);
        CheckForCardComplete();
    }

    public void ScoreShahtzee()
    {
        if(dice.Count != 5 || shahtzee.IsComplete(ScoreTypes.Shahtzee))
            return;

        AddToScore(shahtzee.FiveOfAKind(GetDiceValues()));
        PickUpAllDice();
        UnPause();
        checkMarks[12].SetActive(true);
        CheckForCardComplete();
    }

    public void PickUpDice(GameObject g)
    {
        dice.Remove(g.GetComponent<Dice>());
        gun.Reload(g);
    }

    public void PickUpAllDice()
    {
        foreach(Dice d in dice)
        {
            gun.Reload(d.gameObject);
        }
        dice = new List<Dice>();
    }


    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);

        isPaused.value = true;
        Time.timeScale = 0f;
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);

        isPaused.value = false;
        Time.timeScale = 1.0f;
    }

    public void AddToScore(int n)
    {
        score += n;
        UpdateScore();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScore();
    }

    public void CheckForCardComplete()
    {
        if(shahtzee.IsCurrentCardComplete())
        {
            AddToScore(shahtzee.cardBonus);
            shahtzee.NewCard();
            foreach(GameObject g in checkMarks)
                g.SetActive(false);
        }
    }

    public void UpdateScore()
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateHealth(int val)
    {
        health += val;
        healthText.text = $"Health: {health}/{maxhealth}";
        if(health <= 0)
        {
            GameOver();
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint.transform.position;
        transform.rotation = Quaternion.identity;
    }

    public void SetPlayerName(string s)
    {
        nameText.text = s;
    }

    public void GameOver()
    {
        Pause();
        gameStarted = false;
        // spawn the death screen, showing score and a button to restart game
    }

    public void ResetGame()
    {
        score = 0;
        health = maxhealth;
        UpdateScore();
        Respawn();
        shahtzee = new Shahtzee();
        PickUpAllDice();
        UnPause();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
