using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-1, 1),Random.Range(-1, 1),Random.Range(-1, 1)) * 1000, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision col)
    {
        rb.useGravity = true;
    }
}
