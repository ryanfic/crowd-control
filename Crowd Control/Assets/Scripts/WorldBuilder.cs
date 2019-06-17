using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

// A class for building the NavMeshSurface and colliders durig run-time.

public class WorldBuilder : MonoBehaviour {

	private string navMeshPath = "Assets/NavMesh/WRLD_NavMesh.asset"; //The location of the stored Nav Mesh, if it is created
	public NavMeshSurface crowdSurface; //The nav mesh


	// Use this for initialization
	void Start () {
		Invoke("buildTheMesh",1.5f);
		Invoke("attachMeshColliders",1.5f);

	}

	void buildTheMesh(){
		//sets the nav mesh settings of WRLD object/children
		SetNavMeshSettings();
		//Loads the nav mesh data
		NavMeshData meshdata = (NavMeshData)AssetDatabase.LoadAssetAtPath(navMeshPath,typeof(NavMeshData));
		//if the nav mesh data doesn't exist
		if(meshdata==null)
		{
			
			Debug.Log("Currently updating NavMeshSurface.");
			//creates a new nav mesh surface
			StartCoroutine(BuildNavmesh(crowdSurface));
		}
		//if the nav mesh data exists
		else{
			//add the nav mesh data to the nav mesh
			crowdSurface.navMeshData = meshdata;
        	crowdSurface.AddData();
		}
	}

	//adds mesh colliders to game objects
	void attachMeshColliders(){
		GameObject go;
		Debug.Log("Currently adding Mesh Colliders.");
		foreach(MeshRenderer mesh in crowdSurface.GetComponentsInChildren<MeshRenderer>(true))
		{
			go=mesh.gameObject;
			go.AddComponent<MeshCollider>();
			/*go.GetComponent<MeshCollider>().convex=true; */
		} 
		Debug.Log("Finished adding Mesh Colliders.");
	} 

	// Sets the layer for gameObject and its children, children's children, etc.
	public static void SetLayerRecursively(string objectName, int layerNum) {
		GameObject go = GameObject.Find(objectName);
		foreach(Transform trans in go.GetComponentsInChildren<Transform>(true)) {
            trans.gameObject.layer = layerNum;
        }
	}
	//sets the nav mesh settings for the WRLD object and its children
	private void SetNavMeshSettings() {
		// Duplicate buildings to make "floor" inside building meshes
		Transform parent = GameObject.Find("Root").transform;
		GameObject buildings = GameObject.Find("Buildings");


		// Puts terrain, roads, and buildings into proper layers
		WorldBuilder.SetLayerRecursively("Terrain", 11);
		WorldBuilder.SetLayerRecursively("Roads", 8);
		WorldBuilder.SetLayerRecursively("Buildings", 9);

        // Discludes buildings from being walkable for NavMesh Agent
        buildings.AddComponent<NavMeshModifier>();
		buildings.GetComponent<NavMeshModifier>().overrideArea = true;
        buildings.GetComponent<NavMeshModifier>().area = NavMesh.GetAreaFromName("Not Walkable");
	}

	// called by startcoroutine whenever you want to build the navmesh
    IEnumerator BuildNavmesh(NavMeshSurface surface) {
		// wait until everything has rendered
		yield return new WaitForEndOfFrame();

        // get the data for the surface
        var data = InitializeBakeData(surface);
 
        // start building the navmesh
        var async = surface.UpdateNavMesh(data);
 
        // wait until the navmesh has finished baking
        yield return async;
 
        Debug.Log("Finished updating NavMeshSurface.");
 
        // you need to save the baked data back into the surface
        surface.navMeshData = data;

		// save the nav mesh data
		SaveNavMeshData(data);
 
        // call AddData() to finalize it
        surface.AddData();
    }
	//saves the nav mesh data
	void SaveNavMeshData(NavMeshData data){
		AssetDatabase.CreateAsset(data, navMeshPath);
	}
 
    // creates the navmesh data
    static NavMeshData InitializeBakeData(NavMeshSurface surface) {
    	var emptySources = new List<NavMeshBuildSource>();
        var emptyBounds = new Bounds();
 
        return UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), emptySources, emptyBounds, surface.transform.position, surface.transform.rotation);
    }
}
