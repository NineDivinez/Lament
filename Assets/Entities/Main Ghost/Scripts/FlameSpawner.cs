using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlameSpawner : MonoBehaviour
{
    //Declared Variables
    public Vector3 center;
    public Vector3 size;
    public Vector3 houseRadius;

    //Game Object References
    public GameObject[] flames;
    public GameObject[] spawnedFlames;

    void Start()
    {
        center = this.transform.position;
    }

    void Update()
    {
        spawnedFlames = GameObject.FindGameObjectsWithTag("WorldFlame");

        if (spawnedFlames.Length <= 8)
        {
            spawnFlame();
        }
    }

    void spawnFlame()
    {
        Vector3 spawnPos = center + new Vector3(Random.Range((-size.x / 2) -houseRadius.x, (size.x / 2) +houseRadius.x), Random.Range(-size.y / 2, size.y / 2), Random.Range((-size.z / 2) -houseRadius.z, (size.z / 2) +houseRadius.z));

        float flameChooser = Random.Range(1f, 100f);
        int flameChosen = 0;

        if (flameChooser >= 50f)
        {
            flameChosen = 1;
        }
        
        Instantiate(flames[flameChosen], spawnPos, Quaternion.identity);
    }
}
