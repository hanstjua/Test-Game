using Battle;
using Cutscene.Actions;
using System;
using System.Collections;
using UnityEngine;

class TalkAnimationExecutor : AnimationExecutor
{
    private readonly Talk _talk;

    public TalkAnimationExecutor(Talk talk, BattleProperties battleProperties) : base(battleProperties, new ActionOutcome[] {})
    {
        _talk = talk;
    }

    public override bool Execute()
    {
        var affiliation = _talk.Affiliation switch
        {
            Talk.SpeakerAffiliation.ally => SpeechBubble.Affiliation.Ally,
            Talk.SpeakerAffiliation.enemy => SpeechBubble.Affiliation.Enemy,
            Talk.SpeakerAffiliation.party => SpeechBubble.Affiliation.Party,
            _ => throw new ArgumentException($"Unhandled affiliation {_talk.Affiliation}")
        };

        var variant = _talk.Position switch
        {
            Talk.BubblePosition.bottom => SpeechBubble.Variant.Bottom,
            Talk.BubblePosition.top => SpeechBubble.Variant.Top,
            _ => throw new ArgumentException($"Unhandled speech bubble position {_talk.Position}")
        };

        var characterConfig = CharacterConfig.Get(_talk.Target);
        var portraitPath = $"{characterConfig.SpritesPath}/{characterConfig.Portrait}";
        var bubble = SpeechBubble.Create(variant, BattleProperties.uiObjects.transform.Find("CameraCanvas/RawImage"));
        bubble.AttachToCharacter(BattleProperties.characters[new AgentId(_talk.Target)].GetComponent<Character>());
        bubble.PlayText(_talk.Speech, 0, affiliation, Resources.Load<Sprite>(portraitPath));
        bubble.StartCoroutine(MonitorAnimation(bubble));
        IsAnimating = true;

        return true;
    }

    private IEnumerator MonitorAnimation(SpeechBubble bubble)
    {
        while (bubble.IsTyping)
        {
            yield return null;
        }

        yield return new WaitForSeconds((float)_talk.Duration);

        GameObject.Destroy(bubble.gameObject);

        IsAnimating = false;
    }
}