using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AnomaliesExpected
{
    public class JobDriver_BrokenStatueAttack : JobDriver
    {
        private const TargetIndex VictimInd = TargetIndex.A;

        //private const float NewTargetScanRadius = 20f;

        //private const int SmearMTBTicks = 60;

        //private const int CheckForCloserTargetMTB = 180;

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
                    pawn4.TakeDamage(new DamageInfo(DamageDefOf.Blunt, 100, 100, instigator: pawn, hitPart: pawn4.health.hediffSet.GetBodyPartRecord(BodyPartDefOfLocal.Spine)));
                    curDriver.ReadyForNextToil();
                }
            });
            Toil toil = stalk;
            toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
            {
                pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
            });
            //stalk.AddFinishAction(delegate
            //{
            //    Pawn pawn2 = job.GetTarget(VictimInd).Pawn;
            //    if (pawn2 == null || !pawn2.Spawned || pawn2.Map != pawn.Map)
            //    {
            //        if (pawn2 != null)
            //        {
            //            Find.Anomaly.EndHypnotize(pawn2);
            //        }
            //        job.SetTarget(VictimInd, RevenantUtility.GetClosestTargetInRadius(pawn, NewTargetScanRadius));
            //        pawn.mindState.enemyTarget = job.GetTarget(VictimInd).Pawn;
            //        if (pawn.mindState.enemyTarget == null)
            //        {
            //            Comp.revenantState = RevenantState.Wander;
            //        }
            //    }
            //});
            yield return stalk;
            //Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
            //toil3.FailOnCannotTouch(VictimInd, PathEndMode.Touch);
            //toil3.initAction = delegate
            //{
            //    hypnotizeEndTick = Find.TickManager.TicksGame + HypnotizeDurationTicks;
            //};
            //toil3.AddFinishAction(delegate
            //{
            //    Pawn pawn = job.GetTarget(VictimInd).Pawn;
            //    Find.Anomaly.EndHypnotize(pawn);
            //    base.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
            //});
            //toil3.tickAction = (Action)Delegate.Combine(toil3.tickAction, (Action)delegate
            //{
            //    job.GetTarget(VictimInd).Pawn.rotationTracker.FaceTarget(pawn);
            //    if (pawn.Drawer.renderer.CurAnimation != AnimationDefOf.RevenantHypnotise)
            //    {
            //        pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantHypnotise);
            //    }
            //    if (!pawn.stances.stunner.Stunned && Find.TickManager.TicksGame >= hypnotizeEndTick)
            //    {
            //        Pawn victim = job.GetTarget(VictimInd).Pawn;
            //        Comp.Hypnotize(victim);
            //        if (Comp.injuredWhileAttacking)
            //        {
            //            Thing thing = ThingMaker.MakeThing(ThingDefOf.RevenantFleshChunk);
            //            thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = Comp.biosignature;
            //            Thing thing2 = GenSpawn.Spawn(thing, pawn.PositionHeld, pawn.Map);
            //            Find.LetterStack.ReceiveLetter("LetterRevenantFleshChunkLabel".Translate(), "LetterRevenantFleshChunkText".Translate(), LetterDefOf.NeutralEvent, thing2, null, null, null, null, 600);
            //            Comp.injuredWhileAttacking = false;
            //        }
            //        Comp.revenantState = RevenantState.Escape;
            //        EndJobWith(JobCondition.Succeeded);
            //    }
            //});
            //toil3.defaultCompleteMode = ToilCompleteMode.Never;
            //toil3.PlaySustainerOrSound(SoundDefOf.Pawn_Revenant_Hypnotize);
            //yield return toil3;
        }
    }
}
