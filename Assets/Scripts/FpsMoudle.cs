using UnityEngine;
using System.Collections;

public class FpsMoudle : MonoBehaviour {
    public UILabel TextUI;
    public int Fps { get; private set; }
    private int m_iSum = 0;
    private int m_iCurIdx = 0;
    private int m_iBufferSize = 60;
    public int BufferSize 
    {
        get
        {
            return m_iBufferSize;
        }
        set
        {
            if (value == m_iBufferSize)
                return;
            m_iBufferSize = value;
            ResizeBuffer();
        }
    }
    private int[] Buffer = null;
    void ResizeBuffer()
    {
        int[] temp = new int[m_iBufferSize];
        if(Buffer != null)
        {
            for(int i = 0; i < Buffer.Length && i < m_iBufferSize; ++i)
            {
                temp[i] = Buffer[i];
            }
        }
        Buffer = temp;
        m_iCurIdx = m_iCurIdx >= m_iBufferSize ? 0 : m_iCurIdx;
    }
    void Awake()
    {
        ResizeBuffer();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_iSum -= Buffer[m_iCurIdx];
        Buffer[m_iCurIdx] = (int)(1 / Time.unscaledDeltaTime);
        m_iSum += Buffer[m_iCurIdx];
        Fps = (m_iSum) / m_iBufferSize;
        m_iCurIdx = (m_iCurIdx + 1) % m_iBufferSize;
        if (TextUI)
            TextUI.text = Mathf.Clamp(Fps, 0, 99).ToString();
	}
}
