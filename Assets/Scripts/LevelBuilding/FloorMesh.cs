using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMesh : MonoBehaviour {

    public const float ITEM_WIDTH = 2.0f;

    public float width, length;
    public float prevWidth;
    public int index;
    
    public Dictionary<int, ItemSuper> m_ItemsOnMesh;

    public GameObject leftRim;
    public GameObject rightRim;
    public GameObject carDetectTrigger;

    public List<GameObject> destroyOnRemake;
     
    public Vector3 prevPos1, prevPos2; //1 left, 2 right
    public Vector3 dir, prevDir;
    public Vector3 endPos1, endPos2;

    public FloorTypeData floorTypeData;

    float lifeTime;

    bool animating;

    Vector3[] vertices;
    Vector2[] uvs;
    Vector3[] normals;
    int[] triangles;

    Mesh mesh;

    private void Awake()
    {
        destroyOnRemake = new List<GameObject>();
    }

    public void ResetMesh()
    {
        lifeTime = 0;
        //GetComponent<MeshRenderer>().enabled = false;

        if (destroyOnRemake.Count > 0)
        {
            foreach(var go in destroyOnRemake)
            {
                Destroy(go);
            }
            destroyOnRemake.Clear();
        }

        foreach (var item in m_ItemsOnMesh)
        {
            if (item.Value != null)
            {
                item.Value.Disable();
            }
        }
        m_ItemsOnMesh.Clear();

        if (GetComponent<MeshCollider>() != null)
        {
            DestroyImmediate(GetComponent<MeshCollider>());
        }
        GetComponent<FloorColor>().DrawColor();
    }

    public void makeMeshNoColor()
    {
        //GetComponent<MeshRenderer>().enabled = true;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        prevPos1.y -= 0.00f;
        prevPos2.y -= 0.00f;

        Vector3 prevPosMid = (prevPos1 + prevPos2) / 2.0f;
        prevPosMid += dir * length;
        endPos1 = prevPosMid + (width / 2.0f * Vector3.Cross(dir, Vector3.up));
        endPos2 = prevPosMid - (width / 2.0f * Vector3.Cross(dir, Vector3.up));
        prevWidth = Vector3.Distance(prevPos1, prevPos2);
        
        mesh = new Mesh();

        int childCount = 1;
        vertices = new Vector3[childCount * 4];
        uvs = new Vector2[childCount * 4];
        normals = new Vector3[childCount * 4];
        triangles = new int[childCount * 2 * 3];

        vertices[0] = prevPos1;
        vertices[1] = prevPos2;
        vertices[2] = endPos1;
        vertices[3] = endPos2;

        uvs[0] = new Vector2(0.0f, 0.0f);
        uvs[1] = new Vector2(1.0f, 0.0f);
        uvs[2] = new Vector2(0.0f, 1.0f);
        uvs[3] = new Vector2(1.0f, 1.0f);

        // float dot = Vector3.Dot(prevDir, dir);
        //float angle = Vector3.Angle(prevDir, dir);
        //if (dot < 0) angle = -angle;
        //print(angle);
        for (int a = 0; a != 4; ++a)
        {
            //Quaternion randomOffset = Quaternion.Euler( (Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1) * Random.Range(0.0f, 5.0f), (Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1) * Random.Range(0.0f, 5.0f), (Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1) * Random.Range(0.0f, 5.0f));
            //normals[a] = randomOffset * Vector3.Cross(dir, (prevPos2 - prevPos1).normalized);  //Vector3.up;
            normals[a] = Vector3.Cross(dir, new Vector3(dir.z, 0, -dir.x));//Quaternion.Euler(dir.x, dir.y, 0) * Vector3.up;
        }

        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;


        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = triangles;

        mesh.name = "Generated mesh";
        GetComponent<MeshFilter>().mesh = mesh;

        leftRim.transform.position = (endPos1 + prevPos1) / 2.0f;
        leftRim.transform.localScale = new Vector3(0.3f, 0.3f, (endPos1 - prevPos1).magnitude);
        leftRim.transform.forward = (endPos1 - prevPos1).normalized;
        rightRim.transform.position = (endPos2 + prevPos2) / 2.0f;
        rightRim.transform.localScale = new Vector3(0.3f, 0.3f, (endPos2 - prevPos2).magnitude);
        rightRim.transform.forward = (endPos2 - prevPos2).normalized;
        carDetectTrigger.transform.position = (prevPos1 + prevPos2) / 2.0f + Vector3.up * 2.0f;
        carDetectTrigger.transform.localScale = new Vector3(Vector3.Distance(prevPos1, prevPos2), 4.0f, 0.3f);
        carDetectTrigger.transform.forward = Vector3.Cross((prevPos1 - prevPos2), Vector3.up);//  dir.normalized;

        animateToDest();
    }

    public void makeMesh()
    {
        makeMeshNoColor();

        //changeTexture("floorDark");
        //dir.y += 0.01f;
        // dir.Normalize();
        //GetComponent<FloorColor>().DrawColor();
    }

    public void changeTexture(string textureName)
    {
        if (GetComponent<MeshRenderer>().material.mainTexture.name == textureName)
        {
            return;
        }
        Texture2D tex = Resources.Load("Textures/" + textureName) as Texture2D;
        Debug.Assert(tex != null, "null tex");
        GetComponent<MeshRenderer>().material.mainTexture = tex;
    }

    public void changeNormalByDir(Vector3 dir)
    {
        for (int a = 0; a != 4; ++a)
        {
            normals[a] = dir;
        }
        GetComponent<MeshFilter>().mesh.normals = normals;
    }

    public int GetMaxSlot()
    {
        return ((int) prevWidth / (int)ITEM_WIDTH) - 1;
    }

    public void PutItemOnSlot(ItemSuper item, int slot)
    {
        int maxSlot = GetMaxSlot();
        if (slot > maxSlot) slot = maxSlot;

        float offset = slot * ITEM_WIDTH + ITEM_WIDTH / 2.0f;
        Vector3 prevPosMid = prevPos1 + (prevPos2 - prevPos1).normalized * offset;
        prevPosMid += dir * length / 2.0f;
        prevPosMid.y += 1.2f;

        item.transform.position = prevPosMid;
        item.transform.forward = prevDir;

        if (m_ItemsOnMesh.ContainsKey(slot))
        {
            if(m_ItemsOnMesh[slot] != null)
            {
                m_ItemsOnMesh[slot].Disable();
                print("Substitute item");
            }
            m_ItemsOnMesh.Remove(slot);
        }
        m_ItemsOnMesh.Add(slot, item);
        item.StartAnim();
    }

    void animateToDest()
    {
        transform.position = new Vector3(0, -100.0f, 0);
        GetComponent<MoveToDecreasingSpeed>().from = transform.position;
        GetComponent<MoveToDecreasingSpeed>().to = Vector3.zero;
        GetComponent<MoveToDecreasingSpeed>().resetAnim();
        animating = true;
    }

	// Use this for initialization
	void OnEnable () {
        m_ItemsOnMesh = new Dictionary<int, ItemSuper>();
    }
	
    void addCollider()
    {
        if (gameObject.GetComponent<MeshCollider>() == null)
            gameObject.AddComponent<MeshCollider>();
        else
        {
            DestroyImmediate(GetComponent<MeshCollider>());
            gameObject.AddComponent<MeshCollider>();
        }
    }

	// Update is called once per frame
	public void InGameUpdate () {
        //GameManager.current.cIndex = index;
        //if((GameManager.GetGameState() == GameManager.GameState.Start || ))
		if(animating)
        {
            //GetComponent<MoveToDecreasingSpeed>().moveToword();
            if (GetComponent<MoveToDecreasingSpeed>().reached == true)
            {
                if (GameManager.current.state == GameManager.GameState.AssembleTrack)
                    GameManager.current.state = GameManager.GameState.Start;
                animating = false;
                lifeTime = 0;
                addCollider();
            }
        }
        if(lifeTime > FloorBuilder.current.floorMeshLifeTime)
        {
            ResetMesh();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Vector3 rot = Player.current.transform.eulerAngles;
            //rot.x = -Mathf.Asin(dir.y) * Mathf.Rad2Deg + 14;
            //Player.current.transform.eulerAngles = rot;//.up = Vector3.Cross(endPos1 - prevPos1, prevPos2 - prevPos1).normalized;

            OnPlayerEnter();
        }
        
        //transform.parent.GetComponent<FloorBuilder>().collidedTime = 0;
    }

    public void OnPlayerEnter()
    {
        transform.parent.GetComponent<FloorBuilder>().meshCollided(index);
        GameManager.current.cIndex = index;

        // Could be useful, not useful now
        //Player.current.EnteredFloor(this);
    }

    /*void OnCollisionExit(Collision collision)
    {
        if (transform.parent.GetComponent<FloorBuilder>().collidingIndex == index)
        {
            transform.parent.GetComponent<FloorBuilder>().collidingIndex = -1;
        } 
    }*/

}
