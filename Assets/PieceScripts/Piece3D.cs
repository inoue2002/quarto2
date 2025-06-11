using UnityEngine;

public class Piece3D : MonoBehaviour, IPiece
{
    [SerializeField]
    private PieceId piece3dId;
    
    [SerializeField]
    private bool isSelected = false;
    
    // ハイライト用のマテリアル
    [SerializeField]
    private Material defaultMaterial;
    
    [SerializeField]
    private Material selectedMaterial;
    
    // レンダラーコンポーネント
    private Renderer pieceRenderer;
    
    // ドメイン層のPieceインスタンス
    private Piece domainPiece;
    
    
    // 子クラスでオーバーライドできるようにvirtualにする
    protected virtual void Awake()
    {
        pieceRenderer = GetComponent<Renderer>();
        if (pieceRenderer != null && defaultMaterial == null)
        {
            defaultMaterial = pieceRenderer.material;
        }
        
        // ドメイン層のPieceインスタンスを作成
        try
        {
            domainPiece = new Piece(piece3dId);
            Debug.Log($"Piece3D: {gameObject.name} のドメインPieceを作成しました。PieceId: {piece3dId}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Piece3D: ドメインPieceの作成に失敗しました: {e.Message}");
        }


    }
    
    // PieceIdを設定するメソッドを追加
    public void SetPieceId(PieceId id)
    {
        piece3dId = id;
        
        // ドメインPieceを更新
        if (domainPiece != null)
        {
            domainPiece = new Piece(piece3dId);
        }
    }
    
    private void OnMouseDown()
    {
        GameObject viewControllerObject = GameObject.Find("ViewController");
        ViewController viewController = viewControllerObject.GetComponent<ViewController>();
        // if (ViewController.Instance != null)
        // {
        //     ViewController.Instance.OnPieceSelected(gameObject);
        //     Select(); // 選択状態にする
        // }
        // else
        // {
        //     Debug.LogError("ViewController.Instance が設定されていません！");
        // }

        SelectPieceByUserCommand selectPieceByUserCommand = new SelectPieceByUserCommand();
        selectPieceByUserCommand.pieceId = piece3dId;

        if(viewController.getInfo().type == GamePhaseType.SelectPieceByUser)
        {
            viewController.execute(selectPieceByUserCommand);
        }

    }
    
    // IPieceインターフェースの実装
    public void Select()
    {
        isSelected = true;
        
        // 選択時の見た目を変更
        if (pieceRenderer != null && selectedMaterial != null)
        {
            pieceRenderer.material = selectedMaterial;
        }
    }
    
    public void Deselect()
    {
        isSelected = false;
        
        // 通常時の見た目に戻す
        if (pieceRenderer != null && defaultMaterial != null)
        {
            pieceRenderer.material = defaultMaterial;
        }
    }
    
    public bool IsSelected
    {
        get { return isSelected; }
    }
    
    // PieceIdプロパティ
    public PieceId getPieceId()
    {
        return piece3dId;
    }
    
    // ドメイン層のPieceインスタンスを取得
    public Piece GetDomainPiece()
    {
        if (domainPiece == null)
        {
            domainPiece = new Piece(piece3dId);
        }
        return domainPiece;
    }
}