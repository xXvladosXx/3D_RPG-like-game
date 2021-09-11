using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform[] _characters;
    [SerializeField] private float _cameraSpeed;

    private Transform _currentCharacter;

    private void OnEnable()
    {
        _currentCharacter = _characters[0];
    }

    
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _currentCharacter.position, Time.deltaTime * _cameraSpeed);

        Vector3 angle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, _currentCharacter.transform.rotation.eulerAngles.x,
                Time.deltaTime * _cameraSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, _currentCharacter.transform.rotation.eulerAngles.y,
                Time.deltaTime * _cameraSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, _currentCharacter.transform.rotation.eulerAngles.z,
                Time.deltaTime * _cameraSpeed));

        transform.eulerAngles = angle;
    }
    
    public void SetCharacterView(Transform view)
    {
        _currentCharacter = view;
    }
}
