using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLineController : MonoBehaviour
{
    public GameObject police;
    public Vector3 targetPosition;
    public float rotSpeed = 5;
    public float speed = 10;
    Vector3 lookAtTarget;
    Quaternion lineRot;
    private bool moving = false;



    void Start()
    {
        SpawnPolice();
        
    }

    private void SpawnPolice()
    {
        Vector3 leftside = GetComponent<LineRenderer>().GetPosition(0);
        Vector3 rightside = GetComponent<LineRenderer>().GetPosition(1);
        float xlength = rightside.x - leftside.x;
        int count = 0;
        for(float i = leftside.x; i<= rightside.x; i += 2*police.GetComponent<CapsuleCollider>().radius )
        {
            Vector3 spawnLocation = new Vector3(i+transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
            Quaternion spawnRotation = Quaternion.identity;
            GameObject p = Instantiate(police,spawnLocation,spawnRotation,this.transform);
            p.GetComponent<PoliceController>().PoliceNum=count;
            count++;
        }
    }
    public void setTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {
            targetPosition = hit.point;
            //Snaps policeline to target
            //this.transform.LookAt(targetPosition);

            //makes a target direction for the policeline to look at (using the ray x and z, but keeping the policeline's y value)
            lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y,targetPosition.z - transform.position.z);
            //the rotation to the new target
            lineRot = Quaternion.LookRotation(lookAtTarget);
            //set moving to true
            moving = true;
        }
    }

    public bool isMoving()
    {
        return moving;
    }

    public void Move()
    {
        //Rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, lineRot, rotSpeed * Time.deltaTime);
        //Move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        //if the line has reached its target position, stop
        //may want a range instead of an exact location
        if(transform.position == targetPosition)
        {
            moving = false;
        }
    }
}
