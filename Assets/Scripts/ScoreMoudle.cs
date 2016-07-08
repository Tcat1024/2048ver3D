using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class ScoreMoudle : MonoBehaviour {
    public UILabel ScoreNumTextUI;
    public UILabel ScoreTipTextUI;
    private int m_iScore = 0;
    public int Score
    {
        get
        {
            return m_iScore;
        }
        set
        {
            if (value == m_iScore)
                return;
            m_iScore = value;
            UpdateUI();
        }
    }
    void UpdateUI()
    {
        int iMinWidth = 0;
        if (ScoreNumTextUI)
        {
            ScoreNumTextUI.text = m_iScore.ToString();
            iMinWidth = Mathf.Max(ScoreNumTextUI.width + (int)(2 * Mathf.Abs(ScoreNumTextUI.transform.localPosition.x)), iMinWidth);
        }
        if(ScoreTipTextUI)
        {
            iMinWidth = Mathf.Max(ScoreTipTextUI.width + (int)(2 * Mathf.Abs(ScoreTipTextUI.transform.localPosition.x)), iMinWidth);
        }
        GetComponent<UISprite>().width = iMinWidth;
    }
	// Use this for initialization
	void Start ()
    {
    }

    bool m_bInited = false;
	// Update is called once per frame
	void Update () {
        if(!m_bInited)
        {
            UpdateUI();
            m_bInited = true;
        }
    }
}
