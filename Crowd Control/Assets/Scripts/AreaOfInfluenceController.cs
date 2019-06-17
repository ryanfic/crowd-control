using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfInfluenceController : MonoBehaviour
{
    private float influence; //how much the AOI affects a Follower

    //sets the influence of the AOI
    public void setInfluence(float inf){
        influence = inf;
    }
    void OnTriggerStay(Collider other){
        //Ignore parents
        if(/*GameObject.ReferenceEquals(other.gameObject,gameObject.transform.parent)*/other.gameObject.transform==gameObject.transform.parent.transform){
            Physics.IgnoreCollision(other,gameObject.GetComponent<SphereCollider>());
        }
        //if the crowd in the AOI is a follower, influence them
        else{
            other.GetComponent<FollowerController>()?.beInfluenced(influence);
        }
    }
}
