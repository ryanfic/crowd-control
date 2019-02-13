﻿using System.Collections;
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

    public GameObject BusStop;
    public GameObject[] BusStops;

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
        if(Input.GetMouseButton(0))
        {
            if(makingPL)
            {
                Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
			    RaycastHit hit;

			    if(Physics.Raycast(ray, out hit))
                {
                    PLpoint[PLpointcount] = hit.point;
                    PLpointcount++;
                }
                if(PLpointcount == 2)
                {
                    addPL();
                    print(PLpointcount);
                    resetPLPoint();
                    print(PLpointcount);
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
        foreach(GameObject line in PoliceLines)
        {
                    if(line.GetComponent<PoliceLineController>().isMoving())
                    {
                        line.GetComponent<PoliceLineController>().Move();
                    }
        }
        if(PoliceLine.GetComponent<PoliceLineController>().isMoving())
        {

            PoliceLine.GetComponent<PoliceLineController>().Move();
        }
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

			if(Physics.Raycast(ray, out hit)) {

				if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Buildings")) {

					Debug.Log("Crowd agent created.");
                    Vector3 spawnLocation = new Vector3(hit.point.x,1,hit.point.z);
					addCrowd(spawnLocation);
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
        Quaternion spawnRotation = Quaternion.identity;
        GameObject line = Instantiate(PoliceLinetemplate, spawnLocation, spawnRotation);
        PoliceLines.Add(line);
        pltot++;
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