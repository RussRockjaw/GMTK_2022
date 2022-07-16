using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject projectile;
    [SerializeField]private int health;
    [SerializeField]private float fireRate;
    [SerializeField]private int burstSize;
    [SerializeField]private float turnRate;
    [SerializeField]private float reloadDelay;
    [SerializeField]private bool isReloading;
    [SerializeField]private bool isShooting = false;
    [SerializeField]private List<GameObject> ammo;
    private Vector3 origin;
   
    void Awake()
    {
        origin = transform.position + new Vector3(0f, 0.5f, 0f);
        for(int i = 0; i < burstSize; i++)
        {
            ammo.Add(Instantiate(projectile, origin, Quaternion.identity));
            ammo[i].SetActive(false);
        }
    }

    void Start()
    {
        StartCoroutine(TurretOperation());
    }

    IEnumerator TurretOperation()
    {
        while(true)
        {
            yield return new WaitUntil(() => isShooting);
            StartCoroutine(FireBurst());

            yield return new WaitWhile(() => isShooting);
            StartCoroutine(Reload());
        }
    }

    // Update is called once per frame
    void Update()
    {       
        //Track player
        Vector3 dir = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnRate * Time.deltaTime);
        RaycastHit hit;
        if(!isShooting && !isReloading)
            if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                if(hit.transform.tag == "Player")
                {
                    isShooting = true;
                }
            }
    }

    IEnumerator FireBurst()
    {
        for(int i = 0; i < ammo.Count; i++)
        {
            ammo[i].GetComponent<Projectile>().SetDirection(player.transform.position - origin);
            ammo[i].SetActive(true);
            yield return new WaitForSecondsRealtime(fireRate / burstSize);
        }    
        isShooting = false;
        isReloading = true; 
    }

    IEnumerator Reload()
    {
        yield return new WaitForSecondsRealtime(reloadDelay);
        for(int i = 0; i < ammo.Count; i++)
        {   
            ammo[i].SetActive(false);
            ammo[i].transform.position = origin;
        }
        isReloading = false;
    }
}
