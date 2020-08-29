using UnityEngine;

/// <summary>
/// 数学の便利関数群
/// </summary>
public static class MathUtility
{
    /// <summary>
    /// 任意の座標が範囲内かを取得します。
    /// </summary>
    /// <param name="source"> 任意の座標 </param>
    /// <param name="min"> 最小値 </param>
    /// <param name="max"> 最大値 </param>
    /// <returns> 任意の座標が範囲内か </returns>
    public static bool IsInRange(Vector2Int source, Vector2Int min, Vector2Int max)
    {
        // 範囲内に丸めた座標を用意。
        var temp = source;
        temp.Clamp(min, max);
        // 「元の座標」と「範囲内に丸めた座標」が等しい場合、範囲内である。
        return source == temp;
    }
}
