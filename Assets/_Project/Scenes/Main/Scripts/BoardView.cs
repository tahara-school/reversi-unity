using System;
using UnityEngine;

/// <summary>
/// オセロの盤ビュー
/// </summary>
public class BoardView : MonoBehaviour
{
    [SerializeField, Tooltip("配置する石の複製元")]
    private DiskView diskOriginal = default;

    [SerializeField, Tooltip("配置する石の親となるトランスフォーム")]
    private Transform diskParent = default;

    [SerializeField, Tooltip("盤の座標系モデル")]
    private BoardCoordinateModel coordinateModel = default;


    /// <summary>
    /// 盤に置かれた石
    /// </summary>
    private DiskView[,] DiskViews { get; } = new DiskView[BoardCoordinateModel.SquareNumberInLine, BoardCoordinateModel.SquareNumberInLine];


    /// <summary>
    /// 盤上に石を置きます。
    /// </summary>
    /// <param name="position"> 盤上の座標 </param>
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
}
