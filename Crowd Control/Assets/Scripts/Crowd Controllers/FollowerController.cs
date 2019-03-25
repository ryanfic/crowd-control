using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerController : LeavingCrowdController
{
    protected float riotthreshold = 80f;
    public float riotlevel = 0f;
    public bool rioting = false;
    protected bool hasRiotLocation = false;
    protected Vector3 riotLocation;
    protected InstigatorController ig;
    public Material[] material = new Material[2];

    void Start()
    {
        Invoke("initalizeAOI",4);
        base.Start();
        
    }
    void initalizeAOI(){
        finalDestination = getNearestAOI().transform.position;
    }
    void Update(){
        if(!rioting && riotlevel>=riotthreshold)
        {
            startRioting();
        }
        else if(rioting && riotlevel<=riotthreshold){
            stopRioting();
        }
    }

    private void startRioting(){
        rioting = true;
        gameObject.GetComponent<MeshRenderer>().material = material[1];
        Move();
        influence=-influence;
        updateAOInf();
    }
    private void stopRioting(){
        rioting = false;
        gameObject.GetComponent<MeshRenderer>().material = material[0];
        Move();
        influence=-influence;
        updateAOInf();
    }
    private void updateAOInf(){
        gameObject.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(influence);
    }
    protected void Move()
    {
        if(rioting){
            gameObject.GetComponent<NavMeshAgent>().SetDestination(riotLocation);
        }
        else{
            gameObject.GetComponent<NavMeshAgent>().SetDestination(finalDestination);
        }
    }
    public void beInfluenced(float influence){
        if(influence>0){
            if(riotlevel+influence<=100f)
                riotlevel += influence;
            else
            {
                riotlevel =100f;
            }
        }
        else{
            if(riotlevel+influence>=0f)
                riotlevel += influence;
            else
            {
                riotlevel=0f;
            }
        }
        
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "AreaOfInfluence"){
            if(/*!hasRiotLocation*/true){
                if(other.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Instigator"))
                    ig=other.gameObject.transform.parent.GetComponent<InstigatorController>();
                    if(ig!=null){
                        hasRiotLocation = true;
                        riotLocation = ig.getFinalDestination();
                        //gameObject.GetComponent<NavMeshAgent>().SetDestination(riotLocation);
                    }
                }
        }
    }
}
