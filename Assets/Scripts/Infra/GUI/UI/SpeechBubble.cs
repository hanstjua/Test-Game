using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public enum Variant
    {
        Top,
        Bottom
    };

    public enum Affiliation
    {
        Party,
        Ally,
        Enemy
    };

    [field: SerializeField] private Color _partyColor;
    [field: SerializeField] private Color _allyColor;
    [field: SerializeField] private Color _enemyColor;

    private bool _isTyping = false;
    private double _interval = 0;
    private int _index = 0;
    private string _text = "";
    private string _colorString = "";
    private TMP_Text _textComponent;
    private Image[] _images;
    private Image _portrait;
    private Character _character;
    private Camera _camera;
    private RenderTexture _renderTexture;

    public bool IsTyping { get => _isTyping; }

    public static SpeechBubble Create(Variant variant, Transform parent = null)
    {
        var obj = parent == null ?
        Instantiate(Resources.Load<GameObject>($"Prefabs/Maps/SpeechBubble{variant}")) :
        Instantiate(Resources.Load<GameObject>($"Prefabs/Maps/SpeechBubble{variant}"), parent);
        obj.transform.position = new(-10,-10,0);

        var ret = obj.GetComponent<SpeechBubble>();
        ret.Init();

        return ret;
    }

    private void Init()
    {
        _textComponent = GetComponentInChildren<TMP_Text>();
        _images = new Image[]
        {
            transform.Find("Bubble").GetComponent<Image>(),
            transform.Find("Pointer").GetComponent<Image>()
        };
        _portrait = transform.Find("Bubble/Portrait").GetComponent<Image>();

        SetCamera(Camera.main);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_character != null)
        {
            var screenPoint = _camera.WorldToScreenPoint(_character.transform.position);
            var screenPosition = new Vector3(
                screenPoint.x * Screen.width / _renderTexture.width,
                screenPoint.y * Screen.height / _renderTexture.height,
                0
            );
            GetComponent<RectTransform>().anchoredPosition3D = screenPosition;
        }
    }

    IEnumerator Typing()
    {
        while (_index < _text.Length)
        {
            _textComponent.text = _text.Insert(_index++, _colorString);
            yield return new WaitForSeconds((float)_interval);
        }
        _textComponent.text = _text;
        _isTyping = false;
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
        _renderTexture = _camera.targetTexture;
    }

    public void AttachToCharacter(Character character)
    {
        _character = character;
    }

    public void PlayText(string text, double interval, Affiliation affiliation, Sprite portrait)
    {
        var color = affiliation switch
        {
            Affiliation.Ally => _allyColor,
            Affiliation.Enemy => _enemyColor,
            Affiliation.Party => _partyColor,
            _ => throw new NotImplementedException()
        };

        _colorString = $"<#{ColorUtility.ToHtmlStringRGB(color)}>";

        foreach (var image in _images)
        {
            image.color = color;
        }

        _portrait.sprite = portrait;

        _text = text;
        _interval = Math.Max(0, interval);
        _index = 0;
        _isTyping = true;

        StartCoroutine(Typing());
    }
}
