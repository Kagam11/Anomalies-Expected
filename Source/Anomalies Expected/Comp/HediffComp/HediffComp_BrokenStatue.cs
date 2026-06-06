using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AnomaliesExpected
{
    public class HediffComp_BrokenStatue : HediffComp_ObservingStage, IThingHolder
    {
        public HediffCompProperties_BrokenStatue Props => (HediffCompProperties_BrokenStatue)props;

        private ThingOwner<Thing> innerContainer;
        public Thing BrokenStatue => innerContainer.InnerListForReading.FirstOrDefault() as ThingWithComps;
        public Comp_BrokenStatue BrokenStatueComp => BrokenStatueCompCached ?? (BrokenStatueCompCached = BrokenStatue.TryGetComp<Comp_BrokenStatue>());
        private Comp_BrokenStatue BrokenStatueCompCached;

        public int countWandering;

        IThingHolder IThingHolder.ParentHolder => Pawn;

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public HediffComp_BrokenStatue()
        {
            innerContainer = new ThingOwner<Thing>(this, LookMode.Deep, removeContentsIfDestroyed: false);
        }

        public DamageWorker.DamageResult BreakSpine(Pawn target)
        {
            return target.TakeDamage(new DamageInfo(Props.damageDef, Props.spineDMG, Props.spinePenetration, instigator: Pawn, hitPart: target.health.hediffSet.GetBodyPartRecord(BodyPartDefOfLocal.Spine)));
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            Transform();
        }

        public void Transform()
        {
            if (BrokenStatue.DestroyedOrNull())
            {
                innerContainer.TryAdd(ThingMaker.MakeThing(Props.spawnedThingDef));
            }
            CompActivity ActivityComp = BrokenStatue.TryGetComp<CompActivity>();
            ActivityComp.EnterPassiveState();
            IntVec3 intVec3 = Pawn.PositionHeld;
            Map map = Pawn.MapHeld;
            if (Pawn.Dead)
            {
                ResurrectionUtility.TryResurrect(Pawn);
            }
            Pawn.DeSpawn();
            BrokenStatueComp.GetDirectlyHeldThings().TryAdd(Pawn);
            if (!innerContainer.TryDrop(BrokenStatue, intVec3, map, ThingPlaceMode.Near, out var lastResultingThing))
            {
                if (!RCellFinder.TryFindRandomCellNearWith(intVec3, (IntVec3 c) => c.Standable(map), map, out var result, 1))
                {
                    Debug.LogError("Could not drop BrokenStatue pile!");
                }
                lastResultingThing = GenSpawn.Spawn(innerContainer.Take(BrokenStatue), result, map);
            }
        }

        public bool Wander(bool isFoundTarget = false)
        {
            if (isFoundTarget)
            {
                countWandering = 0;
                return false;
            }
            countWandering++;
            if (countWandering > 10)
            {
                countWandering = 0;
                Transform();
                return true;
            }
            return false;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && innerContainer.removeContentsIfDestroyed)
            {
                innerContainer.removeContentsIfDestroyed = false;
            }
            Scribe_Values.Look(ref countWandering, "countWandering", 0);
        }
    }
}
