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
    private List<GameObject> dice;
    public Shahtzee shahtzee;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        health = 20;
        dice = new List<GameObject>();
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
            GameObject d = gun.Shoot();
            if(d != null)
                dice.Add(d);
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

    // TODO/feature: pick up should happen when looking, on click, not on just walking over it
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Dice")
            PickUp(col.gameObject);
    }

    // TODO/feature: this should open a menu of the score card
    //               the player should be able to click the choice they want to score
    //               with their currently laid out dice
    //               all dice need to be on the ground (in the dice array)
    //               to be able to trigger a CashIn
    public void CashInDice()
    {

    }

    public void PickUp(GameObject g)
    {
        dice.Remove(g);
        gun.Reload(g);
    }

    public void UpdateHealth(int val)
    {
        health += val;
    }
}
