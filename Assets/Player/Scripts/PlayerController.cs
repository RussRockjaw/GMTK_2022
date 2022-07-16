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
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        health = 20;
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
            gun.Shoot();
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

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Bullet")
        {
            gun.Reload(col.gameObject);
        }
    }

    public void UpdateHealth(int val)
    {
        health += val;
    }
}
