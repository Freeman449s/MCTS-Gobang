using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int boardWidth = 15;
    [SerializeField] private int boardHeight = 15;

    [SerializeField] private Color unplacedColor = Color.white;
    [SerializeField] private Color playerColor = Color.green;
    [SerializeField] private Color computerColor = Color.red;

    private Camera mainCamera; // 主相机
    private Vector3 cameraTargetPosition; // 主相机目标位置
    float cameraMoveSmooth = 3; // 控制主相机移动平滑程度

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        initGameBoard();
        cameraTargetPosition = new Vector3((boardWidth - 1) / 2f, (boardHeight - 1) / 2f,
            Mathf.Max(boardHeight, boardWidth) / 2f * Mathf.Sqrt(3) * -1.1f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // LateUpdate()在所有组件的Update()调用后调用
    private void LateUpdate()
    {
        moveCamera();
    }

    void initGameBoard()
    {
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                createCube(x, y, unplacedColor);
            }
        }
    }

    private GameObject createCube(int x, int y, Color c)
    {
        GameObject gameObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObj.transform.position = new Vector3(x, y, 0);
        gameObj.transform.localScale = Vector3.one * 0.9f;
        gameObj.GetComponent<Renderer>().material.color = c;

        return gameObj;
    }

    void moveCamera()
    {
        if (Vector3.Distance(mainCamera.transform.position, cameraTargetPosition) > 0.05f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraTargetPosition,
                Time.deltaTime * cameraMoveSmooth);
        }
        else
        {
            mainCamera.transform.position = cameraTargetPosition;
        }
    }

    void physicsCast()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // 生成垂直于屏幕，到世界空间的射线
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Abs(cameraTargetPosition.z))) // 如果射线击中物体，则执行下面的代码
        {
            // TODO
        }
    }
}