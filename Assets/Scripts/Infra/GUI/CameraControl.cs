using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] public BattleEvents battleEvents;

    private const float TRANSLATION_SCALER = 30.0f;
    private const float ROTATION_INTERVAL = 0.2f;
    private const float ROTATION_DURATION = 0.2f;
    private static readonly KeyCode[] KEYS = {
        KeyCode.W, 
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.Q,
        KeyCode.E
    };

    [field: SerializeField] private GameObject _obj;

    private float _timestamp;
    private float _interval;
    private float _counter;
    private GameObject _pivot;


    public void FocusAt(Vector3 position)
    {
        if (!LeanTween.isTweening(gameObject))
        {
            Vector3[] positions = {
                new(-4.5f, 6f, -4.5f),
                new(-4.5f, 6f, 4.5f),
                new(4.5f, 6f, 4.5f),
                new(4.5f, 6f, -4.5f)
            };

            Vector3 newPosition = new(-1, -1, -1);
            var min = 181f;

            foreach (var p in positions)
            {
                RaycastHit hit;
                var noHit = !Physics.Raycast(position, p, out hit);
                if (noHit || hit.transform.GetComponent<Block>() == null)
                {
                    var v1 = new Vector2(transform.forward.x, transform.forward.z);
                    var v2 = new Vector2(p.x, p.z) * -1;
                    var angle = Vector2.SignedAngle(v1, v2);
                    if (Math.Abs(angle) < Math.Abs(min))
                    {
                        min = angle;
                        newPosition = p;
                    }
                }
            }

            if (newPosition != new Vector3(-1, -1, -1)) LeanTween.move(gameObject, position + newPosition, 0.4f).setEase(LeanTweenType.easeOutExpo);
            if (min < 181f) LeanTween.rotateAround(gameObject, Vector3.up, -min, 0.4f).setEase(LeanTweenType.easeOutExpo);
        }
    }

    public void FocusAtObject(GameObject obj)
    {
        FocusAt(obj.transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        _timestamp = 0;
        _interval = 0;

        _pivot = GameObject.Find("Pivot");
        _counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T) && _obj != null) { FocusAtObject(_obj); }
    }

    public void HandleCameraInput()
    {
        if ((Time.time - _timestamp) > _interval && !LeanTween.isTweening(gameObject))
        {
            _interval = 0;
            _counter = 0;

            foreach (var k in KEYS)
            {
                if (Input.GetKey(k)) OnKeyPress(k);
            }

            _timestamp = Time.time;
        }
    }

    float GetDeltaAngle(float i, float angle)
    {
        var ret = angle * (i - _counter);
        _counter = i;
        return ret;
    }

    void OnKeyPress(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
            var forward = transform.forward;
            transform.position += Time.deltaTime * TRANSLATION_SCALER * new Vector3(forward.x / Math.Abs(forward.x), 0, forward.z / Math.Abs(forward.z));
            break;

            case KeyCode.A:
            var left = transform.right * -1;
            transform.position += Time.deltaTime * TRANSLATION_SCALER * new Vector3(left.x / Math.Abs(left.x), 0, left.z / Math.Abs(left.z));
            break;

            case KeyCode.S:
            var backward = transform.forward * -1;
            transform.position += Time.deltaTime * TRANSLATION_SCALER * new Vector3(backward.x / Math.Abs(backward.x), 0, backward.z / Math.Abs(backward.z));
            break;

            case KeyCode.D:
            var right = transform.right;
            transform.position += Time.deltaTime * TRANSLATION_SCALER * new Vector3(right.x / Math.Abs(right.x), 0, right.z / Math.Abs(right.z));
            break;

            case KeyCode.Q:
            LeanTween.value(gameObject, i => transform.RotateAround(_pivot.transform.position, Vector3.up, GetDeltaAngle(i, 90f)), 0, 1, ROTATION_DURATION)
            .setEase(LeanTweenType.easeInOutExpo);
            _interval = ROTATION_INTERVAL;
            break;

            case KeyCode.E:
            LeanTween.value(gameObject, i => transform.RotateAround(_pivot.transform.position, Vector3.up, GetDeltaAngle(i, -90f)), 0, 1, ROTATION_DURATION)
            .setEase(LeanTweenType.easeInOutExpo);
            _interval = ROTATION_INTERVAL;
            break;
        }
    }
}
