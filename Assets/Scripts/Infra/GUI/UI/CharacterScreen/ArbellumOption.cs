using Battle;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable enable

public class ArbellumOption : CharacterScreenSelectable
{
    public Arbellum? Arbellum { get; private set; }
    public bool IsToggled { get; private set; }

    private bool _isHighlighted = false;

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
        _isHighlighted = true;
        if (!IsToggled)
        {
            GetComponent<Image>().color = CharacterScreen.ArbellumOptionHighlightedColor;
            transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ArbellumOptionHighlightedFontColor;
        }
    }

    public override void Unfocus()
    {
        _isHighlighted = false;
        if (!IsToggled)
        {
            GetComponent<Image>().color = CharacterScreen.ArbellumOptionNormalColor;
            transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ArbellumOptionNormalFontColor;
        }
    }

    public override void OnClick(PointerEventData eventdata)
    {
        if (Arbellum != null) Toggle();
    }

    public override void OnEnter(PointerEventData eventdata)
    {
        if (Arbellum != null)
        {
            // TODO: Update description
            CharacterScreen.transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = Arbellum.Description;

            // TODO: Update available actions
            CharacterScreen.SetAvailableActions(0, Arbellum);

            CharacterScreen.FocusArbellumOption(this);
        }

    }

    public override void OnExit(PointerEventData eventdata)
    {
        if (Arbellum != null)
        {
            // TODO: Clear description
            CharacterScreen.transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = "";
        }
    }

    public void SetArbellum(Arbellum arbellum)
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = arbellum.Type.Name;

        if (arbellum.IsActive)
        {
            GetComponent<Image>().color = CharacterScreen.ArbellumOptionToggledColor;
            transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ArbellumOptionToggledFontColor;

            IsToggled = true;
        }

        Arbellum = arbellum;
    }

    public void ClearArbellum()
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = "";
        Arbellum = null;
    }

    public void Toggle()
    {
        IsToggled = !IsToggled;

        if (IsToggled)
        {
            GetComponent<Image>().color = CharacterScreen.ArbellumOptionToggledColor;
            transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ArbellumOptionToggledFontColor;

            using var uow = CharacterScreen.UnitOfWork!;
            var agent = CharacterScreen.Character!.ActivateArbellum(Arbellum!.Type);
            uow.AgentRepository.Update((AgentId)agent.Id(), agent);
        }
        else
        {
            if (_isHighlighted)
            {
                GetComponent<Image>().color = CharacterScreen.ArbellumOptionHighlightedColor;
                transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ArbellumOptionHighlightedFontColor;
            }
            else
            {
                GetComponent<Image>().color = CharacterScreen.ArbellumOptionNormalColor;
                transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.ArbellumOptionNormalFontColor;
            }

            using var uow = CharacterScreen.UnitOfWork!;
            var agent = CharacterScreen.Character!.DeactivateArbellum(Arbellum!.Type);
            uow.AgentRepository.Update((AgentId)agent.Id(), agent);
        }

        CharacterScreen.UpdateActiveArbella();
    }
}