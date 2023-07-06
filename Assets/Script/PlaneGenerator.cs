using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    public GameObject[] planePrefabs;
    private List<GameObject> activePlane = new List<GameObject>();
    private float spawnPos = 0;
    private float planeLength = 100;
    [SerializeField] private Transform player;
    private int startPlanes = 7;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlane(0);
        for (int i = 0; i < startPlanes; i++)
        {
            SpawnPlane(Random.Range(1, planePrefabs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player.position.z - 60 > spawnPos - (startPlanes * planeLength))
        {
            SpawnPlane(Random.Range(0, planePrefabs.Length));
            DeletePlane();
        }
    }
    private void SpawnPlane(int planeIndex)
    {
        GameObject nextPlane = Instantiate(planePrefabs[planeIndex], transform.forward * spawnPos, transform.rotation);
       activePlane.Add(nextPlane);
        spawnPos += planeLength;
    }
    private void DeletePlane()
    {
        Destroy(activePlane[0]);
        activePlane.RemoveAt(0);
    }
}
