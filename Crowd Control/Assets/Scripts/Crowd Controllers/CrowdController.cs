using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CrowdType{Follower, Instigator, Lawful};
public abstract class CrowdController : MonoBehaviour
{
    protected int value = 66; //The number of people represented by a single crowd agent in the simulation
    protected Vector3 finalDestination; // The place the agent wants to go overall
    protected Vector3 currDestination; // The place the agent is moving due to crowd dynamics
    public bool moving = false;
    protected bool fleeing = false;
    protected float influence = 10f;
    //agents in neighbourhood of interaction
    protected IList<GameObject> crowdinnoi = new List<GameObject>();
    protected NavMeshAgent agent;

    protected CrowdType ctype;

    protected void Start(){
        //moving = true;
        //Invoke("Move",4f);
        agent = gameObject.GetComponent<NavMeshAgent>();
        
    }
    protected void Update(){
        if(Input.GetKeyDown("q")){
            //Debug.Log("Q PRESSED");
            Move();
        }
        if(moving&&gameObject.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathInvalid)
        {
            //Debug.Log("Path invalid, updating");
            Move();
        }
    }
    /*void OnTriggerEnter(Collider other)
    {
        //If the other object is a bus and it is not at maximum capacity, get on and destroy this object
        if(other.gameObject.tag =="Bus" && !(other.gameObject.GetComponent<BusController>().atMaxCapacity()))
        {
            other.gameObject.GetComponent<BusController>().getOn();
            Destroy(gameObject);
            
        } 
    } */

    // Sets the final place the agent wants to go
    protected abstract void setFinalDestination();
    public Vector3 getFinalDestination(){
        return finalDestination;
    }

    //Updates where the agent is currently wanting to go
    public void setCurrDestination(){
        //Default to final Destination

        //If crowd seen is fleeing, flee in the same direction

    }
    protected virtual void Move(){
        //if there are people around moving, update the speed
        //if there are people around moving, update the angle
        gameObject.GetComponent<NavMeshAgent>().SetDestination(finalDestination);
        //Debug.Log("Next spot "+(gameObject.GetComponent<NavMeshAgent>().nextPosition-gameObject.transform.position).ToString());
        /* if(crowdinnoi.Count>0&&gameObject.GetComponent<NavMeshAgent>().nextPosition!=Vector3.zero){
            Vector3 result = gameObject.GetComponent<NavMeshAgent>().nextPosition;//new Vector3(0f,0f,0f);
            foreach(GameObject crowd in crowdinnoi)
            {
                result += (crowd.GetComponent<NavMeshAgent>().nextPosition)/(Vector3.Distance(gameObject.transform.position,crowd.gameObject.transform.position));
            }
            gameObject.GetComponent<NavMeshAgent>().SetDestination(result);
}*/


    }

    //Destination 1 via granville: -732.9075 21.81492 -448.9697
    //Destination 2 via waterfront station: 175.7381 16.23377 584.8232
    //Destination 3 via granville station: -129.7943 32.36957 350.8734
    //Destination 4 via Stadium-chinatown station: -129.8107 32.37526 350.7296

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
    public int getValue()
    {
        return value;
    }

    public void crowdEnterNOI(GameObject crowd)
    {
        crowdinnoi.Add(crowd);
        if(crowd.GetComponent<CrowdController>().isMoving()&&!moving)
        {
            moving = true;
            Move();
        }
        //Debug.Log("Entered NOI!");
    }
    public void crowdExitNOI(GameObject crowd)
    {
        crowdinnoi.Remove(crowd);
        if(crowd.GetComponent<CrowdController>().isMoving()&&!moving)
        {
            moving = true;
            Move();
        }
        //Debug.Log("Exited NOI!");
    }
    public CrowdType getCrowdType()
    {
        return ctype;
    }
    public void policeContact()
    {
        moving = true;
        Move();
    }
    public bool isMoving()
    {
        return moving;
    }
}
