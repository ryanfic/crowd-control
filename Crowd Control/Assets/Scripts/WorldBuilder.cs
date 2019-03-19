using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

// A class for building the NavMeshSurface and colliders durig run-time.

public class WorldBuilder : MonoBehaviour {

	public NavMeshSurface humanSurface;
	public NavMeshSurface vehicleSurface;

	//private NavMeshData savedNavMesh;
	
	private bool firstBuild;

	// Use this for initialization
	void Start () {
		firstBuild = true;
		Invoke("buildTheMesh",1.5f);
		Invoke("attachMeshColliders",1.5f);
	}
	// Update is called once per frame
	/*void Update () {

		if(Input.GetKeyDown("b")) { // Press "b" to rebuild NavMesh

			if(firstBuild) {
				SetNavMeshSettings();
			}

			StartCoroutine(BuildNavmesh(humanSurface));
			//StartCoroutine(BuildNavmesh(vehicleSurface));
			Debug.Log("Currently updating NavMeshSurface.");
		}
		
	} */
	void buildTheMesh(){
		if(firstBuild) {
			SetNavMeshSettings();
		}

		StartCoroutine(BuildNavmesh(humanSurface));
			//StartCoroutine(BuildNavmesh(vehicleSurface));
		Debug.Log("Currently updating NavMeshSurface.");
	}
	void attachMeshColliders(){
		GameObject go;
		Debug.Log("Currently adding Mesh Colliders.");
		foreach(MeshRenderer mesh in humanSurface.GetComponentsInChildren<MeshRenderer>(true))
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

	private void SetNavMeshSettings() {
		// Duplicate buildings to make "floor" inside building meshes
		Transform parent = GameObject.Find("Root").transform;
		GameObject buildings = GameObject.Find("Buildings");
		//GameObject floor = Object.Instantiate(buildings, parent);
		//floor.name = "Floor";
		//floor.transform.localScale = new Vector3(0.99f, 0.265f, 0.99f);

		// Sets WrldMap and all children to navigation static
		foreach(Transform trans in gameObject.GetComponentsInChildren<Transform>(true)) {
        	//GameObjectUtility.SetStaticEditorFlags(trans.gameObject, StaticEditorFlags.NavigationStatic);
        }

		// Puts terrain, roads, and buildings into proper layers
		WorldBuilder.SetLayerRecursively("Terrain", 11);
		WorldBuilder.SetLayerRecursively("Roads", 8);
		WorldBuilder.SetLayerRecursively("Buildings", 9);

        // Discludes buildings from being walkable for NavMesh Agent
        buildings.AddComponent<NavMeshModifier>();
		buildings.GetComponent<NavMeshModifier>().overrideArea = true;
        buildings.GetComponent<NavMeshModifier>().area = NavMesh.GetAreaFromName("Not Walkable");

		// Discludes "floor" from being walkable for NavMesh Agent
        //floor.AddComponent<NavMeshModifier>();
        //floor.GetComponent<NavMeshModifier>().overrideArea = true;
        //floor.GetComponent<NavMeshModifier>().area = NavMesh.GetAreaFromName("Not Walkable");

		/*Debug.Log("NavMeshSurface built for: " + NavMesh.GetSettingsNameFromID(0));

		// Sets settings for police and crowd NavMeshAgents
		NavMeshBuildSettings settings = NavMesh.GetSettingsByID(0);
		settings.agentClimb = 0.4f;
		settings.agentHeight = 2;
		settings.agentRadius = 0.5f;
		settings.agentSlope = 45;
		//settings.agentTypeID = 0; // Needs to match NavMeshAgentID
		settings.minRegionArea = 1000; // Find a good value
		//settings.overrideTileSize
		//settings.overrideVoxelSize
		//settings.tileSize
		//settings.voxelSize*/

		firstBuild = false;

		//surface.BuildNavMesh();
		//Debug.Log("Built NavMeshSurface.");
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
 
        // call AddData() to finalize it
        surface.AddData();
    }
 
    // creates the navmesh data
    static NavMeshData InitializeBakeData(NavMeshSurface surface) {
    	var emptySources = new List<NavMeshBuildSource>();
        var emptyBounds = new Bounds();
 
        return UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), emptySources, emptyBounds, surface.transform.position, surface.transform.rotation);
    }
}
