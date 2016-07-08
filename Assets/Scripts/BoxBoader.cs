using UnityEngine;
using System;

[RequireComponent(typeof(MeshRenderer))]
public class BoxBoader : MonoBehaviour {
    public BoxController ParentBox;
    public Color SelectedColor = Color.yellow;
    private Color DefaultColor;
    private MeshRenderer curMeshRender;

    private bool m_bSelected = false;
    [HideInInspector]
    public bool Selected 
    {
        get
        {
            return m_bSelected;
        }
        set
        {
            if (m_bSelected == value)
                return;
            if(curMeshRender && curMeshRender.material)
            {
                curMeshRender.material.color = value ? SelectedColor : DefaultColor;
            }
            m_bSelected = value;
        }
    }
    //private int m_iClickCount = 0; 
    //DateTime curTime;
    // Use this for initialization
    void Start () 
    {
        curMeshRender = GetComponent<MeshRenderer>();
        if (curMeshRender && curMeshRender.material)
        {
            DefaultColor = curMeshRender.material.color;
            SelectedColor.a = (DefaultColor.a + 1f) / 2;
        }
        //curTime = DateTime.Now;

    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //void OnMouseDown()
    //{
    //    DateTime now = DateTime.Now;
    //    double deltaTime = (now - curTime).TotalSeconds;
    //    if (m_iClickCount == 1 && deltaTime < 0.5)
    //    {
    //        m_iClickCount = 0;
    //        if (ParentBox)
    //            ParentBox.OnBoaderClicked(transform);
    //    }
    //    else
    //    {
    //        m_iClickCount = 1;
    //    }
    //    curTime = now;
    //}
}
