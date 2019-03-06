using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public Camera maincam;

    public GameObject PoliceLinetemplate;
    public List<GameObject> PoliceLines;
    public int pltot = 0;
    public GameObject PoliceLine;

    public GameObject crowdtemplate;
    public List<GameObject> crowdlist;
    public int crowdtot = 0;

    public GameObject BusStoptemplate;
    public List<GameObject> BusStops;
    public int stopstot = 0;

    private PoliceLineController plcontroller;

    private bool makingPL = false;
    public Vector3[] PLpoint = new Vector3[2];
    public int PLpointcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        plcontroller = PoliceLinetemplate.GetComponent<PoliceLineController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        /*f(Input.GetMouseButtonDown(0))
        {
            if(makingPL)
            {
                Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
			    RaycastHit hit;

			    if(Physics.Raycast(ray, out hit))
                {
                    PLpoint[PLpointcount] = hit.point;
                    print(PLpoint[PLpointcount]);
                    PLpointcount++;
                }
                if(PLpointcount == 2)
                {
                    addPL();
                    //print(PLpointcount);
                    resetPLPoint();
                    //print(PLpointcount);
                }
            }
            else
            {
                foreach(GameObject line in PoliceLines)
                {
                    line.GetComponent<PoliceLineController>().targetMousePosition(maincam);
                }
                PoliceLine.GetComponent<PoliceLineController>().targetMousePosition(maincam);
            }
            
            //PoliceLine.GetComponent<PoliceLineController>().Move();
        }
        /*foreach(GameObject line in PoliceLines)
        {
                    if(line.GetComponent<PoliceLineController>().isMoving())
                    {
                        line.GetComponent<PoliceLineController>().Move();
                    }
        }
        if(PoliceLine.GetComponent<PoliceLineController>().isMoving())
        {

            PoliceLine.GetComponent<PoliceLineController>().Move();
        }*/
        if(Input.GetKeyDown("n"))
        {
            resetPLPoint();
            foreach(GameObject line in PoliceLines)
            {
                    line.GetComponent<PoliceLineController>().targetNearestBusStop(BusStops);
            }
            PoliceLine.GetComponent<PoliceLineController>().targetNearestBusStop(BusStops);
        }
        if(Input.GetKeyDown("o")) 
        {
            resetPLPoint();
			Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            Physics.Raycast(ray, out hit);
			if(Physics.Raycast(ray, out hit)) {

				if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Buildings")) {

					Debug.Log("Crowd agent created.");
                    Vector3 spawnLocation = new Vector3(hit.point.x,hit.point.y,hit.point.z);
					addCrowd(spawnLocation);
				}
			}
            else{
                Debug.Log("didnt hit");
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
        if(Input.GetKeyDown("p"))
        {
            makingPL = true;
        }
    }

    void addCrowd(Vector3 pos)
    {
        Quaternion spawnRotation = Quaternion.identity;
        GameObject ncrowd = Instantiate(crowdtemplate, pos, spawnRotation);
        crowdlist.Add(ncrowd);
        crowdtot++;
        //CreateObject(baseCrowdAgent, hit.point, "CrowdAgent (" + crowdCount + ")", crowdAgents.transform).GetComponent<NavMeshAgent>();
    }

    void addPL()
    {
        Vector3 spawnLocation = Vector3.Lerp(PLpoint[0],PLpoint[1],0.5f);
        
        Quaternion spawnRotation; /*= Quaternion.identity; */
        spawnRotation = Quaternion.LookRotation(getNormal2D(PLpoint[0],PLpoint[1]));
        print("Angle: " + spawnRotation);
        GameObject line = Instantiate(PoliceLinetemplate, spawnLocation, spawnRotation);
        line.GetComponent<LineRenderer>().SetPositions(PLpoint);
        PoliceLines.Add(line);
        pltot++;
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

}
