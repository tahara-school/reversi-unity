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

    [SerializeField, Tooltip("盤の中心のトランスフォーム")]
    private Transform centerWorldTransform = default;

    [SerializeField, Tooltip("盤の座標系を表すトランスフォーム")]
    private Transform boardBasisTransform = default;

    [SerializeField, Tooltip("1マスの大きさ(m)"), Min(0f)]
    private float squareScale = 1f;


    /// <summary>
    /// 盤の横一列のマスの数
    /// </summary>
    private static int SquareNumberInLine => 8;

    /// <summary>
    /// 盤の範囲外が指定されたときの例外
    /// </summary>
    private static ArgumentOutOfRangeException OutOfRangeException => new ArgumentOutOfRangeException("盤の範囲外が指定されました。");


    /// <summary>
    /// 盤の中心のワールド座標
    /// </summary>
    private Vector3 CenterWorldPosition => centerWorldTransform.position;

    /// <summary>
    /// 盤の原点のワールド座標
    /// </summary>
    private Vector3 OriginWorldPosition
    {
        get
        {
            // 盤座標系での、中心→原点オフセット
            var boardOffset = SquareNumberInLine / 2f - 0.5f;
            // ワールド座標系での、中心→原点オフセット
            var worldOffset = boardOffset * squareScale;
            // 中心からXY軸へそれぞれオフセットを適応し、原点の座標を取得する。
            return CenterWorldPosition - boardBasisTransform.right * worldOffset - boardBasisTransform.up * worldOffset;
        }
    }

    /// <summary>
    /// 盤上の座標からワールド座標を取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> ワールド座標 </returns>
    private Vector3 GetWorldPosition(Vector2Int boardPosition)
    {
        var x = boardPosition.x * squareScale;
        var y = boardPosition.y * squareScale;
        return OriginWorldPosition + boardBasisTransform.right * x + boardBasisTransform.up * y;
    }

    /// <summary>
    /// 盤上に石を置きます。
    /// </summary>
    /// <param name="position"> 盤上の座標 </param>
    public void PutDisk(Vector2Int boardPosition)
    {
        // 範囲外だったら例外を吐く。
        var isInRange = MathUtility.IsInRange(boardPosition, new Vector2Int(0, 0), new Vector2Int(SquareNumberInLine - 1, SquareNumberInLine - 1));
        if (!isInRange) {
            throw OutOfRangeException;
        }

        // 指定された座標に石を生成。
        Instantiate(diskOriginal, GetWorldPosition(boardPosition), Quaternion.identity, diskParent);
    }
}
