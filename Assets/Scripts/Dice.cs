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

    // Top: 1
    // Left: 2
    // Back: 3
    // Front: 4
    // Right: 5
    // Bottom: 6
    public int GetValue()
    {
        List<Vector3> normals = new List<Vector3>()
        {
            transform.up,
            -transform.right,
            -transform.forward,
            transform.forward,
            transform.right,
            -transform.up
        };

        int winningSide = 0;
        float winningDot = 0;


        for(int i = 0; i < normals.Count; i++)
        {
            float dot = Vector3.Dot(Vector3.up, normals[i]);

            if(dot == 1)
            {
                Debug.Log(i + 1);
                return i + 1;
            }

            if(dot > winningDot)
            {
                winningDot = dot;
                winningSide = i + 1;
            }
        }
        Debug.Log(winningSide);
        return winningSide;
    }
}
