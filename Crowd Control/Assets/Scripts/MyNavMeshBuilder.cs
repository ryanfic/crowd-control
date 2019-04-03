using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyNavMeshBuilder : MonoBehaviour
{
    NavMeshSurface surface;
    // Start is called before the first frame update
    void Start()
    {
        GameObject terrain = GameObject.Find("Terrain");
        GameObject buildings = GameObject.Find("Buildings");
        buildings.AddComponent<NavMeshModifier>();
        terrain.AddComponent<NavMeshSurface>();
        surface=terrain.GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
        
    }

}
