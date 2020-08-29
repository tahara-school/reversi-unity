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
    /// 盤の範囲外が指定されたときの例外
    /// </summary>
    private static ArgumentOutOfRangeException OutOfRangeException => new ArgumentOutOfRangeException("盤の範囲外が指定されました。");


    /// <summary>
    /// 盤上に石を置きます。
    /// </summary>
    /// <param name="position"> 盤上の座標 </param>
    public void PutDisk(Vector2Int boardPosition)
    {
        // 範囲外だったら例外を吐く。
        var isInRange = coordinateModel.GetIsInRange(boardPosition);
        if (!isInRange) {
            throw OutOfRangeException;
        }

        // 指定された座標に石を生成。
        Instantiate(diskOriginal, coordinateModel.GetWorldPosition(boardPosition), Quaternion.identity, diskParent);
    }
}
