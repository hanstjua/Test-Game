using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    const float FRONT_ANGLE = 90f;

    private Transform _mainTransform;
    private Camera _mainCamera;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _mainTransform = transform.parent;
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = _mainCamera.transform.rotation;

        var cameraForward = new Vector3(_mainCamera.transform.forward.x, 0f, _mainCamera.transform.forward.z);

        var signedAngle = Vector3.SignedAngle(_mainTransform.forward, cameraForward, Vector3.up);

        var front = Math.Abs(signedAngle) <= FRONT_ANGLE ? -1.0f : 1.0f;

        _animator.SetFloat("front", front);

        _spriteRenderer.flipX = signedAngle < 0;
    }
}
