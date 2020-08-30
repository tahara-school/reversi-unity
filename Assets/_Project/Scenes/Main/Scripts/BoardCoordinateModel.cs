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

    [SerializeField, Tooltip("X軸Y軸それぞれのマスの数")]
    private Vector2Int squaresNumbers = new Vector2Int(8, 8);


    /// <summary>
    /// 盤の座標系のX軸
    /// </summary>
    private Vector3 AxisX => boardBasisTransform.right;

    /// <summary>
    /// 盤の座標系のY軸
    /// </summary>
    private Vector3 AxisY => boardBasisTransform.up;


    /// <summary>
    /// 盤の座標の最大値
    /// </summary>
    private Vector2Int MaxBoardPosition => new Vector2Int(SquaresNumbers.x - 1, SquaresNumbers.y - 1);

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
            var boardOffset = MathUtility.ToVector2Float(SquaresNumbers) / 2f;
            // ワールド座標系での、中心→原点オフセット
            var worldOffset = boardOffset * squareScale;
            // 中心からXY軸へそれぞれオフセットを適応し、原点の座標を取得する。
            return CenterWorldPosition - AxisX * worldOffset.x - AxisY * worldOffset.y;
        }
    }


    /// <summary>
    /// X軸Y軸それぞれのマスの数
    /// </summary>
    public Vector2Int SquaresNumbers => squaresNumbers;


    /// <summary>
    /// 任意の盤上の座標が範囲外かを取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> 任意の盤上の座標が範囲外か </returns>
    public bool GetIsInRange(Vector2Int boardPosition)
    {
        return MathUtility.IsInRange(boardPosition, Vector2Int.zero, MaxBoardPosition);
    }

    /// <summary>
    /// ワールド座標から盤上の座標を取得します。
    /// </summary>
    /// <param name="worldPosition"> ワールド座標(面に触れていなくても良い) </param>
    /// <returns> 盤上の座標 </returns>
    public Vector2Int GetBoardPosition(Vector3 worldPosition)
    {
        // 盤の原点からの相対座標に変換。
        var relativePosition = worldPosition - OriginWorldPosition;
        // 盤のXY軸それぞれに投影し、長さを計算。
        var dx = Vector3.Dot(AxisX, relativePosition);
        var dy = Vector3.Dot(AxisY, relativePosition);
        // その長さがマス何個分の位置にあるかを計算。
        var x = Mathf.FloorToInt(dx / squareScale);
        var y = Mathf.FloorToInt(dy / squareScale);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// 盤上の座標からワールド座標を取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> ワールド座標 </returns>
    public Vector3 GetWorldPosition(Vector2Int boardPosition)
    {
        // マスの中心に合わせるために0.5fを加算。
        var x = (boardPosition.x + 0.5f) * squareScale;
        var y = (boardPosition.y + 0.5f) * squareScale;
        return OriginWorldPosition + AxisX * x + AxisY * y;
    }
}
