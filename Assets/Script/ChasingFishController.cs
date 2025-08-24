using UnityEngine;

public class ChasingFishController : MonoBehaviour
{
    public Transform lureTransform; // ルアーのTransform
    public float followSpeed = 10f; // ルアーの最高速度と同じ

    void Update()
    {
        if (lureTransform == null) return;

        // 常に一定速度で下に移動
        transform.position += Vector3.down * followSpeed * Time.deltaTime;

        // ルアーに追いついたらゲームオーバー
        if (transform.position.y <= lureTransform.position.y)
        {
            // ゲームオーバー処理を呼び出す
            FindFirstObjectByType<GameManager>().GameOver();
            Destroy(gameObject); // 魚を消す
        }
    }
}