using System;
using UnityEngine;
using UniRx;

/// <summary>
/// オセロの盤ビュー
/// </summary>
public class BoardView : MonoBehaviour
{
    [SerializeField, Tooltip("配置する石の複製元")]
    private DiskView diskOriginal = default;

    [SerializeField, Tooltip("選択中枠の複製元")]
    private GameObject selectedFrameOriginal = default;

    [SerializeField, Tooltip("配置する石の親となるトランスフォーム")]
    private Transform diskParent = default;

    [SerializeField, Tooltip("選択中枠の親となるトランスフォーム")]
    private Transform selectedFrameParent = default;

    [SerializeField, Tooltip("盤のコライダー")]
    private Collider boardCollider = default;

    [SerializeField, Tooltip("盤の座標系モデル")]
    private BoardCoordinateModel coordinateModel = default;


    /// <summary>
    /// 盤に置かれた石
    /// </summary>
    private DiskView[,] DiskViews { get; } = new DiskView[BoardCoordinateModel.SquareNumberInLine, BoardCoordinateModel.SquareNumberInLine];

    /// <summary>
    /// 選択中の枠
    /// </summary>
    private GameObject SelectedFrame { get; set; }

    /// <summary>
    /// マスを選択中か
    /// </summary>
    private bool IsSelecting { get; set; }


    /// <summary>
    /// 盤のクリックのストリームソース
    /// </summary>
    private ISubject<Vector2Int> ClickSubject { get; } = new Subject<Vector2Int>();

    /// <summary>
    /// 盤のクリックのイベント
    /// </summary>
    public IObservable<Vector2Int> ClickObservable => ClickSubject;


    /// <summary>
    /// 盤のマスを選択します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    private void Select(Vector2Int boardPosition)
    {
        // 引数が正しいかを確認。
        var isInRange = coordinateModel.GetIsInRange(boardPosition);
        if (!isInRange) {
            throw new ArgumentOutOfRangeException("盤の範囲外が指定されました。");
        }

        // 盤上の座標をワールド座標に変換。
        var worldPosition = coordinateModel.GetWorldPosition(boardPosition);

        // 既に枠があったら指定座標に移動。
        if (SelectedFrame) {
            SelectedFrame.transform.position = worldPosition;
        }
        // 無かったら指定座標に生成。
        else {
            SelectedFrame = Instantiate(selectedFrameOriginal, worldPosition, Quaternion.identity, selectedFrameParent);
        }
    }

    /// <summary>
    /// 盤のマスの選択を解除します。
    /// </summary>
    private void Deselect()
    {
        if (!SelectedFrame) {
            throw new Exception("まだマスが選択されていないのに、選択解除しようとしました。");
        }
        Destroy(SelectedFrame);
    }


    /// <summary>
    /// 盤上に石を置きます。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <param name="isBlack"> 表面は黒か </param>
    public void PutDisk(Vector2Int boardPosition, bool isBlack)
    {
        // 引数が正しいかを確認。
        var isInRange = coordinateModel.GetIsInRange(boardPosition);
        if (!isInRange) {
            throw new ArgumentOutOfRangeException("盤の範囲外が指定されました。");
        }
        if (DiskViews[boardPosition.y, boardPosition.x]) {
            throw new ArgumentException("既に石が置かれています。");
        }

        // 指定された座標に石を生成。
        var putDisk = Instantiate(diskOriginal, coordinateModel.GetWorldPosition(boardPosition), Quaternion.identity, diskParent);
        // 置かれた石を二次元配列に保持。
        DiskViews[boardPosition.y, boardPosition.x] = putDisk;

        // 指定された表面の色に合わせてひっくり返す。
        putDisk.Turn(isBlack);
    }

    /// <summary>
    /// 盤上の石をひっくり返します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    public void TurnDisk(Vector2Int boardPosition)
    {
        // 引数が正しいかを確認。
        var isInRange = coordinateModel.GetIsInRange(boardPosition);
        if (!isInRange) {
            throw new ArgumentOutOfRangeException("盤の範囲外が指定されました。");
        }
        if (!DiskViews[boardPosition.y, boardPosition.x]) {
            throw new ArgumentException("石が置かれていません。");
        }

        // 指定された座標の石をひっくり返す。
        DiskViews[boardPosition.y, boardPosition.x].Turn();
    }

    /// <summary>
    /// 盤の任意のマスの状態を取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> 盤の任意のマスの状態 </returns>
    public SquareState GetSquareState(Vector2Int boardPosition)
    {
        // 引数が正しいかを確認。
        var isInRange = coordinateModel.GetIsInRange(boardPosition);
        if (!isInRange) {
            throw new ArgumentOutOfRangeException("盤の範囲外が指定されました。");
        }

        var diskView = DiskViews[boardPosition.y, boardPosition.x];

        // まだ石が置かれていない。
        if (!diskView) {
            return SquareState.Empty;
        }

        // 黒石が置かれている。
        if (diskView.IsBlack) {
            return SquareState.Black;
        }
        // 白石が置かれている。
        else {
            return SquareState.White;
        }
    }


    private void Start()
    {
        // オセロの初期配置。
        PutDisk(new Vector2Int(3, 3), false);
        PutDisk(new Vector2Int(3, 4), true);
        PutDisk(new Vector2Int(4, 3), true);
        PutDisk(new Vector2Int(4, 4), false);
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // マウスが盤を指していたら
        if (boardCollider.Raycast(ray, out var hitInfo, float.PositiveInfinity)) {
            var boardPosition = coordinateModel.GetBoardPosition(hitInfo.point);

            // 選択中の枠を生成or移動。
            Select(boardPosition);
            IsSelecting = true;

            // このフレームにマウスクリックしていたら、選択中マスクリックイベント発火。
            if (!Input.GetMouseButtonDown(0)) { return; }
            ClickSubject.OnNext(boardPosition);
        }
        // マウスが盤を指していなかったら
        else {
            // 選択中の枠を削除。
            if (!IsSelecting) { return; }
            Deselect();
            IsSelecting = false;
        }
    }
}
