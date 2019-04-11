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
    private NavMeshPath path;
    private float elapsed;
    private Vector3 destination;
    private Vector3 target; 
    private float pathUpdateFrequency = 2.0f;

    void Start()
    {
        influence=0f;
        Invoke("initalizeAOI",4);
        //base.Start();
        riotLocation = gameObject.transform.position;
        setFinalDestination();
        //Invoke("Move",4);
        //moving = true;
        Destroy(transform.GetChild(0).gameObject);
        ctype = CrowdType.Follower;
        agent = gameObject.GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        elapsed = pathUpdateFrequency;
        StartCoroutine(TravelToDestination());
    }
    void initalizeAOI(){
        riotLocation = getNearestAOI().transform.position;
    }
    void Update(){
        if(moving&&gameObject.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathInvalid)
        {
            //Debug.Log("Path invalid, updating");
            Move();
        }
        if(!rioting && riotlevel>=riotthreshold)
        {
            startRioting();
        }
        else if(rioting && riotlevel<=riotthreshold){
            stopRioting();
        }
             elapsed += Time.deltaTime;
        if (elapsed > pathUpdateFrequency)
        {
         elapsed -= pathUpdateFrequency;
         NavMesh.CalculatePath (transform.position, destination, agent.areaMask, path);
         agent.SetPath (path);
        }
        
    }

    private void startRioting(){
        rioting = true;
        gameObject.GetComponent<MeshRenderer>().material = material[1];//When using capsules
        //gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(230/255f, 126/255f, 34/255f, 1f);//241f/255f, 90f/255f, 34f/255f, 1f);//when using human model
        SkinnedMeshRenderer[] mesh = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer m in mesh)
        {
            m.material.color = Color.magenta; //new Color(241f/255f, 90f/255f, 34f/255f, 1f);;
        }
        if(moving)
        {
            Move();
        }
        
        //influence=-influence;
        //updateAOInf();
    }
    private void stopRioting(){
        rioting = false;
        /*SkinnedMeshRenderer[] mesh = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();//If using human models
        foreach(SkinnedMeshRenderer m in mesh)
        {
            m.material.color = Color.yellow;
        }*/
        gameObject.GetComponent<MeshRenderer>().material = material[0]; //if using capsules
        if(moving)
        {
            Move();
        }
        //influence=-influence;
        //updateAOInf();
    }
    private void updateAOInf(){
        gameObject.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(influence);
    }
    protected override void Move()
    {
        if(rioting){
            //gameObject.GetComponent<NavMeshAgent>().SetDestination(riotLocation);
            destination = riotLocation;
        }
        else{
            //gameObject.GetComponent<NavMeshAgent>().SetDestination(finalDestination);
            destination = finalDestination;
        }
    }
     IEnumerator TravelToDestination ()
    {
     //print ("Traveling towards destination..");
     destination = target;
     while (Vector3.Distance (transform.position, target) > 0.16f)
     {
         yield return null;
     }
     agent.Warp(destination);
     //print("Reached destination!");
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
    /* void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "AreaOfInfluence"){
            if(true){
                if(other.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Instigator"))
                    ig=other.gameObject.transform.parent.GetComponent<InstigatorController>();
                    if(ig!=null&&ig.getFinalDestination()!=null){
                        hasRiotLocation = true;
                        riotLocation = ig.getFinalDestination();
                        //gameObject.GetComponent<NavMeshAgent>().SetDestination(riotLocation);
                    }
                }
        }
    }*/
}
