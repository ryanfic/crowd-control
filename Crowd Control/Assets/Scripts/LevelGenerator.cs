using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public NavMeshSurface surface;
    private int width = 200;
    private int height = 200;

    public GameObject wall;
    public GameObject policeline;
    public GameObject BusStop;

    private bool plSpawned = false;
    private bool bsSpawned = false;



    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();

        surface.BuildNavMesh();
    }

    void GenerateLevel()
    {
        for(int x = 10; x<=width; x+=20)
        {
            for(int y = 10; y <=height; y+=20)
            {
                if(Random.value > 0.7f)
                {
                    Vector3 pos = new Vector3(x-width/2f, 1.25f,y - height/2f);
                    Instantiate(wall, pos, Quaternion.identity,transform);
                }
                else if (!plSpawned)
                {
                    Vector3 pos = new Vector3(x-width/2f, 1.25f, y-height/2f);
                    Instantiate(policeline, pos, Quaternion.identity);
                    plSpawned = true;
                    
                }
                else if(!bsSpawned && x>170)
                {
                    Vector3 pos = new Vector3(x-width/2f, 0f, y-height/2f);
                    Instantiate(BusStop, pos, Quaternion.identity);
                    bsSpawned = true;
                }
            }
        }
    }
}
