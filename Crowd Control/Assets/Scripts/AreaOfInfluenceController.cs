using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfInfluenceController : MonoBehaviour
{
    private float influence;

    public void setInfluence(float inf){
        influence = inf;
    }
    void OnTriggerStay(Collider other){
        if(/*GameObject.ReferenceEquals(other.gameObject,gameObject.transform.parent)*/other.gameObject.transform==gameObject.transform.parent.transform){
            Physics.IgnoreCollision(other,gameObject.GetComponent<SphereCollider>());
        }
        //Debug.Log(""+ other.gameObject.GetInstanceID()+ gameObject.transform.parent.GetInstanceID());
        /*FollowerController fc = other.GetComponent<FollowerController>();
        if(fc!=null){
            fc.beInfluenced(influence);
        }*/
        else{
            other.GetComponent<FollowerController>()?.beInfluenced(influence);
        }
    }
}
