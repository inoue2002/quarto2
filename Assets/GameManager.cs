using UnityEngine;

/// <summary>
/// ゲーム全体の状態を管理するシングルトンクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // プレイヤー1の選択情報
    private string _player1PutSelection;
    private string _player1GiveSelection;
    
    // プレイヤー2の選択情報
    private string _player2PutSelection;
    private string _player2GiveSelection;
    
    // プロパティ
    public string Player1PutSelection 
    { 
        get { return _player1PutSelection; } 
        set { _player1PutSelection = value; } 
    }
    
    public string Player1GiveSelection 
    { 
        get { return _player1GiveSelection; } 
        set { _player1GiveSelection = value; } 
    }
    
    public string Player2PutSelection 
    { 
        get { return _player2PutSelection; } 
        set { _player2PutSelection = value; } 
    }
    
    public string Player2GiveSelection 
    { 
        get { return _player2GiveSelection; } 
        set { _player2GiveSelection = value; } 
    }
    
    // シングルトンの保証
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // プレイヤータイプを取得（Player/CPUの区別）
    public PlayerType GetPlayerType(string selection)
    {
        if (selection == null) return PlayerType.Player;
        
        if (selection.ToLower().Contains("human"))
            return PlayerType.Player;
        else
            return PlayerType.Cpu;
    }
    
    // CPUレベルを取得（1-5）
    public int GetCpuLevel(string selection)
    {
        if (selection == null) return 1;
        
        if (selection.ToLower().Contains("cpu"))
        {
            string level = selection.Substring(selection.Length - 1);
            int.TryParse(level, out int result);
            return result > 0 ? result : 1;
        }
        
        return 1;
    }
    
    // 選択情報をリセット
    public void ResetSelections()
    {
        _player1PutSelection = null;
        _player1GiveSelection = null;
        _player2PutSelection = null;
        _player2GiveSelection = null;
    }
} 