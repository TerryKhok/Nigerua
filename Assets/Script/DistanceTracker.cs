using TMPro;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = _player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.y < transform.position.y)
        {
            transform.position = new Vector3(0, _player.transform.position.y, 0);
        }
        _text.text = Mathf.FloorToInt(transform.position.y).ToString();
    }
}
