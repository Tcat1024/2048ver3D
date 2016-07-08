using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class CubeMain : MonoBehaviour
{
    public int ColorOffsetNum = 3 ;
	private float m_iMaxColor = 1f;
	public float MaxColor
	{
		get {
			return m_iMaxColor;
		}
		set {
			m_iMaxColor = Mathf.Clamp (value, 0f, 1f);
		}
	}
	private float m_iMinColor = 0.4f;
	public float MinColor
	{
		get{
			return m_iMinColor;
		}
		set{
			m_iMinColor = Mathf.Clamp (value, 0f, 1f);
		}
	}
    public Color StartColor = new Color(1f, 1f, 0.4f);

    private CubeTxt TextObj = null;
    private int m_iNumber = 0;
    [HideInInspector]
    public int Number
    {
        get
        {
            return m_iNumber;
        }
        set
        {
            m_iNumber = value;
            GetComponent<MeshRenderer>().material.color = GetNumberColor(m_iNumber);
            if (TextObj == null)
                TextObj = transform.GetComponentInChildren<CubeTxt>();
            string strNum = string.Format("{0:d}", m_iNumber);
            TextObj.gameObject.GetComponent<TextMesh>().text = strNum;
            float scale = Mathf.Min(2.5f, 4.5f / strNum.Length);
            TextObj.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public delegate void MoveCallBackType();
    private MoveCallBackType m_funcCallBack = null;
    private Vector3 m_vTarPos;
    private Vector3 m_vStartPos;
    private float m_fAniTime = 0f;
    private float m_fDuration = 0f;
    [HideInInspector]
    public bool IsMoving { private set; get; }
    public void Move(Vector3 tarpos, float fDuration, MoveCallBackType callback = null)
    {
        IsMoving = true;
        m_vTarPos = tarpos;
        m_fAniTime = fDuration;
        m_fDuration = 0f;
        m_vStartPos = transform.localPosition;
        m_funcCallBack = callback;
        UpdateMoveAni(0);
    }


    public void Destroy()
    {
        CubePool.GetInstance().DestroyCube(this);
    }
	// Use this for initialization
	void Start () {
        
	
	}


	// Update is called once per frame
	void Update () {
        if(IsMoving)
        {
            UpdateMoveAni(m_fDuration += Time.deltaTime);
        }
	}

    void UpdateMoveAni(float fDuration)
    {
        float fPer = fDuration / m_fAniTime;
        if(fPer >= 1f)
        {
            transform.localPosition = m_vTarPos;
            IsMoving = false;
            m_fAniTime = 0;
            m_fDuration = 0;
            if (m_funcCallBack != null)
                m_funcCallBack();
        }
        else
            transform.localPosition = m_vStartPos + (m_vTarPos - m_vStartPos) * fPer;
    }


    static Dictionary<int, Color> ColorMap = new Dictionary<int, Color>();
    Color GetNumberColor(int number)
    {
        Color Result;
        if(!ColorMap.TryGetValue(number, out Result))
        {
            int count = 0;
            int tarN = number;
            while ((tarN = tarN >> 1) > 0)
            {
                ++count;
            }
            float[] rgb = new float[3];
            int circle = ((count - 1) / ColorOffsetNum) % 6;
            int index = (count - 1) % ColorOffsetNum;
            if (circle % 2 == 0)
            {
                rgb[(1 + circle) % 3] = MaxColor - index * (MaxColor - MinColor) / ColorOffsetNum;
                rgb[circle % 3] = MaxColor;
                rgb[(circle + 2) % 3] = MinColor;
            }
            else
            {
                rgb[(1 + circle) % 3] = MinColor + index * (MaxColor - MinColor) / ColorOffsetNum;
                rgb[circle % 3] = MinColor;
                rgb[(circle + 2) % 3] = MaxColor;
            }
            Result.r = rgb[0];
            Result.g = rgb[1];
            Result.b = rgb[2];
            Result.a = StartColor.a;
            ColorMap.Add(number, Result);
        }

        return Result;
    }

    static CubeMain New()
    {
        return CubePool.GetInstance().GetCube();
    }
}
