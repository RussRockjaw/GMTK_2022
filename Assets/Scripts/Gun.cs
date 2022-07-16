using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]private Camera cam;
    [SerializeField]private GameObject bullet;
    [SerializeField]private List<GameObject> bullets;

    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();
        for(int i = 0; i < 5; i++)
        {
            bullets.Add(Instantiate(bullet, transform.position, Quaternion.identity));
        }
    }

    public GameObject Shoot()
    {
        RaycastHit hit;
        GameObject b = null;

        if(Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit)
                && bullets.Count > 0)
        {
            b = bullets[0];
            b.transform.position = cam.transform.position + cam.transform.forward * 2;
            b.transform.LookAt(hit.point);
            b.GetComponent<Dice>().Activate();
            bullets.RemoveAt(0);
        }

        return b;
    }

    public void Reload(GameObject bullet)
    {
        bullet.transform.rotation = Quaternion.identity;
        
        bullets.Add(bullet);
        bullet.SetActive(false);
        bullet.GetComponent<Rigidbody>().useGravity = false;
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
