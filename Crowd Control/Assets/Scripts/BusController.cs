using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusController : MonoBehaviour
{
    public int capacity; //number of people on bus
    public int maxcapacity; //maximum number of people allowed on bus

    public bool atMaxCapacity()
    {
        
        return (capacity == maxcapacity);
    }

    //Increases the number of people on the bus
    public void getOn()
    {
        capacity++;
    }
}
