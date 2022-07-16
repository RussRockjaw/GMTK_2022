using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private int damage = -1;
    public Vector3 direction;
    private float speed = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().UpdateHealth(damage);
        }
        gameObject.SetActive(false);
    }
}
