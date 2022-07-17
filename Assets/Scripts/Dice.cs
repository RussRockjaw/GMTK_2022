using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]Rigidbody rb;
    [SerializeField]private int damage;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector2[] UVs = new Vector2[mesh.vertices.Length];

        // Front
        UVs[0] = new Vector2(0.0f, 0.0f);
        UVs[1] = new Vector2(0.333f, 0.0f);
        UVs[2] = new Vector2(0.0f, 0.333f);
        UVs[3] = new Vector2(0.333f, 0.333f);
        // Top
        UVs[4] = new Vector2(0.334f, 0.333f);
        UVs[5] = new Vector2(0.666f, 0.333f);
        UVs[8] = new Vector2(0.334f, 0.0f);
        UVs[9] = new Vector2(0.666f, 0.0f);
        // Back
        UVs[6] = new Vector2(1.0f, 0.0f);
        UVs[7] = new Vector2(0.667f, 0.0f);
        UVs[10] = new Vector2(1.0f, 0.333f);
        UVs[11] = new Vector2(0.667f, 0.333f);
        // Bottom
        UVs[12] = new Vector2(0.0f, 0.334f);
        UVs[13] = new Vector2(0.0f, 0.666f);
        UVs[14] = new Vector2(0.333f, 0.666f);
        UVs[15] = new Vector2(0.333f, 0.334f);
        // Left
        UVs[16] = new Vector2(0.334f, 0.334f);
        UVs[17] = new Vector2(0.334f, 0.666f);
        UVs[18] = new Vector2(0.666f, 0.666f);
        UVs[19] = new Vector2(0.666f, 0.334f);
        // Right        
        UVs[20] = new Vector2(0.667f, 0.334f);
        UVs[21] = new Vector2(0.667f, 0.666f);
        UVs[22] = new Vector2(1.0f, 0.666f);
        UVs[23] = new Vector2(1.0f, 0.334f);
        mesh.uv = UVs;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-1, 1),Random.Range(-1, 1),Random.Range(-1, 1)) * 1000000, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        rb.useGravity = true;
        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Health -= damage;
        }
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
                return i + 1;
            }

            if(dot > winningDot)
            {
                winningDot = dot;
                winningSide = i + 1;
            }
        }
        return winningSide;
    }
}
