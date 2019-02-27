using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed; //How fast the camera moves
    public float rotateSpeed; //How fast the camera rotates
    public float rotateAmount; //How much the camera rotates

    private Quaternion rotation;

    private float panDetect = 15f;
    private float minHeight = 10f;
    private float maxHeight = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.rotation = rotation;
        }
    }

    //moves the camera
    void MoveCamera()
    {
        //camera movement is based off of the main camera
        float moveX = Camera.main.transform.position.x; 
        float moveY = Camera.main.transform.position.y;
        float moveZ = Camera.main.transform.position.z;

        //the x and y components of the mouseposition
        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        // if the left arrow is pressed or the xPos is greater than 0 and within panDetect from the border of the game (within the first 15px of the screen)
        if(Input.GetKey(KeyCode.LeftArrow) || xPos>0 && xPos<panDetect)
        {
            //moves left
            moveX -= panSpeed;
        }
        // if the right arrow is pressed or the xPos is less than the Screen width and within panDetect from the border of the game (within the last 15px of the screen)
        else if(Input.GetKey(KeyCode.RightArrow) || xPos<Screen.width && xPos>Screen.width - panDetect)
        {
            //moves right
            moveX += panSpeed;
        }

        // if the up arrow is pressed or the yPos is less than the screen height and within panDetect from the border of the game (within the first 15px of the screen)
        if(Input.GetKey(KeyCode.UpArrow) || yPos<Screen.height && yPos > Screen.height - panDetect)
        {
            //move up
            moveZ += panSpeed;
        }
        // if the down arrow is pressed or the yPos is greater than 0 and within panDetect from the border of the game (within the last 15px of the screen)
        else if(Input.GetKey(KeyCode.DownArrow) || yPos>0 && yPos < panDetect)
        {
            //move down
            moveZ -= panSpeed;
        }

        //User moves the camera up/down using the mouse scroll wheel
        //scroll up = move up, scroll down = move down
        //Multiplied by some value, related it to panSpeed times some number
        moveY += Input.GetAxis("Mouse ScrollWheel") * (panSpeed * 20);

        //Clamps moveY between minHeight & maxHeight (makes sure moveY is between those values)
        moveY = Mathf.Clamp(moveY, minHeight, maxHeight);
        Vector3 newPos = new Vector3(moveX,moveY,moveZ);
        Camera.main.transform.position = newPos;
    }

    void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //If middle mouse button is pressed down
        if(Input.GetMouseButton(2)||Input.GetKey(KeyCode.LeftControl) )
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
        }

        //If destination is not the origin of the camera (the camera destination has moved)
        if(destination != origin)
        {
            //rotate the camera from the origin, towards the destination, smoothing the movement using deltatime and using the rotatespeed to rotate
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }
    
}
