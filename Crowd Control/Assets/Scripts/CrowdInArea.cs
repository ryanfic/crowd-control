using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowdInArea : MonoBehaviour
{
    public Text countText;
    private int count;

    private string file = "crowddata.txt";

    public TextWriter wr;
    
    void Start()
    {
        countText = GameObject.Find("Count Text").GetComponent<Text>();
        count =0;
        SetCountText();
        InvokeRepeating("WriteCountToFile",0.0f,1.0f);
    }


    void OnTriggerEnter(Collider other){
        //int toadd = other.gameObject.GetComponent<CrowdController>()?.getValue() ?? 0;
        count+=other.gameObject.GetComponent<CrowdController>()?.getValue()??0;
        SetCountText();
    }
    void OnTriggerExit(Collider other){
        count-=other.gameObject.GetComponent<CrowdController>()?.getValue()??0;
        SetCountText();
    }
    void SetCountText(){
        countText.text = "Crowd: " + count.ToString();
    }

    void WriteCountToFile()
    {
        wr.AppendWrite(file,count);
    }

}
