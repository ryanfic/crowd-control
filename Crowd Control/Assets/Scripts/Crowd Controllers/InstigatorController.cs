using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstigatorController : CrowdController
{
    void Start(){
        Invoke("setFinalDestination",4);
        base.Start();
    }
    public InstigatorController(){
        influence = 10;
    }
    protected override void setFinalDestination(){
        //finalDestination = getNearestAOI().transform.position;
        finalDestination = new Vector3(-6.8f, 28f, -3.2f);
    }
}
