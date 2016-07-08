using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{
    const float MOVE_ANI_PER_BLOCK_DURATION = 0.05f;
 
    public struct BoxSizeType
    {
        public int X;
        public int Y;
        public int Z;
        public Vector3 Scale;
        public int Size
        {
            get
            {
                return X * Y * Z;
            }
        }
        public BoxSizeType(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            Scale.x = 1f / x;
            Scale.y = 1f / y;
            Scale.z = 1f / z;
        }
    }
    public BoxSizeType BoxSize = new BoxSizeType(4, 4, 4);
    public Transform CubesObjTran;
    public int StartCount = 4;
    public int NewCount = 2;
    public ScoreMoudle ScoreBoard;
    private bool m_bIsMoving = false;
    [HideInInspector]
    public bool IsMoving { private set{m_bIsMoving = value;} get{return m_bIsMoving;} }

    public void Restart()
    {
        foreach(var cube in Cubes)
        {
            if (cube)
                cube.Destroy();
        }
        Awake();
        Start();
    }

    private CubeMain[] Cubes;
    private int[] Numbers;
    private BoxBoader curBoader = null;

    int GetIndex(int x, int y, int z) { return BoxSize.X * BoxSize.Y * z + BoxSize.X * y + x; }

    Vector3 GetPos(int x, int y, int z)
    {
        return new Vector3(BoxSize.Scale.x * (x - BoxSize.X / 2 + 0.5f), BoxSize.Scale.y * (y - BoxSize.Y / 2 + 0.5f), BoxSize.Scale.z * (z - BoxSize.Z / 2 + 0.5f));
    }

    void NewCube(int number, int x, int y, int z)
    {
        int Idx = GetIndex(x, y, z);
        if (Idx < 0 || Idx >= BoxSize.Size || Cubes == null || Cubes[Idx] != null)
            return;
        Numbers[Idx] = number;
        var newCube = CubePool.GetInstance().GetCube();
        newCube.Number = number;
        newCube.transform.SetParent(CubesObjTran);
        newCube.transform.localScale = BoxSize.Scale;
        newCube.transform.localPosition = GetPos(x, y, z);
        Cubes[Idx] = newCube;
    }

    void NewRandomCube(int count)
    {
        int curCount = CubesObjTran.childCount;
        int down = BoxSize.Size - curCount;
        int up = Mathf.Min(count, down);
        for (int i = 0; i < BoxSize.X; ++i)
        {
            for (int j = 0; j < BoxSize.Y; ++j)
            {
                for(int k = 0; k < BoxSize.Z; ++ k)
                {
                    int curIdx = GetIndex(i, j, k);
                    if (Cubes[curIdx] != null)
                        continue;
                    if (Random.Range(0, down) < up)
                    {
                        NewCube(Random.Range(1, 3) * 2, i, j, k);
                        if (--up == 0)
                            return;
                    }
                    --down;
                }
            }

        }
    }

    void AddScore(int score)
    {
        if (ScoreBoard)
        {
            ScoreBoard.Score += score;
        }
    }

    void SetScore(int score)
    {
        if(ScoreBoard)
        {
            ScoreBoard.Score = score;
        }
    }

    void Awake()
    {
        int size = BoxSize.Size;
        Cubes = new CubeMain[size];
        Numbers = new int[size];
    }

	// Use this for initialization
	void Start () {
        SetScore(0);
        NewRandomCube(StartCount);
	}

    private bool m_bIsTouchMove = false;
	// Update is called once per frame
	void Update ()
    {
        BoxBoader boader = null;
        if (Input.mousePresent)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            if (hitInfo.collider)
            {
                boader = hitInfo.collider.gameObject.GetComponent<BoxBoader>();
            }
            if(boader && Input.GetMouseButtonDown(0))
            {
                OnBoaderClicked(boader.transform);
            }
        }
        else if(Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            var ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            if (hitInfo.collider)
            {
                boader = hitInfo.collider.gameObject.GetComponent<BoxBoader>();
            }
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        m_bIsTouchMove = false;
                    }
                    break;
                case TouchPhase.Moved:
                    {
                        m_bIsTouchMove = true;
                    }
                    break;
                case TouchPhase.Ended:
                    {
                        if (m_bIsTouchMove == false && boader)
                        {
                            OnBoaderClicked(boader.transform);
                        }
                    }
                    break;
            }
        }
        else
            m_bIsTouchMove = true;


        if (curBoader != boader)
        {
            if (curBoader)
                curBoader.Selected = false;
            curBoader = boader;
            if (curBoader)
                curBoader.Selected = true;
        }

        if (IsMoving)
            UpdateMoveAni();
	}

    delegate int GetIndexFuncType(int x, int y,int z);
    delegate Vector3 GetPosFuncType(int x, int y, int z);
    public void OnBoaderClicked(Transform tranBoader)
    {
        if (IsMoving)
            return;
        var localPos = tranBoader.localPosition;
        if(localPos.x > 0)
        {
            MoveAllCube(BoxSize.Y, BoxSize.Z, BoxSize.X, (int xx, int yy, int zz) =>
            {
                return GetIndex(BoxSize.X - zz - 1, xx, yy);
            }, 
            (int xx, int yy, int zz) =>
            {
                return GetPos(BoxSize.X - zz - 1, xx, yy);
            });
        }
        else if(localPos.x < 0)
        {
            MoveAllCube(BoxSize.Y, BoxSize.Z, BoxSize.X, (int xx, int yy, int zz) =>
            {
                return GetIndex(zz, xx, yy);
            },
            (int xx, int yy, int zz) =>
            {
                return GetPos(zz, xx, yy);
            });
        }
        else if(localPos.y > 0)
        {
            MoveAllCube(BoxSize.X, BoxSize.Z, BoxSize.Y, (int xx, int yy, int zz) =>
            {
                return GetIndex(xx, BoxSize.Y - zz - 1, yy);
            },
            (int xx, int yy, int zz) =>
            {
                return GetPos(xx, BoxSize.Y - zz - 1, yy);
            });
        }
        else if (localPos.y < 0)
        {
            MoveAllCube(BoxSize.X, BoxSize.Z, BoxSize.Y, (int xx, int yy, int zz) =>
            {
                return GetIndex(xx, zz, yy);
            },
            (int xx, int yy, int zz) =>
            {
                return GetPos(xx, zz, yy);
            });
        }
        else if (localPos.z > 0)
        {
            MoveAllCube(BoxSize.X, BoxSize.Y, BoxSize.Z, (int xx, int yy, int zz) =>
            {
                return GetIndex(xx, yy, BoxSize.Z - zz - 1);
            },
            (int xx, int yy, int zz) =>
            {
                return GetPos(xx, yy, BoxSize.Z - zz - 1);
            });
        }
        else if (localPos.z < 0)
        {
            MoveAllCube(BoxSize.X, BoxSize.Y, BoxSize.Z, (int xx, int yy, int zz) =>
            {
                return GetIndex(xx, yy, zz);
            },
            (int xx, int yy, int zz) =>
            {
                return GetPos(xx, yy, zz);
            });
        }
    }

    void MoveAllCube(int x, int y, int z, GetIndexFuncType getIdx, GetPosFuncType getPos)
    {
        bool bMoved = false;
        for(int i = 0; i < x; ++i)
        {
            for (int j = 0; j < y; ++j)
            {
                int startZ = 0;
                int startIdx = getIdx(i, j, startZ);
                for (int k = startZ + 1; k < z; ++k)
                {
                    int curIdx = getIdx(i, j, k);
                    var curCube = Cubes[curIdx];
                    if (curCube == null)
                        continue;
                    if(Numbers[startIdx] == 0)
                    {
                        Numbers[startIdx] = Numbers[curIdx];
                        Numbers[curIdx] = 0;
                        curCube.Move(getPos(i, j, startZ), MOVE_ANI_PER_BLOCK_DURATION * (k - startZ));
                        bMoved = true;
                        Cubes[startIdx] = curCube;
                        Cubes[curIdx] = null;
                    }
                    else if (Numbers[startIdx] != Numbers[curIdx])
                    {
                        startIdx = getIdx(i, j, ++startZ);
                        if (startIdx == curIdx)
                            continue;
                        Numbers[startIdx] = Numbers[curIdx];
                        Numbers[curIdx] = 0;
                        curCube.Move(getPos(i, j, startZ), MOVE_ANI_PER_BLOCK_DURATION * (k - startZ));
                        bMoved = true;
                        Cubes[startIdx] = curCube;
                        Cubes[curIdx] = null;
                    }
                    else
                    {
                        Numbers[startIdx] *= 2;
                        int tarNum = Numbers[startIdx];
                        Numbers[curIdx] = 0;
                        var tarCube = Cubes[startIdx];
                        curCube.Move(getPos(i, j, startZ), MOVE_ANI_PER_BLOCK_DURATION * (k - startZ), () =>
                        {
                            AddScore(curCube.Number);
                            curCube.Number = tarNum;
                            tarCube.Destroy();
                        });
                        bMoved = true;
                        Cubes[startIdx] = curCube;
                        Cubes[curIdx] = null;
                        startIdx = getIdx(i, j, ++startZ);
                    }
                }
            }
        }
        if (!bMoved)
            return;
        IsMoving = true;
        UpdateMoveAni();
    }

    void UpdateMoveAni()
    {
        bool AniEnd = true;
        foreach(var cube in Cubes)
        {
            if(cube != null && cube.IsMoving)
            {
                AniEnd = false;
                break;
            }
        }
        if (AniEnd)
            OnMoveAniEnd();
    }

    void OnMoveAniEnd()
    {
        IsMoving = false;
        NewRandomCube(NewCount);
    }
}
