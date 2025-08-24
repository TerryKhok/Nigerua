using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Warning UI�̂��߂ɕK�v

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // ��Q���̃v���n�u
    public float spawnInterval = 2f;
    public GameObject warningUIPrefab; // Warning�\���p��UI�v���n�u
    public Transform canvasTransform; // UI��\������Canvas
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

            // �����_���ȏ�Q����I��
            GameObject selectedObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // �����_����X���W�ɐ���
            float randomX = Random.Range(-screenWidth, screenWidth);
            Vector3 spawnPosition = new Vector3(randomX, player.transform.position.y - 10, 0);

            // Warning�\��
            ShowWarning(randomX);
            yield return new WaitForSeconds(0.5f); // Warning�\������o���܂ł̎���

            Instantiate(selectedObstacle, spawnPosition, Quaternion.identity);
        }
    }

    void ShowWarning(float positionX)
    {
        // ���[���h���W���X�N���[�����W�ɕϊ�����UI��\��
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(positionX,-4.0f,0f));
        GameObject warning = Instantiate(warningUIPrefab, new Vector3(positionX, player.transform.position.y - 4.5f, 0), Quaternion.identity, canvasTransform);
        // ��莞�Ԃŏ���
        Destroy(warning, 0.5f);
    }
}