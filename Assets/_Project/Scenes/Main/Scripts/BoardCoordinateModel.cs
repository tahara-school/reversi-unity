using UnityEngine;

/// <summary>
/// 盤の座標系モデル
/// </summary>
public class BoardCoordinateModel : MonoBehaviour
{
    [SerializeField, Tooltip("盤の中心のトランスフォーム")]
    private Transform centerTransform = default;

    [SerializeField, Tooltip("盤の座標系を表すトランスフォーム")]
    private Transform boardBasisTransform = default;

    [SerializeField, Tooltip("1マスの大きさ(m)"), Min(0f)]
    private float squareScale = 1f;


    /// <summary>
    /// 盤の横一列のマスの数
    /// </summary>
    public static int SquareNumberInLine => 8;


    /// <summary> 
    /// 盤の中心のワールド座標
    /// </summary>
    private Vector3 CenterWorldPosition => centerTransform.position;

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
    /// 任意の盤上の座標が範囲外かを取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> 任意の盤上の座標が範囲外か </returns>
    public bool GetIsInRange(Vector2Int boardPosition)
    {
        return MathUtility.IsInRange(boardPosition, new Vector2Int(0, 0), new Vector2Int(SquareNumberInLine - 1, SquareNumberInLine - 1));
    }

    /// <summary>
    /// 盤上の座標からワールド座標を取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> ワールド座標 </returns>
    public Vector3 GetWorldPosition(Vector2Int boardPosition)
    {
        var x = boardPosition.x * squareScale;
        var y = boardPosition.y * squareScale;
        return OriginWorldPosition + boardBasisTransform.right * x + boardBasisTransform.up * y;
    }
}
