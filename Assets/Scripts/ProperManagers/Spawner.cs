using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;

    public GameObject SpawnObject()
    {
        return Instantiate(spawnObject, transform.position, Quaternion.identity);
    }

    public GameObject SpawnObject(GameObject obj)
    {
        return Instantiate(obj, transform.position, Quaternion.identity);
    }

    public static GameObject SpawnObject(GameObject obj, Vector3 position)
    {
        return Instantiate(obj, position, Quaternion.identity);
    }
}
