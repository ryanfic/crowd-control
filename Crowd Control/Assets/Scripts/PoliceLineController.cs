using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceLineController : MonoBehaviour
{
    public GameObject policeTemplate; //the template for the police, used to generate the police in the line
    public IList<GameObject>  police = new List<GameObject>(); // the list of the police in the line

    private int numPolice=0; // the number of police in the line
    public Vector3 targetPosition; // used in non nav mesh agent movement

    private float targetAccuracy = 5f; // How close the object has to get to the target before it says it has reached it, used in non nav mesh agent movement

    public float rotSpeed = 5; //used in non nav mesh agent movement
    public float speed = 10; // used in non nav mesh agent movement
    Vector3 lookAtTarget; // used in non nav mesh agent movement
    Quaternion lineRot; //used in non nav mesh agent movement
    public bool moving = false; //used in non nav mesh agent movement
    public float overlap = 0.1f; // the overlap of the police in the line, to ensure the crowd doesnt fall through the line as they are pushed

    private NavMeshAgent agent; //used for nav mesh agent movement



    private Queue<Vector3> waypoints = new Queue<Vector3>();
    private Queue<float> waypointDelay = new Queue<float>();


    void Start()
    {
        SpawnPolice();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update(){
        //if the agent has reached its destination
        if(reachedNextWaypoint())
        {
            //if has another waypoint
            if(waypoints.Count>0)
            {
                //move to next waypoint
                moveToNextWaypoint();
                //Debug.Log("Moving to next waypoint!");
            }
            //otherwise switch to standing still mode
             else
            {
                //disable navmeshagent
                agent.enabled = false;
                //add obstacles back to police
                PoliceController pc;
                foreach(GameObject p in police)
                {
                    pc = p.GetComponent<PoliceController>();
                    pc.enableNMObstacle();
                }
            }
        }
    }

    private void SpawnPolice()
    {

        Vector3 leftside = GetComponent<LineRenderer>().GetPosition(0);
        Vector3 rightside = GetComponent<LineRenderer>().GetPosition(1);
        
        float length = Vector3.Distance(rightside,leftside);
        int count = 0;
        for(float i = 0; i <= length; i += (2*policeTemplate.GetComponent<CapsuleCollider>().radius-overlap))
        {
            //Makes the relative position an interpolation (lerp) with the fraction i (i is count diameters away from the left position)
            Vector3 relativepos = Vector3.Lerp(leftside,rightside, i/length); 
            relativepos = gameObject.transform.rotation*relativepos;
            Vector3 spawnLocation = new Vector3(relativepos.x+transform.localPosition.x,relativepos.y+transform.localPosition.y+1,relativepos.z+transform.localPosition.z);
            Quaternion spawnRotation = Quaternion.identity;
            GameObject p = Instantiate(policeTemplate,spawnLocation,spawnRotation,this.transform);
            police.Add(p);
            p.GetComponent<PoliceController>().PoliceNum=count;
            count++;
        }
        //Debug.Log("Police" +count);
        numPolice += count;


        /*float xlength = rightside.x - leftside.x;
        int count = 0;
        for(float i = leftside.x; i<= rightside.x; i += 2*police.GetComponent<CapsuleCollider>().radius )
        {
            Vector3 spawnLocation = new Vector3(i+transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
            Quaternion spawnRotation = Quaternion.identity;
            GameObject p = Instantiate(police,spawnLocation,spawnRotation,this.transform);
            p.GetComponent<PoliceController>().PoliceNum=count;
            count++;
        } */
    }
    //check if the police line has reached the next waypoint
    private bool reachedNextWaypoint()
    {
        //if the agent has reached its destination
        if(agent.enabled&&!agent.pathPending)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath||agent.velocity.sqrMagnitude ==0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //move the police line to the nearest bus stop
    public void moveToNearestBusStop(IList<GameObject> stops)
    {
        GameObject targetStop = getNearestBusStop(stops);
        moveToGivenPosition(targetStop.transform.position);
    }

    //get the nearest bus stop
    private GameObject getNearestBusStop(IList<GameObject> stops)
    {
        float shortestdistance = Mathf.Infinity;
        GameObject closeststop = null;
        foreach(GameObject stop in stops)
        {
            if(stop.gameObject.tag == "BusStop" && Vector3.Distance(stop.transform.position,transform.position) < shortestdistance)
            {
                shortestdistance = Vector3.Distance(stop.transform.position,transform.position);
                closeststop = stop;
            }
        }
        return closeststop;
    }
    //move the police line to the mouse's position
    public void moveToMousePosition(Camera cam)
    {
        Ray ray = /*Camera.main */cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {

            if(hit.collider.tag == "Ground"||hit.collider.tag =="BusStop"){
                moveToGivenPosition(hit.point);
            }
        }
    }
    //move the police line
    public void moveToGivenPosition(Vector3 pos)
    {
        PoliceController pc;
        foreach(GameObject p in police)
        {
            pc = p.GetComponent<PoliceController>();
            pc.disableNMObstacle();
        }
        //setTargetPosition(pos); //use non-nav mesh agent based movement
        AgentMove(pos); //use nav mesh agent based movement
    }

    //begins moving the police line using non nav mesh agent movement
    private void setTargetPosition(Vector3 targetpos)
    {
        targetPosition = targetpos;
        //Snaps policeline to target
        //this.transform.LookAt(targetPosition);

        //makes a target direction for the policeline to look at (using the ray x and z, but keeping the policeline's y value)
        lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y,targetPosition.z - transform.position.z);
        //the rotation to the new target
        lineRot = Quaternion.LookRotation(lookAtTarget);
        //set moving to true
        moving = true;
    }
    //move to the next waypoint
    public void moveToNextWaypoint(){
            moveToGivenPosition(waypoints.Dequeue());
    }

    //instantly move the police line to the next waypoint
    public void warpToNextWaypoint(){
            
            if(waypoints.Count>0&&waypointDelay.Count>0){
                transform.position = waypoints.Dequeue();
                waypointDelay.Dequeue();
            }
    }
    //check if the police line is moving
    public bool isMoving()
    {
        return moving;
    }

    //A non nav mesh agent method of moving
    public void Move()
    {
        //Rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, lineRot, rotSpeed * Time.deltaTime);
        //Move
        //ector3 destination = (targetPosition-transform.position);
        //destination *= Vector3.Distance(targetPosition,transform.position)<(speed * Time.deltaTime)?1:(speed * Time.deltaTime);
        //Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        //Debug.Log("Attempting to elevate");
        //destination = correctElevation(destination);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);;
        //if the line has reached its target position, stop
        //may want a range instead of an exact location
        if(Vector3.Distance(transform.position,targetPosition)<targetAccuracy)
        {
            moving = false;
        }
    }
    private Vector3 correctElevation(Vector3 pos){
        Debug.Log("Attempting to elevate");
        pos.y=pos.y+100;
        Ray ray = new Ray(pos,Vector3.down*1000);
        RaycastHit hit;
        /*Debug.DrawRay(pos,Vector3.down, Color.green, 500); */
		if(Physics.Raycast(ray, out hit)) {
			if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Buildings")) {
                Vector3 location = new Vector3(hit.point.x,hit.point.y,hit.point.z);
                return location;
                Debug.Log("Hit " +hit.transform.gameObject.name +" at: " +location); 
			}
            else{
                Debug.Log("building " +pos);
                return pos;
            }
		}
        else {
            Debug.Log("No hit from " +pos);
            return pos;
        }
    }

    //move police line using nav mesh agent
    public void AgentMove(Vector3 targetpos)
    {
        //Debug.Log("Moving");
        agent.enabled = true;
        agent.SetDestination(targetpos);
    }

    //get the number of police agents in the police line
    public int getNumPolice(){
        return numPolice;
    }

    //add multiple more waypoints and delays to the waypoints
    public void addWaypoints(Queue<Vector3> points,Queue<float> delays)
    {
        foreach(Vector3 point in points)
        {
            waypoints.Enqueue(point);
        }
        foreach(float delay in delays)
        {
            waypointDelay.Enqueue(delay);
        }
    }
    //add a single waypoint and delay to that waypoint
    public void addWaypoints(Vector3 point, float delay)
    {
        waypoints.Enqueue(point);
        waypointDelay.Enqueue(delay);
    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer ==19/*Lawful layer */||collider.gameObject.layer ==18 /*Follower layer */||collider.gameObject.layer==17/*Instigator layer */){
            collider.gameObject.GetComponent<CrowdController>().policeContact();
        }
    }
    //To be implemented later
    /* When things are about to collide with the Policeline AKA when the police line needs to squishtogether*/
    //void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer ==13/*Police layer ||collision.gameObject.layer ==12 /*Crowd layer*/ ||collision.gameObject.layer==14/*PoliceLine layer*/ ){
            /*Debug.Log("Ignoring" + collision.gameObject.name);*/
    //        Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(),GetComponent<Collider>());
    //    }
        /* if(collision.gameObject.layer==9/*Building layer )
        {
            Debug.Log("Building collision");
            
            /*Vector3 closestpoc = new Vector3(0,1000,0); //closest point of contact
            ContactPoint[] points = new ContactPoint[collision.contactCount];
            collision.GetContacts(points);
            foreach(ContactPoint point in points)
            {
                Vector3 distance = point.point - transform.position;
                if(distance.magnitude<closestpoc.magnitude){
                    closestpoc=point.point;
                }
            }
            Vector3 direction = closestpoc - transform.position;
            Debug.Log("Closest point: " + closestpoc);
            Debug.DrawRay(transform.position,direction, Color.green, 500);*/
            /*squishTogether();
        }*/
    //}
    /*If the police line will collide with a Building (collision with a collider on building layer), shrink the line */
    public void squishTogether(){
        foreach(GameObject officer in police)
        {
            officer.GetComponent<PoliceController>().squishTogether();
        }
    }
}
