using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstigatorController : CrowdController
{
    void Start(){
        Invoke("setFinalDestination",4);
        ctype = CrowdType.Instigator;
        base.Start();
    }
    public InstigatorController(){
        influence = 10;
    }
    protected override void setFinalDestination(){
        finalDestination = getNearestAOI().transform.position;
    }
}
