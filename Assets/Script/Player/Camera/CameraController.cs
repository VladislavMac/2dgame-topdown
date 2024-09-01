using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private Vector3 _offset = new Vector3(0, 0, -1);

    private float _cooldownStart = 0.5f;
    private float _cooldown;

    private bool _agressiveMode = false;

    private void Start()
    {
        _cooldown = _cooldownStart;        
    }

    private void LateUpdate()
    {
        Vector3 temp = _player.transform.position + _offset;

        if (_cooldown <= 0 && _agressiveMode)
        {
            temp += new Vector3(Random.Range(0, 10), Random.Range(0, 10), 0);
            _cooldown = _cooldownStart;
        }
        else
        {
            _cooldown -= Time.deltaTime;
        }

        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, temp, 1f);
        // Обновление позиции камеры
        transform.position = temp;
    }
}
