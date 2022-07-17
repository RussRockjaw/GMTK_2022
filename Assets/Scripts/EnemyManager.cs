using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{   
    [SerializeField]private GameObject turretPrefab;
    [SerializeField]private GameObject chargerPrefab;
    [SerializeField]private int spawnArea = 15;
    [SerializeField]private List<GameObject> deadEnemies;
    [SerializeField]private List<GameObject> liveEnemies;
    [SerializeField]private int maxEnemies = 20;
    [SerializeField]private float spawnInterval = 5;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < maxEnemies; i++)
        {
            int r = Random.Range(0, 2);
            if(r == 0)
            {
                deadEnemies.Add(Instantiate(turretPrefab, new Vector3(0, 0, 0), Quaternion.identity));
                deadEnemies[i].SetActive(false);
            }
            else if(r == 1)
            {
                deadEnemies.Add(Instantiate(chargerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
                deadEnemies[i].SetActive(false);
            }
        }
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        //Add pause and game over checks here
        //yield return new WaitUnil(() => !isPause);
        while(true)
        {
            if(liveEnemies.Count > 0)
            {
                for(int i = liveEnemies.Count - 1; i >= 0; i--)
                {
                    if(!liveEnemies[i].activeSelf)
                    {
                        deadEnemies.Add(liveEnemies[i]);
                        liveEnemies.RemoveAt(i);
                    }
                }
                //liveEnemies.RemoveAll(enemy => !enemy.gameObject.activeSelf);
            }
                
            if(liveEnemies.Count < maxEnemies)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-spawnArea, spawnArea), 1.1f, Random.Range(-spawnArea, spawnArea));
                if(!Physics.CheckBox(pos, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity))
                {
                    int r = Random.Range(0, deadEnemies.Count);
                    deadEnemies[r].GetComponent<Enemy>().Respawn(pos);
                    deadEnemies[r].SetActive(true);
                    liveEnemies.Add(deadEnemies[r]);
                    deadEnemies[r].GetComponent<Enemy>().Reset();
                    deadEnemies.RemoveAt(r);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
