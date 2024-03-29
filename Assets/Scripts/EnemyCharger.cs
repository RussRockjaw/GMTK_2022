using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : Enemy
{
    private GameObject body;

    [SerializeField]private GameObject player;
    [SerializeField]private int health;
    [SerializeField]private int damage = -1;
    [SerializeField]private float speed;
    [SerializeField]private float turnSpeed;
    [SerializeField]private bool foundTarget;
    [SerializeField]private bool isCharging;
    [SerializeField]private bool onCooldown;
    [SerializeField]private float cooldown;
    public ScriptableBool isPaused;
    private Coroutine lastRoutine;

    void Awake()
    {
        player = GameObject.Find("Player");
    }
    // Start is called before the first frame update
    void Start()
    {   
        //player = GameObject.Find("Player");
        Health = MaxHealth;
        body = gameObject.transform.GetChild(0).gameObject;
        lastRoutine = StartCoroutine(ChargerOperation());
    }

    // Update is called once per frame
    void Update()
    {
       if(Health <= 0)
       {
            StopCoroutine(lastRoutine);
            this.Kill();
       }  
    }

    IEnumerator ChargerOperation()
    {  
        while(true)
        {
            yield return new WaitUntil(() => !isPaused.value);
            if(!foundTarget && !isCharging && !onCooldown)
            {
                Vector3 dir = player.transform.position - transform.position;
                dir.y=0;
                Quaternion rotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);

                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                {
                    if(hit.transform.tag == "Player")
                    {
                        foundTarget = true;
                        isCharging = true;
                    }
                }
            }

            else if(foundTarget && isCharging && !onCooldown)
            {
                Debug.DrawRay(transform.position, transform.forward * 30, Color.red); 
                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            }

            else if(!foundTarget && !isCharging && onCooldown)
            {
                yield return new WaitForSecondsRealtime(cooldown);
                onCooldown = false;
            }

            yield return null;  
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag != "Floor")
        {
            isCharging = false;
            foundTarget = false;
            onCooldown = true;
        }

        if(col.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().UpdateHealth(damage);
        }

    }

    public override void Resume()
    {
        lastRoutine = StartCoroutine(ChargerOperation());
    }
}
