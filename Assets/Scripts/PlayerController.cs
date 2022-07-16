using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private Gun gun;
    [SerializeField]private Camera cam;
    [SerializeField]private int health;
    [SerializeField]private float speed;
    [SerializeField]private float cameraSpeed;
    private float horizontalRotation, verticalRotation;
    private float vert, hori;
    private Vector3 dir;
    private List<Dice> dice;

    public Shahtzee shahtzee;
    public float pickupDistance = 1.0f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        health = 20;
        dice = new List<Dice>();
        shahtzee = new Shahtzee();
    }

    void Update()
    {
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

        // cash in dice if all dice are on the ground
        if(Input.GetKeyDown("r") && dice.Count == 5)
        {
            GetDiceValues();
        } 

        // check if we are looking at dice and if they are close enough to pick up
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit))
        {
            // TODO/feature: highlight the selected dice somehow?
            if(hit.collider.gameObject.tag == "Dice" && Input.GetKeyDown("e") && hit.distance <= pickupDistance)
                PickUpDice(hit.collider.gameObject);
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

    // TODO/feature: this should open a menu of the score card
    //               the player should be able to click the choice they want to score
    //               with their currently laid out dice
    //               all dice need to be on the ground (in the dice array)
    //               to be able to trigger a CashIn
    public void GetDiceValues()
    {
        List<int> diceRolls = new List<int>();
        foreach(Dice d in dice)
        {
            diceRolls.Add(d.GetValue());
        }
    }

    public void PickUpDice(GameObject g)
    {
        dice.Remove(g.GetComponent<Dice>());
        gun.Reload(g);
    }

    public void UpdateHealth(int val)
    {
        health += val;
    }
}
