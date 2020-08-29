using UnityEngine;

/// <summary>
/// オセロの石ビュー
/// </summary>
public class DiskView : MonoBehaviour
{
    [SerializeField, Tooltip("オセロの石を表す3Dモデル")]
    private Transform disk = default;


    /// <summary>
    /// ひっくり返ります。
    /// </summary>
    /// <param name="isBlack"> 表面は黒か </param>
    public void Turn(bool isBlack)
    {
        if (isBlack) {
            disk.rotation = Quaternion.identity;
        }
        else {
            disk.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
    }
}
