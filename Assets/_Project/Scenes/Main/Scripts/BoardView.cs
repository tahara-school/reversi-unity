using System;
using UnityEngine;

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
    /// 盤上に石を置きます。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    public void PutDisk(Vector2Int boardPosition)
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
    }

    /// <summary>
    /// 盤上の石をひっくり返します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    public void TurnDisk(Vector2Int boardPosition)
    {
        if (!DiskViews[boardPosition.y, boardPosition.x]) {
            throw new ArgumentException("石が置かれていません。");
        }

        // 指定された座標の石をひっくり返す。
        DiskViews[boardPosition.y, boardPosition.x].Turn();
    }

    /// <summary>
    /// 盤のマスを選択します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    public void Select(Vector2Int boardPosition)
    {
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
    public void Deselect()
    {
        if (!SelectedFrame) {
            throw new Exception("まだマスが選択されていないのに、選択解除しようとしました。");
        }
        Destroy(SelectedFrame);
    }
}
