using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    public static ViewController Instance { get; private set; }
    public GameObject SelectedPiece { get; private set; }
    
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private GameStartButton gameStartButton;

    public Vector3 fixedPosition = new Vector3(10f, 0f, 0f); // 固定位置

    [SerializeField]
    private TextMeshProUGUI PlayerTurnText; // プレイヤーのターンを表示するテキスト

    private int currentPlayer = 1; // 現在のプレイヤー（1または2）

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わっても破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Audio Listenerが複数あるか確認し、必要に応じて削除
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (listeners.Length > 1)
        {
            Debug.LogWarning($"シーンに{listeners.Length}個のAudio Listenerが見つかりました。1つだけ残します。");
            for (int i = 1; i < listeners.Length; i++)
            {
                Debug.Log($"余分なAudio Listener '{listeners[i].gameObject.name}' を削除します。");
                Destroy(listeners[i]);
            }
        }

        // GameControllerがアタッチされていない場合は探す
        if (gameController == null)
        {
            gameController = FindFirstObjectByType<GameController>();
            
            // 見つからない場合は新しく作成
            if (gameController == null)
            {
                GameObject controllerObj = new GameObject("GameController");
                gameController = controllerObj.AddComponent<GameController>();
                DontDestroyOnLoad(controllerObj);
            }
        }
    }

    void Start()
    {
        try
        {
            // シーンに存在するGameStartButtonを探す
            if (gameStartButton == null)
            {
                gameStartButton = FindFirstObjectByType<GameStartButton>();
                if (gameStartButton != null)
                {
                    Debug.Log("GameStartButtonをシーンから見つけました");
                }
            }

            // ゲーム開始ボタンのクリックイベントにリスナーを追加
            if (gameStartButton != null)
            {
                if (gameStartButton.gameStartButton != null)
                {
                    gameStartButton.gameStartButton.onClick.AddListener(OnGameStart);
                    Debug.Log("GameStartButtonのリスナーを設定しました");
                }
                else
                {
                    Debug.LogWarning("gameStartButton.gameStartButtonがnullです。GameStartButtonコンポーネントのButton参照を確認してください。");
                }
            }
            else
            {
                Debug.LogWarning("gameStartButtonがnullです。シーンにGameStartButtonコンポーネントが存在するか確認してください。");
            }

            // PlayerTurnTextが設定されていない場合、TextMeshProUGUIを探す
            if (PlayerTurnText == null)
            {
                PlayerTurnText = FindFirstObjectByType<TextMeshProUGUI>();
                if (PlayerTurnText != null)
                {
                    Debug.Log("PlayerTurnTextをシーンから見つけました");
                }
                else
                {
                    Debug.LogWarning("PlayerTurnTextが見つかりませんでした。シーンにTextMeshProUGUIコンポーネントが存在するか確認してください。");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ViewControllerのStart()メソッドでエラーが発生しました: {e.Message}\n{e.StackTrace}");
        }
    }

    public void OnGameStart()
    {
        // ゲーム開始時にプレイヤー1のターンを表示
        currentPlayer = 1;
        UpdatePlayerTurnText();
    }

    public void OnPieceSelected(GameObject piece)
    {
        SelectedPiece = piece;
        Debug.Log($"{piece.name} が選択されました。");
        MovePieceToFixedPosition(piece);
    }

    public void OnBoardCellClicked(Transform cellTransform)
    {
        if (SelectedPiece != null)
        {
            Debug.Log($"{cellTransform.position} がクリックされました。");
            
            // セルのBoardCellコンポーネントを取得
            BoardCell boardCell = cellTransform.GetComponent<BoardCell>();
            if (boardCell == null)
            {
                Debug.LogError("BoardCellコンポーネントが見つかりません。");
                return;
            }
            
            // セルの位置を取得
            Position cellPosition = boardCell.GetPosition();
            
            // 駒をセルに移動
            SelectedPiece.transform.position = new Vector3(cellTransform.position.x, SelectedPiece.transform.position.y, cellTransform.position.z);
            
            try
            {
                // Piece3Dコンポーネントを取得
                Piece3D piece3D = SelectedPiece.GetComponent<Piece3D>();
                if (piece3D != null)
                {
                    // コマンドの実行
                    var command = new PutPieceByUserCommand
                    {
                        pieceId = piece3D.getPieceId(),
                        position = cellPosition
                    };
                    
                    Debug.Log($"PutPieceByUserCommand作成: pieceId={piece3D.getPieceId()}, position=({cellPosition.X}, {cellPosition.Y})");
                    
                    // GameControllerが存在する場合のみ実行
                    if (gameController != null)
                    {
                        Result result = gameController.execute(command);
                        
                        // PutPieceResultにキャストして勝者を確認
                        if (result is PutPieceResult putPieceResult)
                        {
                            // クアルトが成立した場合
                            if (putPieceResult.winner != PlayerId.None)
                            {
                                Debug.Log($"クアルト成立！ 勝者: {putPieceResult.winner}");
                                QuartoMessageBox.ShowQuartoMessage();
                            }
                        }
                        else
                        {
                            Debug.LogWarning("ResultをPutPieceResultにキャストできませんでした");
                        }
                    }
                    else
                    {
                        Debug.LogError("GameControllerがnullです");
                    }
                    
                    // 駒の配置を完了
                    PlacePieceOnBoard(cellTransform);
                    
                    // プレイヤーのターンを切り替える
                    SwitchPlayerTurn();
                }
                else
                {
                    Debug.LogError("Piece3Dコンポーネントが見つかりません。");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"OnBoardCellClickedでエラーが発生しました: {e.Message}");
            }
        }
        else
        {
            Debug.Log("駒が選択されていません。");
        }
    }

    private void PlacePieceOnBoard(Transform cellTransform)
    {
        // 駒をクリックしたマスに移動
        SelectedPiece.transform.position = new Vector3(cellTransform.position.x, SelectedPiece.transform.position.y, cellTransform.position.z);
        Debug.Log($"{SelectedPiece.name} を {cellTransform.name} に配置しました。");
        SelectedPiece = null; // 駒の選択を解除
    }

    private void MovePieceToFixedPosition(GameObject piece)
    {
        // 固定位置に駒を移動
        piece.transform.position = fixedPosition;
        Debug.Log($"{piece.name} を固定位置に移動しました。");
    }

    public void CallGameController(Command command)
    {
        if (gameController != null)
        {
            gameController.execute(command);
        }
        else
        {
            Debug.LogError("GameController が設定されていません！");
        }
    }

    private void SwitchPlayerTurn()
    {
        // プレイヤーのターンを切り替える
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        UpdatePlayerTurnText();
    }

    private void UpdatePlayerTurnText()
    {
        // プレイヤーのターンを表示するテキストを更新
        if (PlayerTurnText != null)
        {
            PlayerTurnText.text = $"player{currentPlayer}";
        }
    }
}