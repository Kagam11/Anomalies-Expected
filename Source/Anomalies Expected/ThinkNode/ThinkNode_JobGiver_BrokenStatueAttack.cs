using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace AnomaliesExpected
{
    public class ThinkNode_JobGiver_BrokenStatueAttack : ThinkNode_JobGiver
    {
        private static Faction tmpFaction;

        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn target = ScanForTarget(pawn);
            HediffComp_BrokenStatue BrokenStatueComp = pawn.health.hediffSet.GetHediffComps<HediffComp_BrokenStatue>().FirstOrDefault();
            if (target != null)
            {
                BrokenStatueComp.Wander(true);
                pawn.mindState.enemyTarget = target;
                if (AEMod.Settings.BrokenStatueDisableSpineBreaker)
                {
                    return JobMaker.MakeJob(JobDefOf.AttackMelee, target);
                }
                else
                {
                    return JobMaker.MakeJob(JobDefOfLocal.AE_BrokenStatueAttack, target);
                }
            }
            return null;
        }

        public static Pawn ScanForTarget(Pawn pawn, bool forced = false)
        {
            Pawn target = null;
            float dist = float.PositiveInfinity;
            tmpFaction = pawn.Faction;
            TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors);
            RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(traverseParms, isDestination: true), delegate (Region x)
            {
                List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
                for (int i = 0; i < list.Count; i++)
                {
                    Pawn pawn2 = (Pawn)list[i];
                    if (ValidTarget(pawn2))
                    {
                        float d = pawn.PositionHeld.DistanceTo(pawn2.PositionHeld);
                        if (d < dist)
                        {
                            dist = d;
                            target = pawn2;
                        }
                    }
                }
                return false;
            });
            return target;
        }

        public static bool ValidTarget(Pawn target)
        {
            return target.RaceProps.Humanlike && tmpFaction.HostileTo(target.Faction) && !target.IsSubhuman && !target.DeadOrDowned;
        }
    }
}
