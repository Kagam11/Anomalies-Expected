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

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            if (BrokenStatue.DestroyedOrNull())
            {
                innerContainer.TryAdd(ThingMaker.MakeThing(Props.spawnedThingDef));
            }
            IntVec3 intVec3 = Pawn.PositionHeld;
            Map map = Pawn.MapHeld;
            ResurrectionUtility.TryResurrect(Pawn);
            Pawn.DeSpawn();
            BrokenStatueComp.GetDirectlyHeldThings().TryAdd(Pawn);
            if (!innerContainer.TryDrop(BrokenStatue, intVec3, map, ThingPlaceMode.Near, out var lastResultingThing))
            {
                if (!RCellFinder.TryFindRandomCellNearWith(intVec3, (IntVec3 c) => c.Standable(map), map, out var result, 1))
                {
                    Debug.LogError("Could not drop BrokenStatue!");
                }
                lastResultingThing = GenSpawn.Spawn(innerContainer.Take(BrokenStatue), result, map);
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && innerContainer.removeContentsIfDestroyed)
            {
                innerContainer.removeContentsIfDestroyed = false;
            }
        }
    }
}
