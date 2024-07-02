using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CharacterScreenSelectable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterScreen CharacterScreen { get; set; }

    public abstract void OnClick(PointerEventData eventdata);
    public abstract void OnEnter(PointerEventData eventdata);
    public abstract void OnExit(PointerEventData eventdata);
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void Focus();
    public abstract void Unfocus();

    public void OnPointerClick(PointerEventData eventData) => OnClick(eventData);
    public void OnPointerEnter(PointerEventData eventData) => OnEnter(eventData);
    public void OnPointerExit(PointerEventData eventData) => OnExit(eventData);
}