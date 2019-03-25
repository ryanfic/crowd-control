using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdGenerator : MonoBehaviour
{
    public GameObject crowdtemplate;
    public float crowdPercentInstigator; //percent of the crowd that are instigators, 30.5 = 30.5%
    public float crowdPercentFollower; //percent of the crowd that are followers
    
    private Vector3 granvilleExit = new Vector3(-732.9075f,21.81492f,-448.9697f);
    private Vector3 waterfrontStation = new Vector3(175.7381f,16.23377f,584.8232f);
    private Vector3 granvilleStation = new Vector3(-129.7943f,32.36957f,350.8734f);
    private Vector3 stadiumChinatownStation = new Vector3(-129.8107f,32.37526f,350.7296f);
    public Material[] material;
    void Start()
    {
        for(int i=0;i<10;i++){
                 StartCoroutine(delaySpawnCrowdAroundSpot(new Vector3((float)(-66+i*5),29f,(float)(50-5*i)), 75,15,3));
            
        }
        //addCrowd(new Vector3(-66,29f,50));
    }

    void addCrowd(Vector3 pos)
    {
        Quaternion spawnRotation = Quaternion.identity;
        //pos.y = hitGroundAtPos(pos).y;
        GameObject ncrowd = Instantiate(crowdtemplate, pos, spawnRotation);
        float rand = Random.value*100;
        //If the roll is lower than the chance to get Instigator, make the crowd agent an instigator
        if(rand<crowdPercentInstigator){
            ncrowd.transform.Find("AreaOfInfluence").GetComponent<SphereCollider>().radius=20;
            ncrowd.AddComponent<InstigatorController>();
            ncrowd.layer = LayerMask.NameToLayer("Instigator");
            ncrowd.GetComponent<MeshRenderer>().material = material[0];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(ncrowd.GetComponent<InstigatorController>().getInfluence());
        }
        //If the roll is above the chance to get Instigator but lower than the chance to get Follower, make the crowd agent an follower
        else if(rand<crowdPercentInstigator+crowdPercentFollower){
            ncrowd.AddComponent<FollowerController>();
            ncrowd.layer = LayerMask.NameToLayer("Follower");
            ncrowd.GetComponent<MeshRenderer>().material = material[1];
            FollowerController fc =  ncrowd.GetComponent<FollowerController>();
            fc.material[0]= material[1];
            fc.material[1]= material[2];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(fc.getInfluence());
        }
        //Else make it lawful
        else{
            ncrowd.AddComponent<LawfulController>();
            ncrowd.layer = LayerMask.NameToLayer("Lawful");
            ncrowd.GetComponent<MeshRenderer>().material = material[3];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(ncrowd.GetComponent<LawfulController>().getInfluence());
        }
        //crowdlist.Add(ncrowd);
        //crowdtot++;
        //CrowdController cScript = (CrowdController)ncrowd.GetComponent("CrowdController");
        //RlCrowdTot+=cScript.getValue();
        /*To print how many crowd objects exist */
        /* Debug.Log("New Crowd Total:" + RlCrowdTot);*/
        //CreateObject(baseCrowdAgent, hit.point, "CrowdAgent (" + crowdCount + ")", crowdAgents.transform).GetComponent<NavMeshAgent>();
    }

    void spawnCrowdAroundSpot(Vector3 pos, int num, float radius)
    {
        for(int i = 0; i<num;i++)
        {
            Vector3 spot = (Vector3)Random.insideUnitCircle;
            spot.z=spot.y;
            spot.y=0;
            addCrowd(pos+spot*radius);
        }
    }
    IEnumerator delaySpawnCrowdAroundSpot(Vector3 pos, int num, float radius,int time)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i<num;i++)
        {
            Vector3 spot = (Vector3)Random.insideUnitCircle;
            spot.z=spot.y;
            spot.y=0;
            addCrowd(pos+spot*radius);
        }
    }
}
