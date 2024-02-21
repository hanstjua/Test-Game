using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{
    const float FRONT_ANGLE = 90f;

    public List<SpriteTimestamp> frontSprites;
    public List<SpriteTimestamp> backSprites;

    private Transform _mainTransform;
    private Camera _mainCamera;
    private SpriteRenderer _spriteRenderer;
    private float _signedAngle;

    // Start is called before the first frame update
    void Start()
    {
        _mainTransform = transform.parent;
        _mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        LeanTween.value(gameObject, UpdateSprite, 0, 1, 0.8f).setRepeat(-1);
    }

    public void LoadSprites(string characterName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Sprites/{characterName}/walking-sheet");
        frontSprites = new List<SpriteTimestamp> {
            new() {sprite=sprites[0], timestamp=0},
            new() {sprite=sprites[2], timestamp=0.25f},
            new() {sprite=sprites[4], timestamp=0.5f},
            new() {sprite=sprites[2], timestamp=0.75f}
        };
        backSprites = new List<SpriteTimestamp> {
            new() {sprite=sprites[5], timestamp=0},
            new() {sprite=sprites[7], timestamp=0.25f},
            new() {sprite=sprites[9], timestamp=0.5f},
            new() {sprite=sprites[7], timestamp=0.75f}
        };
    }

    private void UpdateSprite(float value)
    {
        transform.rotation = _mainCamera.transform.rotation;

        var cameraForward = new Vector3(_mainCamera.transform.forward.x, 0f, _mainCamera.transform.forward.z);

        _signedAngle = Vector3.SignedAngle(_mainTransform.forward, cameraForward, Vector3.up);

        var _front = Math.Abs(_signedAngle) > FRONT_ANGLE;

        var spriteTimestamps = _front ? frontSprites : backSprites;

        Sprite sprite = null;
        foreach (var spriteTimestamp in spriteTimestamps)
        {
            if (spriteTimestamp.timestamp > value) break;
            sprite = spriteTimestamp.sprite;
        }
        _spriteRenderer.sprite = sprite;

        _spriteRenderer.flipX = _signedAngle < 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
