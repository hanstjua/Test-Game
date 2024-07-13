using Battle;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AvailableAction : CharacterScreenSelectable
{
    public Action Action { get; private set; }
    public bool IsMuted { get; private set; }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void Focus()
    {
        throw new System.NotImplementedException();
    }

    public override void Unfocus()
    {
        throw new System.NotImplementedException();
    }

    public override void OnClick(PointerEventData eventdata)
    {

    }

    public override void OnEnter(PointerEventData eventdata)
    {
        if (Action != null)
        {
            // TODO: Update desccription
            CharacterScreen.transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = Action.Description;

            GetComponent<Image>().color = CharacterScreen.ActionHighlightedColor;
            if (!IsMuted) transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ActionHighlightedFontColor;
        }
    }

    public override void OnExit(PointerEventData eventdata)
    {
        if (Action != null)
        {
            // TODO: Clear description
            CharacterScreen.transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = "";

            GetComponent<Image>().color = CharacterScreen.ActionNormalColor;
            if (!IsMuted) transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ActionNormalFontColor;
        }
    }

    public void SetAction(Action action, bool muted)
    {
        var name = transform.Find("Name").GetComponent<TMP_Text>();
        name.text = action.Type.Name;
        name.color = muted ? CharacterScreen.ActionMutedFontColor : CharacterScreen.ActionNormalFontColor;
        IsMuted = muted;

        Action = action;
    }

    public void ClearAction()
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = "";
        Action = null;
    }
}