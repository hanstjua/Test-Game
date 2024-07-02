using UnityEngine.EventSystems;

public class ArbellumOption : CharacterScreenSelectable
{
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
        // TODO: activate arbellum
    }

    public override void OnEnter(PointerEventData eventdata)
    {
        // TODO: Update description
        // TODO: Update available actions
    }

    public override void OnExit(PointerEventData eventdata)
    {
        // TODO: Clear description
        // TODO: Clear available actions
    }
}