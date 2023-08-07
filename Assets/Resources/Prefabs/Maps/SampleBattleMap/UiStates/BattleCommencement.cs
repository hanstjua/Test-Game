using UnityEngine;
using TMPro;
using Battle;

public class BattleCommencement : IUiState
{
    private readonly float _period = 2;
    private float _elapsed = 0;
    private TMP_Text _commenceText;

    public BattleCommencement(GameObject uiObjects)
    {
        _commenceText = uiObjects.transform.Find("CameraCanvas/RawImage/CommenceText").GetComponent<TMP_Text>();
        _commenceText.gameObject.SetActive(true);
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        _elapsed += Time.deltaTime;
        var angle = 2 * Mathf.PI / _period * _elapsed;
        if (angle >= Mathf.PI)
        {
            _commenceText.gameObject.SetActive(false);

            return new TransitionBattlePhase();
        }
        else
        {
            _commenceText.alpha = Mathf.Sin(angle);
            return this;
        }
    }
}