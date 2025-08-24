using UnityEngine;

public class ChasingFishController : MonoBehaviour
{
    public Transform lureTransform; // ���A�[��Transform
    public float followSpeed = 10f; // ���A�[�̍ō����x�Ɠ���

    void Update()
    {
        if (lureTransform == null) return;

        // ��Ɉ�葬�x�ŉ��Ɉړ�
        transform.position += Vector3.down * followSpeed * Time.deltaTime;

        // ���A�[�ɒǂ�������Q�[���I�[�o�[
        if (transform.position.y <= lureTransform.position.y)
        {
            // �Q�[���I�[�o�[�������Ăяo��
            FindFirstObjectByType<GameManager>().GameOver();
            Destroy(gameObject); // ��������
        }
    }
}