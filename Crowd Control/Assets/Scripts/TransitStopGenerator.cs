using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitStopGenerator : MonoBehaviour
{
    public GameObject TransitTemplate;
    private Vector3 VancouverCityCenterStationLocation = new Vector3(-319.1273f,34.57629f,257.2944f);

    public List<GameObject> GenerateStops()
    {
        List<GameObject> stoplist = new List<GameObject>();
        GameObject go = Instantiate(TransitTemplate,VancouverCityCenterStationLocation,Quaternion.identity);
        stoplist.Add(go);
        return stoplist;
    }
}
