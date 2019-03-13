using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitStopController : MonoBehaviour
{
    public GameObject TransitTemplate;
    protected float spawnFrequency = 3f; //Time between spawns
    protected float transitWaitTime;// = TransitTemplate.GetComponent<TransitController>().waitTime;

    // Start is called before the first frame update
    void Start()
    {
        transitWaitTime = TransitTemplate.GetComponent<TransitController>().getWaitTime();
        InvokeRepeating("SpawnTransit", 0f, spawnFrequency+transitWaitTime);
    }

    public void SpawnTransit()
    {
        Vector3 spawnLocation = new Vector3(transform.localPosition.x,transform.localPosition.y+1,transform.localPosition.z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(TransitTemplate, spawnLocation, spawnRotation);
        Debug.Log("Transit created.");
    }
}
