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
    public ScriptableBool isPaused;
    Coroutine lastRoutine;

    // Start is called before the first frame update
    void Awake()
    {
        GenerateEnemies();
    }

    void Start()
    {
        lastRoutine = StartCoroutine(SpawnEnemies());
    }

    //destroys current enemies and generates a fresh list of enemies
    public void Restart()
    {
        StopCoroutine(lastRoutine);
        for(int i = deadEnemies.Count-1; i >= 0; i--)
        {
            deadEnemies[i].SetActive(true);
            deadEnemies[i].GetComponent<Enemy>().Delete();
        }
        deadEnemies.Clear();

        for(int i = liveEnemies.Count-1; i >= 0; i--)
        {
            liveEnemies[i].SetActive(true);
            liveEnemies[i].GetComponent<Enemy>().Delete();
        }
        liveEnemies.Clear();

        GenerateEnemies();
    }

    //Call this when you are ready to have the manager resume functions after a game reset
    public void Resume()
    {
        lastRoutine = StartCoroutine(SpawnEnemies());
    }

    private void GenerateEnemies()
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

    IEnumerator SpawnEnemies()
    {
        //Add pause and game over checks here
        yield return new WaitUntil(() => !isPaused.value);
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
                    deadEnemies[r].GetComponent<Enemy>().Resume();
                    deadEnemies.RemoveAt(r);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
