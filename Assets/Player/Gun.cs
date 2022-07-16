using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]private Camera cam;
    [SerializeField]private GameObject bullet;
    [SerializeField]private List<GameObject> bullets;
    private Vector3 offset = new Vector3(0f, 0f, 0.5f);
    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();
        for(int i = 0; i < 5; i++)
        {
            bullets.Add(Instantiate(bullet, transform.position, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        //Debug.Log("Shot Out!");
        
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit))
        {
            if(bullets.Count > 0)
            {
                bullets[0].transform.position = cam.transform.position;
                bullets[0].transform.LookAt(hit.point);
                bullets[0].GetComponent<Bullet>().Activate();
                bullets.RemoveAt(0);
            }
        }
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
