using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Warning UIのために必要

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // 障害物のプレハブ
    public float spawnInterval = 2f;
    public GameObject warningUIPrefab; // Warning表示用のUIプレハブ
    public Transform canvasTransform; // UIを表示するCanvas
    public GameObject player;

    private float screenWidth;

    void Start()
    {
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ランダムな障害物を選択
            GameObject selectedObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // ランダムなX座標に生成
            float randomX = Random.Range(-screenWidth, screenWidth);
            Vector3 spawnPosition = new Vector3(randomX, player.transform.position.y - 10, 0);

            // Warning表示
            ShowWarning(randomX);
            yield return new WaitForSeconds(0.5f); // Warning表示から出現までの時間

            Instantiate(selectedObstacle, spawnPosition, Quaternion.identity);
        }
    }

    void ShowWarning(float positionX)
    {
        // ワールド座標をスクリーン座標に変換してUIを表示
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(positionX,-4.0f,0f));
        GameObject warning = Instantiate(warningUIPrefab, new Vector3(positionX, player.transform.position.y - 4.5f, 0), Quaternion.identity, canvasTransform);
        // 一定時間で消す
        Destroy(warning, 0.5f);
    }
}