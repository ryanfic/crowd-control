using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CrowdGenerator : MonoBehaviour
{
    public GameObject crowdtemplate;
    public float crowdPercentInstigator; //percent of the crowd that are instigators, 30.5 = 30.5%
    public float crowdPercentFollower; //percent of the crowd that are followers
    public IList<GameObject> crowdlist = new List<GameObject>();
    
    private float crowdPercentLawful;
    private int tospawn = 600;
    private int totalInstigators = 0;
    private int totalFollowers = 0;
    
    private Vector3 granvilleExit = new Vector3(-732.9075f,21.81492f,-448.9697f);
    private Vector3 waterfrontStation = new Vector3(175.7381f,16.23377f,584.8232f);
    private Vector3 granvilleStation = new Vector3(-129.7943f,32.36957f,350.8734f);
    private Vector3 stadiumChinatownStation = new Vector3(-129.8107f,32.37526f,350.7296f);
    public Material[] material;
    private string flowfile = "crowdflowdata.txt";
    private string compositionfile = "crowdcompdata.txt";
    


    //public TextWriter wr;
    void Start()
    {
        crowdPercentLawful = 100 - crowdPercentInstigator - crowdPercentFollower;
        /*for(int i=0;i<10;i++){
                 StartCoroutine(delaySpawnCrowdAroundSpot(new Vector3((float)(-66+i*5),29f,(float)(50-5*i)), 20,15,3));
            
        }*/
        //4:-113.2818 33.1 97.550
        //3: -82.7300 31.7 66.288
        //2: -46.649 30.3 30.999
        //1: -26.2 29.44 12.1
        float timetospawn = 3.1f;
        if(File.Exists(compositionfile))
        {
            Invoke("dataSpawnCrowd",timetospawn-1.5f);
        }
         else
        {
            randomSpawnCrowd();
        }
        Invoke("WriteCrowdRatios",timetospawn);
        Invoke("WriteCrowdLocation",timetospawn+4f);
        //Invoke("RemoveSelf",timetospawn+0.5f);
    }
    void randomSpawnCrowd()
    {
        //Randomly spawn the crowd
        for(int i=0;i<4;i++){
                 //StartCoroutine(delaySpawnCrowdAroundSpot(new Vector3((float)(-26.2-i*30),29f,(float)(12.1+30*i)), 140,25,3)); //spawn up the road
            StartCoroutine(delaySpawnCrowdAroundSpot(new Vector3((float)(-116.2+i*30),29f,(float)(102.1-30*i)), 150,25,3)); //spawn down the road
        }
        //Write the crowd data
        //Invoke("WriteCrowdRatios",timetospawn);
    }
    void dataSpawnCrowd()
    {
        StreamReader sr = new StreamReader(compositionfile);
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            //parse the file for data
            //split data by ; delimiter
            string[] fields = line.Split(';');
            //first field is crowd type
            CrowdType ctype;
            if(fields[0]=="Lawful")
            {
                ctype=CrowdType.Lawful;
            }
            else if(fields[0]=="Follower")
            {
                ctype=CrowdType.Follower;
            }
            else
            {
                ctype=CrowdType.Instigator;
            }
            
            //second field is location
            //get xyz
            string[] posString = fields[1].Split(',');
            float[] xyz = new float[3];
            for(int i=0;i<posString.Length;i++)
            {
                xyz[i] = float.Parse(posString[i]);
            }
            Vector3 pos = new Vector3(xyz[0],xyz[1],xyz[2]);

            //Spawn the agent!
            addCrowdAgent(pos,ctype);
        }
    }
    void addCrowdAgent(Vector3 pos)
    {
        Quaternion spawnRotation = Quaternion.identity;
        //pos.y = hitGroundAtPos(pos).y;
        GameObject ncrowd = Instantiate(crowdtemplate, pos, spawnRotation);
        float rand = Random.value*100;
        //If the roll is lower than the chance to get Instigator, make the crowd agent an instigator
        if(rand<crowdPercentInstigator&&(100f*totalInstigators)/tospawn<crowdPercentInstigator){
            /* ncrowd.transform.Find("AreaOfInfluence").GetComponent<SphereCollider>().radius=12;
            ncrowd.AddComponent<InstigatorController>();
            ncrowd.layer = LayerMask.NameToLayer("Instigator");
            SkinnedMeshRenderer[] mesh = ncrowd.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer m in mesh)
            {
                m.material.color = Color.red;
            }
            //ncrowd.GetComponent<MeshRenderer>().material = material[0];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(ncrowd.GetComponent<InstigatorController>().getInfluence());
            totalInstigators++;*/
            changeAgentToInstigator(ncrowd);
        }
        //If the roll is above the chance to get Instigator but lower than the chance to get Follower, make the crowd agent an follower
        else if(rand<crowdPercentInstigator+crowdPercentFollower&&(100f*totalFollowers)/tospawn<crowdPercentFollower){
            /*ncrowd.AddComponent<FollowerController>();
            ncrowd.layer = LayerMask.NameToLayer("Follower");
            //ncrowd.GetComponent<MeshRenderer>().material = material[1];
            FollowerController fc =  ncrowd.GetComponent<FollowerController>();
            SkinnedMeshRenderer[] mesh = ncrowd.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer m in mesh)
            {
                m.material.color = Color.yellow;
            }
            fc.material[0]= material[1];
            fc.material[1]= material[2];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(fc.getInfluence());
            totalFollowers++;*/
            changeAgentToFollower(ncrowd);
        }
        //Else make it lawful
        else{
            /* ncrowd.AddComponent<LawfulController>();
            ncrowd.layer = LayerMask.NameToLayer("Lawful");
            SkinnedMeshRenderer[] mesh = ncrowd.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer m in mesh)
            {
                m.material.color = Color.green;
            }
            //ncrowd.GetComponent<MeshRenderer>().material = material[3];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(ncrowd.GetComponent<LawfulController>().getInfluence());*/
            changeAgentToLawful(ncrowd);
        }
        crowdlist.Add(ncrowd);
        //crowdtot++;
        //CrowdController cScript = (CrowdController)ncrowd.GetComponent("CrowdController");
        //RlCrowdTot+=cScript.getValue();
        /*To print how many crowd objects exist */
        /* Debug.Log("New Crowd Total:" + RlCrowdTot);*/
        //CreateObject(baseCrowdAgent, hit.point, "CrowdAgent (" + crowdCount + ")", crowdAgents.transform).GetComponent<NavMeshAgent>();
    }
    void addCrowdAgent(Vector3 pos, CrowdType ctype)
    {
        Quaternion spawnRotation = Quaternion.identity;
        //pos.y = hitGroundAtPos(pos).y;
        GameObject ncrowd = Instantiate(crowdtemplate, pos, spawnRotation);

        //If the type is a Lawful, make the agent a Lawful
        if(ctype == CrowdType.Lawful){

            changeAgentToLawful(ncrowd);
        }
        //If the type is a Follower, Make the agent a Follower
        else if(ctype == CrowdType.Follower){
            changeAgentToFollower(ncrowd);
        }
        //Else make it an Instigator
        else{
            changeAgentToInstigator(ncrowd);
        }
        //Might be irrelevant to this situation
        crowdlist.Add(ncrowd);
    }
    void changeAgentToInstigator(GameObject ncrowd)
    {
        ncrowd.transform.Find("AreaOfInfluence").GetComponent<SphereCollider>().radius=12;
            ncrowd.AddComponent<InstigatorController>();
            ncrowd.layer = LayerMask.NameToLayer("Instigator");
            SkinnedMeshRenderer[] mesh = ncrowd.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer m in mesh)
            {
                m.material.color = Color.red;
            }
            //ncrowd.GetComponent<MeshRenderer>().material = material[0];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(ncrowd.GetComponent<InstigatorController>().getInfluence());
            totalInstigators++;
    }
    void changeAgentToFollower(GameObject ncrowd)
    {
        ncrowd.AddComponent<FollowerController>();
            ncrowd.layer = LayerMask.NameToLayer("Follower");
            //ncrowd.GetComponent<MeshRenderer>().material = material[1];
            FollowerController fc =  ncrowd.GetComponent<FollowerController>();
            SkinnedMeshRenderer[] mesh = ncrowd.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer m in mesh)
            {
                m.material.color = Color.yellow;
            }
            fc.material[0]= material[1];
            fc.material[1]= material[2];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(fc.getInfluence());
            totalFollowers++;
    }
    void changeAgentToLawful(GameObject ncrowd)
    {
        ncrowd.AddComponent<LawfulController>();
            ncrowd.layer = LayerMask.NameToLayer("Lawful");
            SkinnedMeshRenderer[] mesh = ncrowd.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer m in mesh)
            {
                m.material.color = Color.green;
            }
            //ncrowd.GetComponent<MeshRenderer>().material = material[3];
            ncrowd.GetComponentInChildren<AreaOfInfluenceController>().setInfluence(ncrowd.GetComponent<LawfulController>().getInfluence());
    }

    void spawnCrowdAroundSpot(Vector3 pos, int num, float radius)
    {
        for(int i = 0; i<num;i++)
        {
            Vector3 spot = (Vector3)Random.insideUnitCircle;
            spot.z=spot.y;
            spot.y=0;
            addCrowdAgent(pos+spot*radius);
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
            addCrowdAgent(pos+spot*radius);
        }
    }
    void WriteCrowdRatios()
    {
        Debug.Log("I " + (totalInstigators*100f)/tospawn + " F " + (totalFollowers*100f)/tospawn);
        StreamWriter sw = new StreamWriter(flowfile);
        string toadd = "Instigators "+(totalInstigators*100f)/tospawn+" Followers " + (totalFollowers*100f)/tospawn + " Lawful " + (tospawn-(totalInstigators+totalFollowers))*100f/tospawn;
        sw.WriteLine(toadd);
        sw.Close();
    }
    //write the locations of all the crowd to a file
    void WriteCrowdLocation()
    {
        
        StreamWriter sw = new StreamWriter(compositionfile);
        string towrite;
        foreach(GameObject crowd in crowdlist)
        {
            //clear towrite
            towrite = "";
            //add type
            CrowdController cc = crowd.GetComponent<CrowdController>();
            //variables delimited by ;
            towrite +=  cc.getCrowdType()+";";
            //add location
            //x y z separation delimited by ,
            towrite += crowd.transform.position.x +","+crowd.transform.position.y +","+crowd.transform.position.z;

            sw.WriteLine(towrite);
        }
        sw.Close();
    }

    void RemoveSelf()
    {
        Destroy(this);
    }
}
