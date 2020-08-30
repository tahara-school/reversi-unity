using UnityEngine;

/// <summary>
/// オセロの石の情報構造体
/// </summary>
public struct DiskInformation
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="isBlack"> 黒面か </param>
    /// <param name="position"> 石の盤上の座標 </param>
    public DiskInformation(bool isBlack, Vector2Int position)
    {
        IsBlack = isBlack;
        Position = position;
    }

    /// <summary>
    /// 黒面か
    /// </summary>
    public bool IsBlack { get; set; }

    /// <summary>
    /// 石の盤上の座標
    /// </summary>
    public Vector2Int Position { get; set; }
}
