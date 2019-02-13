using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public GameObject PoliceLine;

    private PoliceLineController plcontroller;

    // Start is called before the first frame update
    void Start()
    {
        plcontroller = PoliceLine.GetComponent<PoliceLineController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            PoliceLine.GetComponent<PoliceLineController>().setTargetPosition();
            //PoliceLine.GetComponent<PoliceLineController>().Move();
        }
        if(PoliceLine.GetComponent<PoliceLineController>().isMoving())
        {
            PoliceLine.GetComponent<PoliceLineController>().Move();
        }
        
    }
}
