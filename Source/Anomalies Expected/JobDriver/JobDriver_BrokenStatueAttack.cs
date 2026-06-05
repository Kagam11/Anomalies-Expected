using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace AnomaliesExpected
{
    public class JobDriver_BrokenStatueAttack : JobDriver
    {
        private const TargetIndex VictimInd = TargetIndex.A;
        private Pawn victim => (Pawn)base.TargetThingA;

        public HediffComp_BrokenStatue BrokenStatueComp => BrokenStatueCompCached ?? (BrokenStatueCompCached = pawn.health.hediffSet.GetHediffComps<HediffComp_BrokenStatue>().FirstOrDefault());
        private HediffComp_BrokenStatue BrokenStatueCompCached;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            if (pawn.mindState.enemyTarget != null)
            {
                job.SetTarget(VictimInd, pawn.mindState.enemyTarget);
            }
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(VictimInd);
            Toil stalk = Toils_Combat.FollowAndMeleeAttack(VictimInd, TargetIndex.A, delegate
            {
                Log.Message($"1");
                if (!pawn.stances.FullBodyBusy)
                {
                    BrokenStatueComp.BreakSpine(victim);
                    pawn.mindState.enemyTarget = null;
                    ReadyForNextToil();
                }
            });
            stalk.tickAction = delegate
            {
                if (pawn.IsHashIntervalTick(250))
                {
                    Pawn target = ThinkNode_JobGiver_BrokenStatueAttack.ScanForTarget(pawn);
                    if (target != null)
                    {
                        pawn.mindState.enemyTarget = target;
                        job.SetTarget(VictimInd, target);
                    }
                }
            };
            yield return stalk;
        }
    }
}
