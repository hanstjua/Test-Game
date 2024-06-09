using Common;
using Battle;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static readonly Vector3 POSITION_OFFSET = new(0, 0.1f, 0);
    public AgentId agentId;
    public Map map;

    private BattleEvents _battleEvents;
    private UnitOfWork _unitOfWork;
    
    // Start is called before the first frame update
    void Start()
    {
        // if (agentId == null) throw new Exception("agentId has not been set.");

        // for demo purposes

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Character Create(AgentId id, Map map, string resourcePath, Vector3 position, BattleEvents battleEvents, UnitOfWork unitOfWork)
    {
        var obj = Instantiate((GameObject) Resources.Load(resourcePath), position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        var character = obj.GetComponent<Character>();
        character.agentId = id;
        character.map = map;
        character._battleEvents = battleEvents;
        character._unitOfWork = unitOfWork;

        character._battleEvents.characterMoved.AddListener((a, b) => character.SetPosition(a, b));

        return character;
    }

    public Character SetPosition(AgentId id, Position to)
    {
        if (id == agentId)
        {
            using (_unitOfWork)
            {
                var agent = _unitOfWork.AgentRepository.Get(agentId).Move(to);

                _unitOfWork.AgentRepository.Update(agentId, agent);

                _unitOfWork.Save();
            }

            transform.position = map.ToUIPosition(to);
        }

        return this;
    }

    public Character SetDirection(AgentId id, Direction towards)
    {
        if (id == agentId)
        {
            using (_unitOfWork)
            {
                var agent = _unitOfWork.AgentRepository.Get(agentId).Face(towards);

                _unitOfWork.AgentRepository.Update(agentId, agent);

                _unitOfWork.Save();
            }

            Vector2 direction = towards switch
            {
                Direction.North => new Vector2(0, 1),
                Direction.East => new Vector2(1,0),
                Direction.South => new Vector2(0, -1),
                Direction.West => new Vector2(-1,0)
            };

            transform.eulerAngles = new Vector3(0, (direction.x * 90) + (direction.y * (1 - direction.y) * 90), 0);
        }

        return this;
    }

    public void LoadSprites(string characterName)
    {
        var sprite = transform.Find("Sprite").GetComponent<CharacterSprite>();
        sprite.LoadSprites(characterName);
    }
}
