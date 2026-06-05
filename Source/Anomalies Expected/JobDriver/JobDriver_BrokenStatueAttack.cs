using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AnomaliesExpected
{
    public class JobDriver_BrokenStatueAttack : JobDriver
    {
        private const TargetIndex VictimInd = TargetIndex.A;

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
            Toil stalk = Toils_Combat.FollowAndMeleeAttack(VictimInd, TargetIndex.B, delegate
            {
                if (!pawn.stances.FullBodyBusy)
                {
                    Pawn pawn4 = (Pawn)job.GetTarget(VictimInd).Thing;
                    JobDriver curDriver = pawn.jobs.curDriver;
                    pawn4.TakeDamage(new DamageInfo(DamageDefOf.Blunt, 20 * AEMod.Settings.BrokenStatueSpineDmgMult, 0.1f * AEMod.Settings.BrokenStatueSpineDmgMult, instigator: pawn, hitPart: pawn4.health.hediffSet.GetBodyPartRecord(BodyPartDefOfLocal.Spine)));
                    curDriver.ReadyForNextToil();
                }
            });
            Toil toil = stalk;
            toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
            {
                pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
            });
            yield return stalk;
        }
    }
}
