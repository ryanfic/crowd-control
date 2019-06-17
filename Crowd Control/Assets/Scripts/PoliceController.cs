using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceController : MonoBehaviour
{
    
    public int PoliceNum; //to hold the number of the police agent in the police line/police squadron

    private Vector3 originalPosition; //To be used with the 

    void Start()
    {
        originalPosition= gameObject.transform.localPosition;
    }

    //To be implemented later
    /*Squish together to allow movement in smaller corridors */
    public void squishTogether()
    {
        //should get the position of the police agent and move it closer to the center of the police line

        //currently just shuffles the agent to the right
        Vector3 pos = new Vector3(5,0,0); /*gameObject.transform.localPosition;*/
        Debug.Log("Relocating to" + pos);
        gameObject.transform.Translate(pos);
    }

    //To be implemented later
    /*Used to spread the police line apart, back to (or closer to) the original position of the police agent */
    public void separate()
    {

    }

    //Turn on the Nav Mesh Obstacle, used when the police line stops moving
    public void enableNMObstacle()
    {
        gameObject.GetComponent<NavMeshObstacle>().enabled = true;
    }
    //turn off the Nav Mesh Obstacle, used when the police line begins moving
    public void disableNMObstacle()
    {
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
    }
}
