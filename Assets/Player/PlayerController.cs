using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private Gun gun;
    [SerializeField]private Camera cam;
    [SerializeField]private Rigidbody rb;
    [SerializeField]private float force;
    [SerializeField]private float maxSpeed;
    [SerializeField]private float cameraSpeed;
    private float horizontalRotation, verticalRotation;
    private float vert, hori;
    private Vector3 dir;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(dir * force);
    }

    void Update()
    {
        vert = Input.GetAxis("Vertical");
        hori = Input.GetAxis("Horizontal");
        dir = new Vector3(hori, 0, vert).normalized;
        if(rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed; 

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
}
