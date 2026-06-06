using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AnomaliesExpected
{
    public class ThinkNode_JobGiver_BrokenStatueAttack : ThinkNode_JobGiver
    {
        private static Faction tmpFaction;

        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn target = RevenantUtility.ScanForTarget(pawn);
            HediffComp_BrokenStatue BrokenStatueComp = pawn.health.hediffSet.GetHediffComps<HediffComp_BrokenStatue>().FirstOrDefault();
            if (target == null)
            {
                BrokenStatueComp.Wander();
                //if (BrokenStatueComp.Wander())
                //{
                //    return null;
                //}
                //CellFinder.TryFindRandomReachableNearbyCell(pawn.Position, pawn.Map, 30, TraverseParms.For(TraverseMode.PassDoors), (IntVec3 x) => x.Standable(pawn.Map), null, out var result);
                //Job job = JobMaker.MakeJob(JobDefOf.GotoWander, result);
                //job.locomotionUrgency = LocomotionUrgency.Walk;
                return null;
            }
            else
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
