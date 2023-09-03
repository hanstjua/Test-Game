using System.Linq;
using Battle.Statuses;
using UnityEditor.Media;

namespace Battle.Services.Actions
{
    public class Defend : Action
    {
        public override AreaOfEffect AreaOfEffect => new AreaOfEffect(
            new Position[] {new Position(0, 0, 0)},
            0
        );
        public override string Name => "Defend";
        public override ActionType Type => ActionType.Defend;

        public ActionOutcome[] PreExecute(Agent actor, Battle battle, UnitOfWork unitOfWork)
        {
            ActionOutcome[] ret = {};

            var weaponEffects = actor.Weapon.ApplyPreExecutionInitiatorEffects(ret, actor, actor, battle, unitOfWork);
            var armourEffects = actor.Armour.ApplyPostExecutionInitiatorEffects(ret, actor, actor, battle, unitOfWork);

            return weaponEffects.Concat(armourEffects).ToArray();
        }

        public ActionEffect Execute(Agent actor)
        {
            if (actor.IsAlive())
            {
                return new AddStatus(actor.Id() as AgentId, new Guard(3));
            }
            else return null;
        }

        public ActionOutcome[] PostExecute(Agent actor, Battle battle, UnitOfWork unitOfWork)
        {
            ActionOutcome[] ret = {};

            var weaponEffects = actor.Weapon.ApplyPostExecutionInitiatorEffects(ret, actor, actor, battle, unitOfWork);
            var armourEffects = actor.Armour.ApplyPostExecutionInitiatorEffects(ret, actor, actor, battle, unitOfWork);

            return weaponEffects.Concat(armourEffects).ToArray();
        }
    }
}