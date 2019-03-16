using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitController : MonoBehaviour
{
    protected float waitTime = 1f;
    // Start is called before the first frame update
    public int capacity; //number of people on bus
    public int maxcapacity; //maximum number of people allowed on bus

    void Start()
    {
        waitAtStop();
    }
    public bool atMaxCapacity()
    {
        
        return (capacity == maxcapacity);
    }

    //Increases the number of people on the bus
    public void getOn(int num)
    {
        if(capacity+num > maxcapacity)
        {
            capacity=maxcapacity;
        }
        else
        {
            capacity += num;
        }
    }

    public void waitAtStop()
    {
        Invoke("moveToNextStop", waitTime);
    }
    protected void moveToNextStop()
    {
        Destroy(gameObject);
        //Debug.Log("Moved to next stop.");
    }
    public float getWaitTime()
    {
        return waitTime;
    }
}
