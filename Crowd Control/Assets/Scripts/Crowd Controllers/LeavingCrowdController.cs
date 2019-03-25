using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeavingCrowdController : CrowdController
{
    private static Vector3 granvilleExit = new Vector3(-732.9075f,21.81492f,-448.9697f);
    private static Vector3 waterfrontStation = new Vector3(175.7381f,16.23377f,584.8232f);
    private static Vector3 granvilleStation = new Vector3(-129.7943f,32.36957f,350.8734f);
    private static Vector3 stadiumChinatownStation = new Vector3(-129.8107f,32.37526f,350.7296f);
    private Vector3[] exits = new Vector3[]{granvilleExit,waterfrontStation,granvilleStation,stadiumChinatownStation};
    // Start is called before the first frame update
    public LeavingCrowdController(){
        influence = -10f;
    }
    void Start()
    {
        
        setFinalDestination();
        base.Start();
        
    }

    // Update is called once per frame

    protected override void setFinalDestination()
    {
        float rand = Random.value*exits.Length;
        finalDestination = exits[(int)Mathf.Floor(rand)];
    }
}
