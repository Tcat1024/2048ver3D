using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public float CameraDist = 10f;
    public float CameraMinDist = 4f;
    public float CameraMaxDist = 10f;
    public float CameraDegSpeed;
    public float CameraDistSpeed;
    public float Smooth = 2f;
    private int m_iScreenScale = 1;
    private Quaternion m_qStanderRoate;
    private bool m_bShouldUpdatePos = false;
	// Use this for initialization
	void Start () {
        m_iScreenScale = Mathf.Min(Screen.width, Screen.height);
        m_qStanderRoate = transform.rotation;
        if (CameraDegSpeed == 0f)
        {
            CameraDegSpeed = 720f;
        }
        if (CameraDistSpeed == 0f)
        {
            CameraDistSpeed = 3f;
        }
        UpdateCameraPos();
	}
	
	// Update is called once per frame
	void Update () {
        m_bShouldUpdatePos = false;
        UpdateViewRoate();
        
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if(wheel == 0f)
        {
            if(Input.touchCount == 2)
            {
                Touch curTouch1 = Input.GetTouch(0);
                Touch curTouch2 = Input.GetTouch(1);
                if(curTouch1.phase == TouchPhase.Moved || curTouch2.phase == TouchPhase.Moved)
                {
                    float deltaTouch = Vector2.Distance(curTouch1.position, curTouch2.position) - Vector2.Distance(curTouch1.position - curTouch1.deltaPosition, curTouch2.position - curTouch2.deltaPosition);
                    UpdateCameraDist(deltaTouch);
                    m_bShouldUpdatePos = true;
                }
            }
        }
        else
        {
            UpdateCameraDist(wheel * m_iScreenScale);
            m_bShouldUpdatePos = true;
        }
        if(m_bShouldUpdatePos)
        {
            UpdateCameraPos();
        }
	}

    private void UpdateCameraDist(float deltaD)
    {
        CameraDist = Mathf.Clamp(CameraDist - deltaD * CameraDistSpeed * (CameraMaxDist - CameraMinDist) / m_iScreenScale, CameraMinDist, CameraMaxDist);
    }
    private void UpdateViewRoate()
    {
        float deltaX = 0f;
        float deltaY = 0f;
        bool UpdateRoate = false;
        if (Input.touchCount == 1)
        {
            Touch curTouch = Input.GetTouch(0);
            if (curTouch.phase == TouchPhase.Moved)
            {
                deltaX = curTouch.deltaPosition.x;
                deltaY = curTouch.deltaPosition.y;
                UpdateRoate = true;
            }
        }
        else if (Input.GetMouseButton(1) && Input.touchCount == 0)
        {
            deltaX = Input.GetAxis("Mouse X");
            deltaY = Input.GetAxis("Mouse Y");
            UpdateRoate = true;
        }
        if(UpdateRoate)
        {
            float yaw = deltaX * CameraDegSpeed / m_iScreenScale;
            float pitch = deltaY * CameraDegSpeed / m_iScreenScale;
            Vector3 angles = m_qStanderRoate.eulerAngles;
            angles.y += yaw;
            angles.x -= pitch;
            if (angles.x > 90 && angles.x < 270)
            {
                angles.x = pitch > 0 ? 270 : 90;
            }
            m_qStanderRoate.eulerAngles = angles;
        }
        if(m_qStanderRoate != transform.rotation)
        {
            m_bShouldUpdatePos = true;
            if (Smooth > 0f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, m_qStanderRoate, Time.deltaTime * Smooth);
            }
            else
            {
                transform.rotation = m_qStanderRoate;
            }
        }
    }

    private void UpdateCameraPos()
    {
        Camera.main.transform.position = Camera.main.transform.TransformDirection(Vector3.back) * CameraDist;
    }
}
