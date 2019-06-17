using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimulationController : MonoBehaviour
{
    public Camera maincam;

    /*The Start time for the simulation, in real world seconds */
    /*1 hour = 3600 seconds; 12PM = 43200 */
    public float simulationStartTime = 0f;

    public GameObject PoliceLinetemplate;
    public IList<GameObject> PoliceLines = new List<GameObject>();
    public int pltot = 0;

    public int policeTotal =0;

    public GameObject AOITemplate;


    public GameObject crowdtemplate;
    public IList<GameObject> crowdlist = new List<GameObject>();
    public int crowdtot = 0;

    public int RlCrowdTot = 0;

    public GameObject BusStoptemplate;
    public IList<GameObject> BusStops = new List<GameObject>();
    public int stopstot = 0;

    private PoliceLineController plcontroller;

    private bool makingPL = false;
    public Vector3[] PLpoint = new Vector3[2];
    public int PLpointcount = 0;

    private TransitStopGenerator transitGenerator;

    private float timeScale = 3.394f;

    // Start is called before the first frame update
    void Start()
    {
        transitGenerator = gameObject.GetComponent<TransitStopGenerator>();
        plcontroller = PoliceLinetemplate.GetComponent<PoliceLineController>();
        List<GameObject> l = transitGenerator.GenerateStops();
        foreach(GameObject stop in l)
        {
            BusStops.Add(stop);
            stopstot++;
        }

        /*Scenario 1 */
        /*for(int i=0;i<10;i++){
                 StartCoroutine(delaySpawnCrowdAroundSpot(new Vector3((float)(-66+i*5),29f,(float)(50-5*i)), 100,15,3));
            
        }*/
        
        /* Quaternion rot = new Quaternion (0.0f,-0.4f,0.0f,0.9f);
        Queue<Vector3> waypoints = new Queue<Vector3>();
        Queue<float> delays = new Queue<float>();
        //delays.Enqueue(10f); //For Vancouver Conference Run
        waypoints.Enqueue(new Vector3(-66.6f, 31.5f, 47.6f));
        //96.25958-15.32647 = 81
                delays.Enqueue(22+2f);
        waypoints.Enqueue(new Vector3(-97.0f, 31.1f, 78.9f));
        //153.9031-106.9043 = 47
                delays.Enqueue(13+2f);
        waypoints.Enqueue(new Vector3(-130.1f, 32.6f, 112.7f));
        //210.5616-161.3619 = 50
                delays.Enqueue(13+2f);
        waypoints.Enqueue(new Vector3(-155.5f, 33.0f, 130.3f));
        //252.7543-214.8141 = 38
                delays.Enqueue(10+2f);
        waypoints.Enqueue(new Vector3(-196.6f, 33.9f, 173.7f));
        //317.9051-257.4592 = 61
                delays.Enqueue(15.5f+2f);
        waypoints.Enqueue(new Vector3(-224.9f, 34.2f, 203.6f));
        //368.6794-323.5688 = 46
                delays.Enqueue(12+2f);
        waypoints.Enqueue(new Vector3(-276.5f, 34.8f, 252.3f));
        //435.9674-372.0013 = 64
                delays.Enqueue(16+2f);
        waypoints.Enqueue(new Vector3(-311.9f, 36.3f, 258.4f));*/
        //StartCoroutine(delayAddPL(new Vector3((float)(-4.0),27.3f,(float)(-10.6)),rot,3,waypoints,delays));//first simulation pl location
        //StartCoroutine(delayAddPL(new Vector3((float)(-23.6),28.0f,(float)(9.8)),rot,3,waypoints,delays));//second simulation pl location to test moving around pl

        Queue<float> delays = new Queue<float>();
          //FIRST STREET: Start PL from -34.7, 35.0, 97.0  
         //destination: (-170.8, 24.7, -73.6)
         Queue<Vector3> fwaypoints = new Queue<Vector3>();
        //original waypoint towards VPL
        //fwaypoints.Enqueue(new Vector3(-154.7f, 26.4f, -24.1f));

        //second waypoint towards VPL
        //fwaypoints.Enqueue(new Vector3(-170.8f, 24.7f, -73.6f));

        //Updated trial location towards VPL
        //fwaypoints.Enqueue(new Vector3(-199.4f, 24.7f, -69.3f));

        //End point away from VPL
        fwaypoints.Enqueue(new Vector3(45.2f, 31.2f, 177.1f));

        //rotation for going towards VPL
        //Quaternion frot = Quaternion.Euler(0f,-135.2f,0f);
        //rotation for going away from VPL
        Quaternion frot = Quaternion.Euler(0f,44.636f,0f);
        

        //Location moving towards VPL
        //StartCoroutine(delayAddPL(new Vector3(-40.3f,30.9f,89.5f),frot,3,fwaypoints,delays));
        // Location moving away from VPL
        //StartCoroutine(delayAddPL(new Vector3(-147.9f, 26.8f, -18.4f),frot,3,fwaypoints,delays));
        

         //SECOND STREET: Start PL from -78.3, 35.0, 123.5   Move to:  -197.7, 27.4, -3.4
         Queue<Vector3> swaypoints = new Queue<Vector3>();
        //original location towards VPL
        //swaypoints.Enqueue(new Vector3(-197.7f, 27.4f, -3.4f));
        //Updated trial location towards VPL
        //swaypoints.Enqueue(new Vector3(-234.0f, 25.7f, -33.6f));

        //End point away from VPL
        swaypoints.Enqueue(new Vector3(12.4f, 31.5f, 216.8f));

        //rotation for going towards VPL
        //Quaternion srot = Quaternion.Euler(0f,-135.2f,0f);
        //rotation for going away from VPL
        Quaternion srot = Quaternion.Euler(0f,44.636f,0f);

        //Location moving towards VPL
        //StartCoroutine(delayAddPL(new Vector3(-78.3f, 32.5f, 123.5f),srot,3,swaypoints,delays));
        //Location moving away from VPL
        //StartCoroutine(delayAddPL(new Vector3(-187.1f, 28.0f, 14.8f),srot,3,swaypoints,delays));

         //THIRD STREET: Start PL from -94.2, 35.0, 176.8   Move to:  -221.9, 29.1, 37.2
         //New destination: (-324.5, 26.9, -17.3)
        Queue<Vector3> twaypoints = new Queue<Vector3>();

        //first trial location toward VPL
        //twaypoints.Enqueue(new Vector3(-212.3f, 29.1f, 56.5f));
        //updated trial location toward VPL
        //twaypoints.Enqueue(new Vector3(-292.8f, 26.1f, -30.1f));
        //Move out of the way toward VPL
        //twaypoints.Enqueue(new Vector3(-324.5f, 26.9f, -17.3f));

        //End point away from VPL
        twaypoints.Enqueue(new Vector3(5.9f, 30.6f, 275.3f));
        

        //rotation for going towards VPL
        //Quaternion trot = Quaternion.Euler(0f,-135.2f,0f);
        //rotation for going away from VPL
        Quaternion trot = Quaternion.Euler(0f,44.636f,0f);

        //Location moving towards VPL
        //StartCoroutine(delayAddPL(new Vector3(-113.6f, 33.2f, 158.2f),trot,3,twaypoints,delays));
        //Location moving away from VPL
        StartCoroutine(delayAddPL(new Vector3(-221.5f, 29.2f, 44.5f),trot,3,twaypoints,delays));
        
        
        Invoke("addAOI",0);
        //Invoke("MovePLs",4);
        //move to -6.8 28 -3.2
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if(Input.GetKeyDown("n"))
        {
            resetPLPoint();
            foreach(GameObject line in PoliceLines)
            {
                    line.GetComponent<PoliceLineController>().moveToNearestBusStop(BusStops);
            }
        }
        if(Input.GetKeyDown("i")) 
        {
            resetPLPoint();
			Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit)) {

				if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Buildings")) {

					Debug.Log("Bus Stop created.");
                    Vector3 spawnLocation = new Vector3(hit.point.x,hit.point.y,hit.point.z);
					addBusStop(spawnLocation);
				}
			}
		}

        if(Input.GetKeyDown("w"))
        {
            printMouseLocation();
        }
        if(Input.GetKeyDown("e"))
        {
            foreach(GameObject pl in PoliceLines){
                Debug.Log("Started moving through waypoints at: " + Time.time);
                pl.GetComponent<PoliceLineController>().moveToNextWaypoint();
            }
        }
    }
    void addAOI(){
        Vector3 pos = new Vector3(-117.3f,33f,103.5f);
        Quaternion spawnRotation = Quaternion.Euler(0f,44.69f,0f);//new Quaternion(0.0f,0.4f,0.0f,0.9f);
        GameObject aoi = Instantiate(AOITemplate, pos, spawnRotation);
    }
    void addCrowd(Vector3 pos)
    {
        Quaternion spawnRotation = Quaternion.identity;
        //pos.y = hitGroundAtPos(pos).y;
        GameObject ncrowd = Instantiate(crowdtemplate, pos, spawnRotation);
        crowdlist.Add(ncrowd);
        crowdtot++;
        CrowdController cScript = (CrowdController)ncrowd.GetComponent("CrowdController");
        RlCrowdTot+=cScript.getValue();
        /*To print how many crowd objects exist */
        /* Debug.Log("New Crowd Total:" + RlCrowdTot);*/
        //CreateObject(baseCrowdAgent, hit.point, "CrowdAgent (" + crowdCount + ")", crowdAgents.transform).GetComponent<NavMeshAgent>();
    }

    void spawnCrowdAroundSpot(Vector3 pos, int num, float radius)
    {
        for(int i = 0; i<num;i++)
        {
            Vector3 spot = (Vector3)Random.insideUnitCircle;
            spot.z=spot.y;
            spot.y=0;
            addCrowd(pos+spot*radius);
        }
    }
    IEnumerator delaySpawnCrowdAroundSpot(Vector3 pos, int num, float radius,int time)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i<num;i++)
        {
            Vector3 spot = (Vector3)Random.insideUnitCircle;
            spot.z=spot.y;
            spot.y=0;
            addCrowd(pos+spot*radius);
        }
    }

    void addPL()
    {
        Vector3 spawnLocation = Vector3.Lerp(PLpoint[0],PLpoint[1],0.5f);
        
        Quaternion spawnRotation; 
        spawnRotation = Quaternion.LookRotation(getNormal2D(PLpoint[0],PLpoint[1]));
        print("Angle: " + spawnRotation);
        GameObject line = Instantiate(PoliceLinetemplate, spawnLocation, spawnRotation);
        line.GetComponent<LineRenderer>().SetPositions(PLpoint);
        PoliceLines.Add(line);
        pltot++;
        

    }
    GameObject addPL(Vector3 pos)
    {
        Quaternion spawnRotation = Quaternion.identity;
        GameObject nline = Instantiate(PoliceLinetemplate, hitGroundAtPos(pos), spawnRotation);
        PoliceLines.Add(nline);
        pltot++;
        return nline;
    }
    IEnumerator delayAddPL(Vector3 pos, Quaternion rot , int time)
    {
        yield return new WaitForSeconds(time);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject nline = Instantiate(PoliceLinetemplate, pos, rot);
        PoliceLines.Add(nline);
        pltot++;
        //MovePL(nline);
        
    }
    IEnumerator delayAddPL(Vector3 pos, Quaternion rot , int time, Queue<Vector3> points,Queue<float> delays)
    {
        yield return new WaitForSeconds(time);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject nline = Instantiate(PoliceLinetemplate, pos, rot);
        nline.GetComponent<PoliceLineController>().addWaypoints(points,delays);
        PoliceLines.Add(nline);
        pltot++;
        //MovePL(nline);
    }
    void MovePL(GameObject pl)
    {
        Debug.Log("Started in simulationcontroller");
        pl.GetComponent<PoliceLineController>().moveToNextWaypoint();
        
        //Debug.Log("Called movement!");
    }
    void MovePLs()
    {
        foreach(GameObject pl in PoliceLines){
                //Debug.Log("Started moving through waypoints at: " + Time.time);
                Debug.Log("Started in simulationcontroller");
                pl.GetComponent<PoliceLineController>().moveToNextWaypoint();
        }
        //Debug.Log("Called movement!");
    }

    private void getNumPolice(GameObject pl){
        foreach(var el in PoliceLines){
            PoliceLineController plScript = (PoliceLineController)el.GetComponent("PoliceLineController");
            policeTotal+=plScript.getNumPolice();
            Debug.Log("New Police Total:" + policeTotal);
        }
    }

    //Gets the left hand normal from two points, assuming that they are in a 2d plane on Y
    //Normal is from the midpoint of the two points
    private Vector3 getNormal2D(Vector3 p1, Vector3 p2)
    {
        Vector3 norm = p1-p2;
        norm.y=0;
        norm.x=-norm.x;
        Vector3 result = Vector3.Lerp(p1,p2,0.5f);
        result = result+norm;
        print("Result: " + result);
        return result;
    }
    public void addBusStop(Vector3 pos)
    {
        Quaternion spawnRotation = Quaternion.identity;
        GameObject busstop = Instantiate(BusStoptemplate, pos, spawnRotation);
        BusStops.Add(busstop);
        stopstot++;
    }

    void resetPLPoint()
    {
        makingPL = false;
        for(int i = 0; i<PLpoint.Length;i++)
        {
            PLpoint[i] = new Vector3(0,0,0);
        }
        PLpointcount = 0;
    }
    
    public float getTime(){
        float curtime = simulationStartTime +(Time.time*timeScale);
        Debug.Log("Time: " + curtime);
        return curtime;
    }

    public void printMouseLocation(){
        Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit)) {

				if(/*hit.transform.gameObject.layer != LayerMask.NameToLayer("Buildings") */true) {
                    Vector3 location = new Vector3(hit.point.x,hit.point.y,hit.point.z);
                    Debug.Log("Mouse Location: " +location);
				}
			}
    }

    Vector3 hitGroundAtPos(Vector3 pos){
        pos.y=pos.y+100;
        Ray ray = new Ray(pos,Vector3.down*1000);
        RaycastHit hit;
        /*Debug.DrawRay(pos,Vector3.down, Color.green, 500); */
			if(Physics.Raycast(ray, out hit)) {
				if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Buildings")) {
                    Vector3 location = new Vector3(hit.point.x,hit.point.y,hit.point.z);
                    return location;
                    /*Debug.Log("Hit " +hit.transform.gameObject.name +" at: " +location); */
				}
			}
            /*else 
            Debug.Log("No hit from " +pos); */
            return pos;
            
    }

}
