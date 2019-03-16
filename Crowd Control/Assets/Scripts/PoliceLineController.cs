using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceLineController : MonoBehaviour
{
    public GameObject policeTemplate;
    public IList<GameObject>  police = new List<GameObject>();

    private int numPolice=0;
    public Vector3 targetPosition;

    public float rotSpeed = 5;
    public float speed = 10;
    public float overlap = 0.1f;
    Vector3 lookAtTarget;
    Quaternion lineRot;
    private NavMeshAgent agent;
    private bool moving = false;

    private float minLength=10f;

    private Queue<Vector3> waypoints = new Queue<Vector3>();
    private Queue<float> waypointDelay = new Queue<float>();


    void Start()
    {
        SpawnPolice();
        agent = GetComponent<NavMeshAgent>();
        //Vector3 pos = transform.position;
        //pos.y = 0;
        //transform.position = pos;
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
    public void targetNearestBusStop(IList<GameObject> stops)
    {
        GameObject targetStop = getNearestBusStop(stops);
        AgentMove(targetStop.transform.position);
        //To get time when start moving to bus stop
        Debug.Log("Started moving at " +Time.time);


        //Using my movement method
        //setTargetPosition(targetStop.transform.position);
        
        //setTargetPosition(targetStop.transform.position);
        //targetPosition = hit.point;

        //makes a target direction for the policeline to look at (using the ray x and z, but keeping the policeline's y value)
        //lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y,targetPosition.z - transform.position.z);
        //the rotation to the new target
        //lineRot = Quaternion.LookRotation(lookAtTarget);
        //set moving to true
        //moving = true;
    }

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
    public void targetMousePosition(Camera cam)
    {
        Ray ray = /*Camera.main */cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {

            if(hit.collider.tag == "Ground"||hit.collider.tag =="BusStop"){
                //setTargetPosition(hit.point);
                AgentMove(hit.point);
            }
        }
    }
    public void targetGivenPosition(Vector3 pos)
    {
        AgentMove(pos);
    }
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
    public void targetNextWaypoint(){
            Debug.Log("Next waypoint: " + waypoints.Peek()+", started moving at: " +Time.time);
            AgentMove(waypoints.Dequeue());
    }
    public void delayedTargetNextWaypoint(){
        targetNextWaypoint();
        if(waypoints.Count>0&&waypointDelay.Count>0){
            float t = waypointDelay.Dequeue();
            Invoke("delayedTargetNextWaypoint",t);
        }
        /* else{
            Debug.Log("Reached the destination!");
        }*/

            
    }
    public void warpToNextWaypoint(){
            
            if(waypoints.Count>0&&waypointDelay.Count>0){
                transform.position = waypoints.Dequeue();
                waypointDelay.Dequeue();
            }
    }
    public bool isMoving()
    {
        return moving;
    }

    public void Move()
    {
        //Rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, lineRot, rotSpeed * Time.deltaTime);
        //Move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        //if the line has reached its target position, stop
        //may want a range instead of an exact location
        if(transform.position == targetPosition)
        {
            moving = false;
        }
    }

    public void AgentMove(Vector3 targetpos)
    {
        //Debug.Log("Moving");
        agent.SetDestination(targetpos);
    }

    public int getNumPolice(){
        return numPolice;
    }
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
    public void addWaypoints(Vector3 point, float delay)
    {
        waypoints.Enqueue(point);
        waypointDelay.Enqueue(delay);
    }
    /* When things collide with the trigger on the Policeline*/
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer ==13/*Police layer ||collision.gameObject.layer ==12 /*Crowd layer*/ ||collision.gameObject.layer==14/*PoliceLine layer*/ ){
            /*Debug.Log("Ignoring" + collision.gameObject.name);*/
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(),GetComponent<Collider>());
        }
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
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer ==13/*Police layer */||collider.gameObject.layer ==12 /*Crowd layer */||collider.gameObject.layer==14/*PoliceLine layer */){
            /*Debug.Log("Ignoring" + collision.gameObject.name);*/
            Physics.IgnoreCollision(collider.gameObject.GetComponent<Collider>(),GetComponent<Collider>());
        }
        if(collider.gameObject.layer==9/*Building layer */)
        {
            Debug.Log("Building triggered");
            foreach(GameObject p in police)
            {
                Ray ray = new Ray(p.transform.position,gameObject.transform.forward);
                RaycastHit hit;
                /*Debug.DrawRay(pos,Vector3.down, Color.green, 500); */
			    if(Physics.Raycast(ray, out hit, 1000)) {
                /*Debug.Log("Hit"); */
				if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Buildings")) {
                    Vector3 location = new Vector3(hit.point.x,hit.point.y,hit.point.z);
                    Debug.Log("Going to collide with a building!" + hit.point);
                    /*Debug.Log("Hit " +hit.transform.gameObject.name +" at: " +location); */
				}
			}
                Debug.DrawRay(p.transform.position,gameObject.transform.forward, Color.green, 500);
            }
        }
    }
    /*If the police line will collide with a Building (collision with a collider on building layer), shrink the line */
    public void squishTogether(){
        foreach(GameObject officer in police)
        {
            officer.GetComponent<PoliceController>().squishTogether();
        }
    }
}
