using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CrowdType{Follower, Instigator, Lawful};
public abstract class CrowdController : MonoBehaviour
{
    protected int value = 66; //The number of people represented by a single crowd agent in the simulation
    protected Vector3 finalDestination; // The place the agent wants to go overall

    public bool moving = false; //is the crowd agent moving
    protected float influence = 10f; // how much the agent influences other agents
    
    protected IList<GameObject> crowdinnoi = new List<GameObject>();//agents in neighbourhood of interaction
    protected NavMeshAgent agent; //used for movement

    protected CrowdType ctype; //defines what agent type the crowd agent is

    protected void Start(){
        //moving = true;
        //Invoke("Move",4f);
        agent = gameObject.GetComponent<NavMeshAgent>();
        
    }
    protected void Update(){
        if(moving&&gameObject.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathInvalid)
        {
            //Debug.Log("Path invalid, updating");
            Move();
        }
    }

    // Sets the final place the agent wants to go
    protected abstract void setFinalDestination();
    public Vector3 getFinalDestination(){
        return finalDestination;
    }

    protected virtual void Move(){
        gameObject.GetComponent<NavMeshAgent>().SetDestination(finalDestination);
    }



    //Get the nearest area of interest
    protected GameObject getNearestAOI()
    {
        float shortestdistance = Mathf.Infinity;
        GameObject closestarea = null;
        GameObject[] areas = GameObject.FindGameObjectsWithTag("AreaOfInterest");
        foreach(GameObject area in areas)
        {
            if(Vector3.Distance(area.transform.position,transform.position) < shortestdistance)
            {
                shortestdistance = Vector3.Distance(area.transform.position,transform.position);
                closestarea = area;
            }
        }
        return closestarea;
    }

    public float getInfluence(){
        return influence;
    }


    //If the other object is a bus and it is not at maximum capacity, get on and destroy this object
    /*public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag =="Bus" && !(other.gameObject.GetComponent<BusController>().atMaxCapacity()))
        {
            other.gameObject.GetComponent<BusController>().getOn(value);
            Destroy(gameObject);
        } 
    }*/
    //get the value that this agent represents
    public int getValue()
    {
        return value;
    }

    //A method that is called when another crowd agent enters the neighbourhood of interaction
    public void crowdEnterNOI(GameObject crowd)
    {
        //add that crowd agent to the crowd in neighbourhood of interaction list
        crowdinnoi.Add(crowd);
        //if the other crowd agent is moving and this object is not moving
        if(crowd.GetComponent<CrowdController>().isMoving()&&!moving)
        {
            //begin moving
            moving = true;
            Move();
        }
        //Debug.Log("Entered NOI!");
    }
    //a method that is called when another crowd agent exits the neighbourhood of interaction
    public void crowdExitNOI(GameObject crowd)
    {
        //remove the leaving crowd agent from the list
        crowdinnoi.Remove(crowd);
        //if the other crowd agent is moving and this object is not moving
        if(crowd.GetComponent<CrowdController>().isMoving()&&!moving)
        {
            //begin moving
            moving = true;
            Move();
        }
        //Debug.Log("Exited NOI!");
    }
    //gets the crowd type
    public CrowdType getCrowdType()
    {
        return ctype;
    }
    //a method to be called when police come into contact with this object, makes this object start moving
    public void policeContact()
    {
        moving = true;
        Move();
    }
    //gets if this object is moving
    public bool isMoving()
    {
        return moving;
    }
}
