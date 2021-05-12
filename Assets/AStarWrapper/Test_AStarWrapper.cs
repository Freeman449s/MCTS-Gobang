using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AStarWrapper : MonoBehaviour
{
    int mapWidth;
    int mapHeight;
    Point[,] map = null;
    List<Point> wallList = null;
    int wallNum;
    Point startPoint = null;
    Point targetPoint = null;
    List<Point> oldPath = null;
    List<Point> newPath = null;

    Color normalColor = Color.white;
    Color wallColor = Color.black;
    Color startPosColor = Color.green;
    Color targetPosColor = Color.red;
    Color pathPosColor = Color.yellow;

    Vector3 cameraTargetPosition;
    Camera cameraMain;
    float cameraMoveSmooth = 3;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            SelectStartPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(1))
        {

            SelectTargetPoint(Input.mousePosition);


        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefreshMap();
        }
    }
       
    void LateUpdate()
    {
        CameraMove();
    }



    void Init() {
        mapWidth = 12;
        mapHeight = 10;
        map = new Point[mapWidth, mapHeight];
        wallNum = (int)(mapWidth * mapHeight * 0.1f);
        wallList = new List<Point>();
        oldPath = new List<Point>();
        newPath = new List<Point>();
        InitMap(mapWidth, mapHeight);
        SetWall();

        cameraMain = Camera.main;
        cameraTargetPosition = new Vector3(mapWidth / 2, mapHeight / 2, cameraMain.transform.position.z);
    }

    void InitMap(int mapWidth, int mapHeight)
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y] = new Point(x, y, CreateCube(x, y, normalColor));
            }

        }
    }

    private void FindPathAndShowPath()
    {
        if (startPoint != null && targetPoint != null)
        {
            bool isFind = AStarWrapper.Instance.FindPath(startPoint, targetPoint, map, mapWidth, mapHeight);

            ClearOldPath();

            if (isFind == true)
            {
                ShowPath(startPoint, targetPoint);
            }
        }

    }

    private void ShowPath(Point startPoint, Point targetPoint)
    {      

        newPath.Clear();
        Point temp = targetPoint.Parent;
        while (true)
        {
            if (temp == startPoint)
            {
                break;
            }
            if (temp != null)
            {
                newPath.Add(temp);
            }

            temp = temp.Parent;
        }

        StartCoroutine(PaintnewPath());
    }

    IEnumerator PaintnewPath()
    {
        yield return new WaitForEndOfFrame();
        if (newPath.Count > 0)
        {
            for (int i = newPath.Count - 1; i >= 0; i--)
            {
                SetCubeColor(newPath[i].X, newPath[i].Y, pathPosColor);
                yield return new WaitForSeconds(0.05f);
            }
        }

    }

    private GameObject CreateCube(int x, int y, Color c)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(x, y, 0);
        go.transform.localScale = Vector3.one * 0.9f;
        go.GetComponent<Renderer>().material.color = c;

        return go;
    }
    private void SetCubeColor(int x, int y, Color c)
    {
        map[x, y].gameObject.GetComponent<Renderer>().material.color = c;
    }
    private void SetWall()
    {

        int current = Random.Range(wallNum - wallNum / 2, wallNum + wallNum / 2);

        // 还原为正常
        if (wallList != null)
        {
            foreach (Point wall in wallList)
            {
                wall.IsWall = false;
                SetCubeColor(wall.X, wall.Y, normalColor);
            }

            wallList.Clear();
        }
        while (true)
        {
            if (wallList.Count >= wallNum)
            {
                break;
            }

            Point p = map[Random.Range(0, mapWidth), Random.Range(0, mapHeight)];
            if (wallList.IndexOf(p) == -1)
            {
                wallList.Add(p);

            }
        }

        // 设置为 wall
        if (wallList != null)
        {
            foreach (Point wall in wallList)
            {
                wall.IsWall = true;
                SetCubeColor(wall.X, wall.Y, wallColor);
            }


        }
    }


    void SelectStartPoint(Vector3 mousePos)
    {
        Point tmp = PhysicCAst(mousePos);
        if (tmp != null)
        {
            ClearOldStartPoint();

            startPoint = tmp;
            SetCubeColor(startPoint.X, startPoint.Y, startPosColor);

            // 更新路径
            FindPathAndShowPath();
        }
    }

    private void SelectTargetPoint(Vector3 mousePos)
    {
        Point tmp = PhysicCAst(mousePos);
        if (tmp != null)
        {
            ClearOldTargetPoint();
            targetPoint = tmp;
            SetCubeColor(targetPoint.X, targetPoint.Y, targetPosColor);

            // 更新路径
            FindPathAndShowPath();
        }
    }

    private void RefreshMap()
    {
        ClearOldStartPoint();
        ClearOldTargetPoint();
        ClearOldPath();

        oldPath.Clear();
        newPath.Clear();

        SetWall();
    }

    void ClearOldStartPoint()
    {
        if (startPoint != null)
        {
            if (startPoint == targetPoint)
            {
                SetCubeColor(startPoint.X, startPoint.Y, targetPosColor);
            }
            else {
                SetCubeColor(startPoint.X, startPoint.Y, normalColor);
            }
            startPoint = null;
        }
    }
    void ClearOldTargetPoint()
    {
        if (targetPoint != null)
        {
            if (targetPoint == startPoint)
            {
                SetCubeColor(targetPoint.X, targetPoint.Y, startPosColor);
            }
            else
            {
                SetCubeColor(targetPoint.X, targetPoint.Y, normalColor);

            }
            targetPoint = null;
        }
    }

    void ClearOldPath()
    {
        oldPath = newPath;

        foreach (Point pathItem in oldPath)
        {
            SetCubeColor(pathItem.X, pathItem.Y, normalColor);
        }
        // 避免开始点或者目标点在老路径上被清掉，所以重新绘制一次开始点和目标点的颜色
        if (startPoint != null) {
            SetCubeColor(startPoint.X, startPoint.Y, startPosColor);
        }
        if (targetPoint != null) {
            SetCubeColor(targetPoint.X, targetPoint.Y, targetPosColor);
        }
        
    }

    Point PhysicCAst(Vector3 mousePos)
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 20))
        {
            Transform tmp = hitInfo.collider.transform;

            if (map[(int)tmp.position.x, (int)tmp.position.y].IsWall == false)
            {
                return map[(int)tmp.position.x, (int)tmp.position.y];
            }
        }

        return null;

    }







    void CameraMove()
    {

        if (Vector3.Distance(cameraMain.transform.position, cameraTargetPosition) > 0.05f)
        {
            cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, cameraTargetPosition, Time.deltaTime * cameraMoveSmooth);
        }
        else
        {
            cameraMain.transform.position = cameraTargetPosition;
        }
    }

    
}
