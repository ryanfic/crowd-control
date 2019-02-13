using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceController : MonoBehaviour
{
    public int PoliceNum;
    //public float speed; //For UDLR Movement
    //private Rigidbody rb;//For UDLR Movement

    //private NavMeshAgent agent;//For NavMeshAgentMovement

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();//For UDLR Movement
        //agent = this.GetComponent<NavMeshAgent>(); //For NavMeshAgent movement
    }


    void FixedUpdate()
    {
        //Up/Down/Left/Right movement
        /*float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal,0.0f,moveVertical);
        rb.AddForce(movement*speed);*/

        //Set destination by casting a ray and using NavMeshAgent
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit, 100))
            {
                agent.destination = hit.point;
            }
        }*/
    }
}
