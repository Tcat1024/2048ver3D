using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CubePool : MonoBehaviour {
    static CubePool sInstance;
    public static CubePool GetInstance()
    {
        if (sInstance)
            return sInstance;
        else
        {
            sInstance = GameObject.FindObjectOfType<CubePool>();
            Debug.Assert(sInstance);
            return sInstance;
        }
    }

    public CubeMain CubTemplate;
    private LinkedList<CubeMain> CubeList = new LinkedList<CubeMain>();

    private static int CubeCount = 0;
    public CubeMain GetCube()
    {
        CubeMain result = null;
        if(CubeList.Count > 0)
        {
            result = CubeList.First.Value;
            CubeList.RemoveFirst();
        }
        else
        {
            result = Instantiate<CubeMain>(CubTemplate);
            result.name = "Cube" + CubeCount;
            ++CubeCount;
        }
        result.gameObject.SetActive(true);
        return result;
    }

    public void DestroyCube(CubeMain cube)
    {
        CubeList.AddLast(cube);
        cube.transform.SetParent(gameObject.transform);
        cube.gameObject.SetActive(false); 
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
    void Update()
    {

    }
}
