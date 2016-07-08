using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class CubeTxt : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateTextDir();
	}

    private void UpdateTextDir()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.up));
    }
}
