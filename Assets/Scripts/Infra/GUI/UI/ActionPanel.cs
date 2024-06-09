using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Battle;
using Battle.Services.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ActionPanel : MonoBehaviour
{
    public UnityEvent<Battle.Action> ActionSelected;
    
    private const float INPUT_INTERVAL_S = 0.2f;
    private ActionButton[] _actionButtons;
    private (ArbellumType, (Battle.Action, bool)[][])[] _data;
    private TMP_Text _categoryName;
    private TMP_Text _infoDescription;
    private int _categoryIndex;
    private int _pageIndex;
    private Transform _actionScrollBar;
    private float _defaultScrollBarHeight;
    private Transform _actions;

    private float _timer;

    void Awake()
    {
        ActionSelected = new();

        var actions = transform.Find("Actions");
        _actionButtons = Enumerable.Range(0, actions.childCount - 1)
        .Select(i => actions.GetChild(i).GetComponent<ActionButton>())
        .ToArray();

        _infoDescription = transform.Find("Info/Description").GetComponent<TMP_Text>();

        _categoryName = transform.Find("Category/Name").GetComponent<TMP_Text>();

        _actions = transform.Find("Actions");
        _actionScrollBar = _actions.Find("ActionScrollBar");
        _defaultScrollBarHeight = _actionScrollBar.GetComponent<RectTransform>().sizeDelta.y;

        _timer = INPUT_INTERVAL_S;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in _actionButtons)
        {
            button.Focused.AddListener(action => _infoDescription.text = action.Description);
            button.Unfocused.AddListener(action => _infoDescription.text = "");
            button.Selected.AddListener(action => ActionSelected.Invoke(action));
        }

        UpdateActions(new Dictionary<Battle.Action, bool>{
            {new Attack(), true},
            {new Defend(), false},
            {new UseItem(), true},
            {new Fire(), true},
            {new Ice(), false},
            {new Wind(), true},
            {new Earth(), false},
            {new Thunder(), true},
            {new Water(), false}
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;

            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            NextPage();
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            PreviousCategory();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            NextCategory();
        }
        else
        {
            return;
        }

        _timer = INPUT_INTERVAL_S;
    }

    public void UpdateActions(Dictionary<Battle.Action, bool> actions)
    {
        Debug.Log(name);
        var skills = new HashSet<ArbellumType>(actions.Keys.Select(a => a.Arbellum));

        _data = skills
        .OrderByDescending(skill => skill == ArbellumType.Physical)  // Physical first
        .ThenByDescending(skill => skill != ArbellumType.Supplies)  // Item last
        .ThenBy(skill => skill.Name)
        .Select(
            s => (
                s,
                actions
                .Where(data => data.Key.Arbellum == s)
                .Select(data => (data.Key, data.Value))
                .Select((d, i) => new { Index = i, Value = d})
                .GroupBy(x => x.Index / _actionButtons.Length)
                .Select(x => x.Select(y => y.Value).ToArray())
                .ToArray()
            )
        )
        .ToArray();

        SetCategory(0);
    }

    private void SetCategory(int index)
    {
        if (index >= _data.Length || index < 0)
        {
            Debug.Log(String.Format("Category ndex {0} is out of range (0,{1})!", index, _data.Length));
            return;
        }

        _categoryIndex = index;
        _categoryName.text = _data[_categoryIndex].Item1.Name;

        SetPage(0);

        // move to ActionScrollBar
        var x = _actionScrollBar.GetComponent<RectTransform>().sizeDelta.x;
        _actionScrollBar.GetComponent<RectTransform>().sizeDelta = _data[_categoryIndex].Item2.Length > 1 ? new(x, _defaultScrollBarHeight) : new(x, 0f);

        _actionScrollBar.GetComponent<ActionScrollBar>().SetSections(_data[_categoryIndex].Item2.Length);
    }

    private void SetPage(int index)
    {
        if (index >= _data[_categoryIndex].Item2.Length || index < 0)
        {
            Debug.Log(String.Format("Page index {0} is out of range (0,{1})!", index, _data[_categoryIndex].Item2.Length));
            return;
        }

        _pageIndex = index;
        var page = _data[_categoryIndex].Item2[_pageIndex];

        for (int i = 0; i < _actionButtons.Length; i++)
        {
            if (i < page.Length)
            {
                _actionButtons[i].Enable(page[i].Item1);

                if (!page[i].Item2)
                {
                    _actionButtons[i].Disable();
                }
            }
            else
            {
                _actionButtons[i].SetNoAction();
            }
        }
    }

    public void NextCategory()
    {
        SetCategory((_categoryIndex + 1) % _data.Length);
    }

    public void PreviousCategory()
    {
        var category = _data[_categoryIndex].Item2;
        SetCategory((_categoryIndex - category.Length) % category.Length + (category.Length - 1));
    }

    public void NextPage()
    {
        SetPage((_pageIndex + 1) % _data[_categoryIndex].Item2.Length);
        _actionScrollBar.GetComponent<ActionScrollBar>().NextSection();
    }

    public void PreviousPage()
    {
        var page = _data[_categoryIndex].Item2;
        SetPage((_pageIndex - page.Length) % page.Length + (page.Length - 1));
        _actionScrollBar.GetComponent<ActionScrollBar>().PreviousSection();
    }
}
