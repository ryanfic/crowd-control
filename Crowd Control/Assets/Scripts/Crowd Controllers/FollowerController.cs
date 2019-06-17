using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerController : LeavingCrowdController
{
    protected float riotthreshold = 80f; //the threshold that has to be reached before the follower starts rioting
    public float riotlevel = 0f; //the value that must overcome the threshold to start rioting
    public bool rioting = false; //is the follower rioting

    protected Vector3 riotLocation; //the location the follower will move to when rioting

    public Material[] material = new Material[2]; //the materials for the follower, changes when rioting or not
    private NavMeshPath path; //used for movement
    private float elapsed; //used for movement, time since last update
    private Vector3 destination; //used for movement
    private Vector3 target; //used for movement
    private float pathUpdateFrequency = 2.0f; // used for movement, how frequently the path of the follower is updated

    void Start()
    {
        influence=0f;
        Invoke("initalizeAOI",4); //sets the riot location
        riotLocation = gameObject.transform.position; //initially set the riot location to this location
        setFinalDestination(); //sets the final destination, to be moved to upon movement when not rioting
        Destroy(transform.GetChild(0).gameObject); //remove the area of influence, the follower does not influence others
        ctype = CrowdType.Follower; //sets the type of the follower
        agent = gameObject.GetComponent<NavMeshAgent>(); 
        path = new NavMeshPath();
        elapsed = pathUpdateFrequency; //initially set the time elapsed to be equal to the time it takes for the location of the follower to be updated, ensuring the follower is updated
        StartCoroutine(TravelToDestination()); //what causes the movement
    }
    //sets the riot location
    void initalizeAOI(){
        riotLocation = getNearestAOI().transform.position; //gets the center of the riot, sets the riot location to that
    }
    void Update(){
        //if the path is invalid, update it
        if(moving&&gameObject.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathInvalid)
        {
            //Debug.Log("Path invalid, updating");
            Move();
        }
        //start rioting if the riot level is greater than the threshold and the follower is not already rioting
        if(!rioting && riotlevel>=riotthreshold)
        {
            startRioting();
        }
        //stop rioting if the riot level is lower than the threshold and the follower is rioting
        else if(rioting && riotlevel<=riotthreshold){
            stopRioting();
        }
        //update the time that has elapsed
        elapsed += Time.deltaTime;
        //if the time elapsed is greater than the update frequency
        if (elapsed > pathUpdateFrequency)
        {
            //make the elapsed time less than the update frequency
            elapsed -= pathUpdateFrequency;
            //Calculates the path to the current destination
            NavMesh.CalculatePath (transform.position, destination, agent.areaMask, path);
            //update the path on the nav mesh agent
            agent.SetPath (path);
        }
        
    }
    //begins rioting
    private void startRioting(){
        rioting = true;
        gameObject.GetComponent<MeshRenderer>().material = material[1];//When using capsules, updates the color of the follower
        //gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(230/255f, 126/255f, 34/255f, 1f);//241f/255f, 90f/255f, 34f/255f, 1f);//when using human model
       /* SkinnedMeshRenderer[] mesh = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(); //if using human models
        foreach(SkinnedMeshRenderer m in mesh)
        {
            m.material.color = Color.magenta; //new Color(241f/255f, 90f/255f, 34f/255f, 1f);;
        }*/
        //move if moving
        if(moving)
        {
            Move();
        }

    }
    //stops rioting
    private void stopRioting(){
        rioting = false;
        /*SkinnedMeshRenderer[] mesh = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();//If using human models
        foreach(SkinnedMeshRenderer m in mesh)
        {
            m.material.color = Color.yellow;
        }*/
        gameObject.GetComponent<MeshRenderer>().material = material[0]; //if using capsules, update color of follower
        //move if moving
        if(moving)
        {
            Move();
        }
    }

    protected override void Move()
    {
        //if rioting, set the current destination to the riot location
        if(rioting){
            //gameObject.GetComponent<NavMeshAgent>().SetDestination(riotLocation);
            destination = riotLocation;
        }
        //if not rioting, set the current destination to the exit point
        else{
            //gameObject.GetComponent<NavMeshAgent>().SetDestination(finalDestination);
            destination = finalDestination;
        }
    }
    //moves towards the destination
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
     //influences the agent
    public void beInfluenced(float influence){
        //if the influence is a positive value
        if(influence>0){
            //and the riot level will not go above the maximum (100)
            if(riotlevel+influence<=100f)
                riotlevel += influence;//increase the riot level by the influence amount
            //if the riot level will go above the maximum (100)
            else
            {
                //set the riot level to the maximum
                riotlevel =100f;
            }
        }
        //if the influence is a negative value
        else{
            //if the riot level will not go below the minimum (0)
            if(riotlevel+influence>=0f)
                riotlevel += influence;//decrease the riot level by the influence amount
            //if the riot level will go below the minimum (0)
            else
            {
                //set the riot level to the minimum
                riotlevel=0f;
            }
        }
    }
}
