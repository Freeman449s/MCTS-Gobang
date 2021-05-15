using System;
using System.Collections;
using System.Collections.Generic;
using Gobang;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int boardWidth = 15;
    [SerializeField] private int boardHeight = 15;
    private bool playersTurn = true; // ��ǰ�Ƿ�����һغ�
    private GobangGameState gameState = null; // ��ǰ��Ϸ״̬

    [SerializeField] private Color unplacedColor = Color.white;
    [SerializeField] private Color playerColor = Color.green;
    [SerializeField] private Color computerColor = Color.red;

    private Camera mainCamera; // �����
    private Vector3 cameraTargetPosition; // �����Ŀ��λ��
    float cameraMoveSmooth = 3; // ����������ƶ�ƽ���̶�

    // Start is called before the first frame update
    void Start()
    {
        gameState = new GobangGameState(boardWidth, boardHeight);
        mainCamera = Camera.main;
        initGameBoard();
        cameraTargetPosition = new Vector3((boardWidth - 1) / 2f, (boardHeight - 1) / 2f,
            Mathf.Max(boardHeight, boardWidth) / 2f * Mathf.Sqrt(3) * -1.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (playersTurn)
            {
                Vector3 mousePos = Input.mousePosition;
                playerPlacePiece(mousePos);
            }
        }

        if (!playersTurn)
        {
        }
    }

    // LateUpdate()�����������Update()���ú����
    private void LateUpdate()
    {
        moveCamera();
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="mousePos">���λ��</param>
    void playerPlacePiece(Vector3 mousePos)
    {
        int[] cubeCoord = physicsCast(mousePos);
        if (cubeCoord == null) return;
        if (gameState.getPieceType(cubeCoord) != PieceType.Unplaced) return;
        gameState.boardState[cubeCoord[0], cubeCoord[1]].pieceType = PieceType.Player;
        paintCube(cubeCoord, playerColor);
        playersTurn = false;
    }

    /// <summary>
    /// ��������
    /// </summary>
    void computerPlacePiece()
    {
        MCTS mcts = new MCTS(gameState, 10000);
        GobangMove nextMove;
        MCTSGameMove MCTSMove = mcts.makeMove();
        if (MCTSMove is GobangMove) nextMove = (GobangMove) MCTSMove;
    }

    // ========================================== ���������빤�ߺ��� ==========================================
    void initGameBoard()
    {
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                gameState.boardState[x, y].gameObj = createCube(x, y, unplacedColor);
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

    /// <summary>
    /// ��ÿ֡��Ⱦ��Ϻ���ã�ƽ���ƶ������Ŀ��λ��
    /// </summary>
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

    void paintCube(int[] coord, Color color)
    {
        gameState.boardState[coord[0], coord[1]].gameObj.GetComponent<Renderer>().material.color = color;
    }

    /// <summary>
    /// �������ѡ�з���ģ���ά������
    /// </summary>
    /// <returns>���ѡ�з���ģ���ά�����ꡣû������ʱ����null</returns>
    int[] physicsCast(Vector3 mousePos)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos); // ���ɴ�ֱ����Ļ��������ռ������
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Abs(cameraTargetPosition.z) * 2)) // ������߻������壬��ִ������Ĵ���
        {
            Transform hitObjTransform = hitInfo.collider.transform;
            int[] coord = new int[2];
            coord[0] = (int) hitObjTransform.position.x;
            coord[1] = (int) hitObjTransform.position.y;
            return coord;
        }

        return null;
    }
}