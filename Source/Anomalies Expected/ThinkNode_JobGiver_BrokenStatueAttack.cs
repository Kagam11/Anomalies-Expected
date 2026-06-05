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
        private static HashSet<Pawn> tmpTargets = new HashSet<Pawn>();

        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn target = ScanForTarget(pawn);
            if (target == null)
            {
                CellFinder.TryFindRandomReachableNearbyCell(pawn.Position, pawn.Map, 30, TraverseParms.For(TraverseMode.PassDoors), (IntVec3 x) => x.Standable(pawn.Map), null, out var result);
                Job job = JobMaker.MakeJob(JobDefOf.GotoWander, result);
                job.locomotionUrgency = LocomotionUrgency.Walk;
                return job;
            }
            else
            {
                if (AEMod.Settings.BrokenStatueOnlyMelee)
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
            tmpTargets.Clear();
            TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors);
            RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(traverseParms, isDestination: true), delegate (Region x)
            {
                List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
                for (int i = 0; i < list.Count; i++)
                {
                    Pawn pawn2 = (Pawn)list[i];
                    if (ValidTarget(pawn2))
                    {
                        tmpTargets.Add(pawn2);
                    }
                }
                return false;
            });
            if (tmpTargets.NullOrEmpty())
            {
                return null;
            }
            return tmpTargets.First();
        }
        public static bool ValidTarget(Pawn pawn)
        {
            return pawn.RaceProps.Humanlike && pawn.Faction != Faction.OfEntities && !pawn.IsSubhuman && !pawn.DeadOrDowned;
        }
    }
}
