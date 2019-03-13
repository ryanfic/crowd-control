using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
    public int value; //The number of people represented by a single crowd agent in the simulation
    /*void OnTriggerEnter(Collider other)
    {
        //If the other object is a bus and it is not at maximum capacity, get on and destroy this object
        if(other.gameObject.tag =="Bus" && !(other.gameObject.GetComponent<BusController>().atMaxCapacity()))
        {
            other.gameObject.GetComponent<BusController>().getOn();
            Destroy(gameObject);
            
        } 
    } */
    //If the other object is a bus and it is not at maximum capacity, get on and destroy this object
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag =="Bus" && !(other.gameObject.GetComponent<BusController>().atMaxCapacity()))
        {
            other.gameObject.GetComponent<BusController>().getOn(value);
            Destroy(gameObject);
        } 
    }
}
