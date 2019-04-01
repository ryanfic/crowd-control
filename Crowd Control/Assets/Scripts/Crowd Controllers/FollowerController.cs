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
        influence=0f;
        Invoke("initalizeAOI",4);
        //base.Start();
        riotLocation = gameObject.transform.position;
        setFinalDestination();
        Invoke("Move",4);
        Destroy(transform.GetChild(0).gameObject);
        
    }
    void initalizeAOI(){
        riotLocation = getNearestAOI().transform.position;
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
        //gameObject.GetComponent<MeshRenderer>().material = material[1];
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(230/255f, 126/255f, 34/255f, 1f);//241f/255f, 90f/255f, 34f/255f, 1f);
        SkinnedMeshRenderer[] mesh = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer m in mesh)
        {
            m.material.color = Color.magenta; //new Color(241f/255f, 90f/255f, 34f/255f, 1f);;
        }
        Move();
        //influence=-influence;
        //updateAOInf();
    }
    private void stopRioting(){
        rioting = false;
        SkinnedMeshRenderer[] mesh = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer m in mesh)
        {
            m.material.color = Color.yellow;
        }
        //gameObject.GetComponent<MeshRenderer>().material = material[0];
        Move();
        //influence=-influence;
        //updateAOInf();
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
                    if(ig!=null&&ig.getFinalDestination()!=null){
                        hasRiotLocation = true;
                        riotLocation = ig.getFinalDestination();
                        //gameObject.GetComponent<NavMeshAgent>().SetDestination(riotLocation);
                    }
                }
        }
    }
}
