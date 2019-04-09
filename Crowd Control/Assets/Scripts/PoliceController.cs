using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceController : MonoBehaviour
{
    
    public int PoliceNum;

    private Vector3 originalPosition;

    //public float speed; //For UDLR Movement
    //private Rigidbody rb;//For UDLR Movement

    //private NavMeshAgent agent;//For NavMeshAgentMovement

    // Start is called before the first frame update
    void Start()
    {
        originalPosition= gameObject.transform.localPosition;
        //rb = GetComponent<Rigidbody>();//For UDLR Movement
        //agent = this.GetComponent<NavMeshAgent>(); //For NavMeshAgent movement
    }


    /*Squish together to allow movement in smaller corridors */
    public void squishTogether()
    {
        Vector3 pos = new Vector3(5,0,0); /*gameObject.transform.localPosition;*/
        Debug.Log("Relocating to" + pos);
        /*pos.x+=5;*/
        gameObject.transform.Translate(pos);
    }
    public void enableNMObstacle()
    {
        gameObject.GetComponent<NavMeshObstacle>().enabled = true;
    }
    public void disableNMObstacle()
    {
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
    }
}
