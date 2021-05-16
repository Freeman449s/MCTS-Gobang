using System;
using System.Collections;
using System.Collections.Generic;
using Gobang;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // �����߼�
    [SerializeField] private int boardSideLength = 15;
    [SerializeField] private int N_SIMULATION_TIMES = 10000; // �������Ľڵ���
    private bool playersTurn = true; // ��ǰ�Ƿ�����һغ�
    private GobangGameState gameState = null; // ��ǰ��Ϸ״̬
    private bool gameRunning = true; // ��Ϸ�Ƿ���������

    // ��ɫ����
    [SerializeField] private Color unplacedColor = new Color(219 / 255f, 174 / 255f, 106 / 255f);
    [SerializeField] private Color playerColor = Color.black;
    [SerializeField] private Color computerColor = Color.white;

    // ���
    private Camera mainCamera; // �����
    private Vector3 cameraTargetPosition; // �����Ŀ��λ��
    float cameraMoveSmooth = 3; // ����������ƶ�ƽ���̶�

    // UI
    [SerializeField] private GameObject reminder;

    // Start is called before the first frame update
    void Start()
    {
        if (reminder == null) reminder = GameObject.Find("Reminder");
        hideReminder();
        if (boardSideLength >= 5)
        {
            gameState = new GobangGameState(boardSideLength);
            mainCamera = Camera.main;
            initGameBoard();
            cameraTargetPosition = new Vector3((boardSideLength - 1) / 2f, (boardSideLength - 1) / 2f,
                boardSideLength / 2f * Mathf.Sqrt(3) * -1.1f);
        }
        else
        {
            Debug.LogWarning("Game initiation failed. Game board must be larger than 5x5.");
            gameRunning = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            if (playersTurn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePos = Input.mousePosition;
                    playerPlacePiece(mousePos);
                    if (gameRunning) showReminder("�ȴ����ԡ�����");
                }
            }
            else
            {
                long startTime = Environment.TickCount;
                computerPlacePiece();
                long endTime = Environment.TickCount;
                double deltaTime = endTime - startTime;
                deltaTime = Math.Round(deltaTime / 1000, 2);
                Debug.Log("�������ӡ���ʱԼ" + deltaTime + "�롣");
            }
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
        GobangPoint point = gameState.boardState[cubeCoord[0], cubeCoord[1]];
        point.pieceType = PieceType.Player;
        GobangMove move = new GobangMove(point);
        gameState.placePiece(move);
        paintCube(cubeCoord, playerColor);
        GameResult result = gameState.judgeLastMove();
        if (result != GameResult.NoOutcome)
        {
            gameRunning = false;
            reminder.GetComponent<CanvasGroup>().alpha = 1;
            if (result == GameResult.PlayerWon) showReminder("��Ӯ�ˣ�");
            else if (result == GameResult.Draw) showReminder("ƽ��");
        }

        playersTurn = false;
    }

    /// <summary>
    /// ��������
    /// </summary>
    void computerPlacePiece()
    {
        MCTS mcts = new MCTS(gameState, N_SIMULATION_TIMES);
        GobangMove nextMove;
        MCTSGameMove MCTSMove = mcts.decideMove();
        if (MCTSMove is GobangMove)
        {
            nextMove = (GobangMove) MCTSMove;
            GobangPoint point = nextMove.point;
            if (gameState.boardState[point.coord[0], point.coord[1]].pieceType != PieceType.Unplaced)
                Debug.LogWarning("Selected a point already had a piece.");
            point.pieceType = PieceType.Computer;
            gameState.placePiece(new GobangMove(point));
            paintCube(point.coord, computerColor);
            GameResult result = gameState.judgeLastMove();
            if (result != GameResult.NoOutcome)
            {
                gameRunning = false;
                reminder.GetComponent<CanvasGroup>().alpha = 1;
                if (result == GameResult.ComputerWon) showReminder("ʤ���˱��ҳ���\n��������������");
                else if (result == GameResult.Draw) showReminder("ƽ��");
            }

            if (gameRunning) hideReminder();
            playersTurn = true;
        }
        else
        {
            Debug.LogError("Cannot convert variable nextMove to type GobangMove.");
        }
    }

    // ========================================== ���������빤�ߺ��� ==========================================
    void initGameBoard()
    {
        for (int x = 0; x < boardSideLength; x++)
        {
            for (int y = 0; y < boardSideLength; y++)
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

    // ======================================== ���ߺ��� ========================================
    void showReminder(String text = null)
    {
        reminder.GetComponent<CanvasGroup>().alpha = 1;
        if (text != null) reminder.GetComponent<Text>().text = text;
    }

    void hideReminder()
    {
        reminder.GetComponent<CanvasGroup>().alpha = 0;
    }
}