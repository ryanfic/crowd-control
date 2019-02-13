using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopController : MonoBehaviour
{
    public GameObject BusTemplate;
    public GameObject Bus;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnLocation = new Vector3(transform.localPosition.x,1,transform.localPosition.z);
        Quaternion spawnRotation = Quaternion.identity;
        Bus = Instantiate(BusTemplate, spawnLocation, spawnRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
