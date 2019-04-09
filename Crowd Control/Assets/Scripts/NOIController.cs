using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOIController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.transform==gameObject.transform.parent.transform){
            Physics.IgnoreCollision(other,gameObject.GetComponent<SphereCollider>());
        }
        
         else{
            other.GetComponent<CrowdController>()?.crowdEnterNOI(gameObject.transform.parent.gameObject);
            //other.GetComponent<FollowerController>()?.beInfluenced(influence);
        }
    }
    private void OnTriggerExit(Collider other) {
        
        other.GetComponent<CrowdController>()?.crowdExitNOI(gameObject.transform.parent.gameObject);
            
    }
}
