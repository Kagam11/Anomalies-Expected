using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AnomaliesExpected
{
    public class Comp_BrokenStatue : CompInteractable, IThingHolder
    {
        public new CompProperties_BrokenStatue Props => (CompProperties_BrokenStatue)props;

        private ThingOwner<Thing> innerContainer;
        public Pawn BrokenStatue => innerContainer.InnerListForReading.FirstOrDefault() as Pawn;
        public HediffComp_BrokenStatue BrokenStatueComp => BrokenStatueCompCached ?? (BrokenStatueCompCached = BrokenStatue.health.hediffSet.GetHediffComps<HediffComp_BrokenStatue>().FirstOrDefault());
        private HediffComp_BrokenStatue BrokenStatueCompCached;

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public Comp_BrokenStatue()
        {
            innerContainer = new ThingOwner<Thing>(this, LookMode.Deep, removeContentsIfDestroyed: false);
        }

        protected CompAEStudyUnlocks StudyUnlocks => studyUnlocksCached ?? (studyUnlocksCached = parent.TryGetComp<CompAEStudyUnlocks>());
        private CompAEStudyUnlocks studyUnlocksCached;

        protected CompStudiable Studiable => studiableCached ?? (studiableCached = parent.TryGetComp<CompStudiable>());
        private CompStudiable studiableCached;

        public override bool HideInteraction => (StudyUnlocks?.NextIndex ?? 4) < 4;

        //public override void PostPostMake()
        //{
        //    base.PostPostMake();
        //    TickNextState = Find.TickManager.TicksGame + Props.beamIntervalRange.max;
        //}

        //public override void PostSpawnSetup(bool respawningAfterLoad)
        //{
        //    base.PostSpawnSetup(respawningAfterLoad);
        //    if (!respawningAfterLoad)
        //    {
        //        if (beamTargetState == BeamTargetState.Activating)
        //        {
        //            TickNextState = Find.TickManager.TicksGame + Math.Max(TickNextState - Find.TickManager.TicksGame + Props.ticksWhenCarried, Props.ticksWhenCarried);
        //        }
        //    }
        //}

        //public override void CompTick()
        //{
        //    base.CompTick();
        //    if (Find.TickManager.TicksGame >= TickNextState && parent.Spawned)
        //    {
        //        switch (beamTargetState)
        //        {
        //            case BeamTargetState.Searching:
        //                {
        //                    beamTargetState = BeamTargetState.Activating;
        //                    SkipToRandom();
        //                    TickNextState = Find.TickManager.TicksGame + Props.ticksPerBeamActivationPreparation;
        //                    break;
        //                }
        //            case BeamTargetState.Activating:
        //                {
        //                    beamTargetState = BeamTargetState.Searching;
        //                    SpawnBeams();
        //                    TickNextState = Find.TickManager.TicksGame + Props.beamIntervalRange.RandomInRange;
        //                    break;
        //                }
        //        }
        //    }
        //    if (studyMustBeEnabled && !Studiable.studyEnabled && TickUnlockStudy > -1 && Find.TickManager.TicksGame >= TickUnlockStudy)
        //    {
        //        Studiable.SetStudyEnabled(studyMustBeEnabled);
        //        studyMustBeEnabled = false;
        //        TickUnlockStudy = -1;
        //    }
        //}

        //public void ManualActivation()
        //{
        //    if (sendLocation == IntVec3.Invalid || !sendLocation.InBounds(parent.Map) || sendLocation.Fogged(parent.Map))
        //    {
        //        sendLocation = parent.Position;
        //    }
        //    TargetInfo targetInfoFrom = new TargetInfo(parent.Position, parent.Map);
        //    SoundDefOfLocal.Psycast_Skip_Exit.PlayOneShot(targetInfoFrom);
        //    FleckMaker.Static(targetInfoFrom.Cell, targetInfoFrom.Map, FleckDefOf.PsycastSkipInnerExit, Props.teleportationFleckRadius);
        //    TargetInfo targetInfoTo = new TargetInfo(sendLocation, parent.Map);
        //    SoundDefOf.Psycast_Skip_Entry.PlayOneShot(targetInfoTo);
        //    FleckMaker.Static(targetInfoTo.Cell, targetInfoFrom.Map, FleckDefOf.PsycastSkipFlashEntry, Props.teleportationFleckRadius);
        //    parent.Position = sendLocation;
        //    beamTargetState = BeamTargetState.Activating;
        //    TickNextState = Find.TickManager.TicksGame + Props.ticksWhenCarried;
        //}

        //public void SpawnBeams()
        //{
        //    SpawnBeam(parent.Position);
        //    for (int i = 1; i < beamNextCount; i++)
        //    {
        //        if (CellFinder.TryFindRandomCellNear(parent.Position, parent.Map, Props.beamSubRadius, delegate (IntVec3 newLoc)
        //        {
        //            return true;
        //        }, out var result))
        //        {
        //            SpawnBeam(result);
        //        }
        //        else
        //        {
        //            SpawnBeam(parent.Position);
        //        }
        //    }
        //    beamNextCount = Rand.RangeInclusive(1, beamMaxCount);
        //    if (beamNextCount == beamMaxCount && beamNextCount < Props.beamMaxCount)
        //    {
        //        beamMaxCount = Mathf.Min(beamMaxCount + 1, Props.beamMaxCount);
        //    }
        //    if (studyMustBeEnabled)
        //    {
        //        TickUnlockStudy = Find.TickManager.TicksGame + Props.beamDuration;
        //    }
        //}

        //public void SpawnBeam(IntVec3 position)
        //{
        //    PowerBeam obj = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, position, parent.Map);
        //    obj.duration = Props.beamDuration;
        //    obj.instigator = parent;
        //    obj.weaponDef = parent.def;
        //    obj.StartStrike();
        //}

        //public void SkipToRandom()
        //{
        //    Map map = parent.Map;
        //    IntVec3 result = IntVec3.Invalid;
        //    if (CellFinder.TryFindRandomCell(map, delegate (IntVec3 newLoc)
        //    {
        //        return newLoc.Walkable(map) && !newLoc.Fogged(map) && newLoc.GetFirstPawn(map) == null && newLoc.GetRoom(map) != parent.Position.GetRoom(map);
        //    }, out result) || CellFinder.TryFindRandomCell(map, delegate (IntVec3 newLoc)
        //    {
        //        return newLoc.Walkable(map) && !newLoc.Fogged(map) && newLoc.GetFirstPawn(map) == null;
        //    }, out result))
        //    {
        //        TargetInfo targetInfoFrom = new TargetInfo(parent.Position, parent.Map);
        //        SoundDefOfLocal.Psycast_Skip_Exit.PlayOneShot(targetInfoFrom);
        //        FleckMaker.Static(targetInfoFrom.Cell, targetInfoFrom.Map, FleckDefOf.PsycastSkipInnerExit, Props.teleportationFleckRadius);
        //        TargetInfo targetInfoTo = new TargetInfo(result, parent.Map);
        //        SoundDefOf.Psycast_Skip_Entry.PlayOneShot(targetInfoTo);
        //        FleckMaker.Static(targetInfoTo.Cell, targetInfoFrom.Map, FleckDefOf.PsycastSkipFlashEntry, Props.teleportationFleckRadius);
        //        Find.TickManager.slower.SignalForceNormalSpeedShort();
        //        if (AEMod.Settings.BeamTargetLetter)
        //        {
        //            Find.LetterStack.ReceiveLetter("AnomaliesExpected.BeamTarget.LeftContainment".Translate(parent.LabelCap).RawText, "AnomaliesExpected.BeamTarget.LeftContainmentText".Translate(parent.LabelCap), LetterDefOf.ThreatSmall, targetInfoFrom);
        //        }
        //        else
        //        {
        //            Messages.Message("AnomaliesExpected.BeamTarget.LeftContainment".Translate(parent.LabelCap).RawText, targetInfoFrom, MessageTypeDefOf.NegativeEvent);
        //        }
        //        parent.Position = result;
        //    }
        //    if (Studiable.studyEnabled)
        //    {
        //        Studiable.SetStudyEnabled(false);
        //        studyMustBeEnabled = true;
        //        TickUnlockStudy = -1;
        //    }
        //}

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                if (gizmo is Command_Action command_Action)
                {
                    command_Action.hotKey = KeyBindingDefOf.Misc6;
                }
                yield return gizmo;
            }
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    action = delegate
                    {
                        Transform();
                    },
                    defaultLabel = "Dev: Transform",
                    defaultDesc = "Transform into Brocken Statue"
                };
            }
        }

        //public override void OrderForceTarget(LocalTargetInfo target)
        //{
        //    if (ValidateTarget(target, showMessages: false))
        //    {
        //        TargetLocation(target.Pawn);
        //    }
        //}

        //private void TargetLocation(Pawn caster)
        //{
        //    TargetingParameters targetingParameters = TargetingParameters.ForCell();
        //    targetingParameters.mapBoundsContractedBy = 1;
        //    targetingParameters.validator = (TargetInfo c) => c.Cell.InBounds(caster.Map) && !c.Cell.Fogged(caster.Map);
        //    Find.Targeter.BeginTargeting(targetingParameters, delegate (LocalTargetInfo target)
        //    {
        //        sendLocation = target.Cell;
        //        base.OrderForceTarget(caster);
        //    }, delegate
        //    {
        //        Widgets.MouseAttachedLabel("AnomaliesExpected.BeamTarget.ChooseDest".Translate(parent.Label));
        //    });
        //}

        protected override void OnInteracted(Pawn caster)
        {
        }

        public void Transform()
        {
            if (BrokenStatue.DestroyedOrNull())
            {
                innerContainer.TryAdd(PawnGenerator.GeneratePawn(Props.kindDef, parent.Faction));
            }
            IntVec3 intVec3 = parent.PositionHeld;
            Map map = parent.MapHeld;
            parent.DeSpawn();
            BrokenStatueComp.GetDirectlyHeldThings().TryAdd(parent);
            if (!innerContainer.TryDrop(BrokenStatue, intVec3, map, ThingPlaceMode.Near, out var lastResultingThing))
            {
                if (!RCellFinder.TryFindRandomCellNearWith(intVec3, (IntVec3 c) => c.Standable(map), map, out var result, 1))
                {
                    Debug.LogError("Could not drop BrokenStatue!");
                }
                lastResultingThing = GenSpawn.Spawn(innerContainer.Take(BrokenStatue), result, map);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && innerContainer.removeContentsIfDestroyed)
            {
                innerContainer.removeContentsIfDestroyed = false;
            }
            //    Scribe_Values.Look(ref beamNextCount, "beamNextCount", 1);
            //    Scribe_Values.Look(ref beamMaxCount, "beamMaxCount", 1);
            //    Scribe_Values.Look(ref TickNextState, "TickNextState", Find.TickManager.TicksGame + Props.beamIntervalRange.min);
            //    Scribe_Values.Look(ref TickUnlockStudy, "TickUnlockStudy");
            //    Scribe_Values.Look(ref studyMustBeEnabled, "studyMustBeEnabled");
            //    Scribe_Values.Look(ref beamTargetState, "beamTargetState", BeamTargetState.Searching);
            //    Scribe_Values.Look(ref sendLocation, "sendLocation", IntVec3.Invalid);
        }

        //public override string CompInspectStringExtra()
        //{
        //    List<string> inspectStrings = new List<string>();
        //    int study = StudyUnlocks?.NextIndex ?? 4;
        //    if (study > 0)
        //    {
        //        inspectStrings.Add("AnomaliesExpected.BeamTarget.Indicator".Translate(beamNextCount, Props.beamMaxCount).RawText);
        //        if (study > 1)
        //        {
        //            inspectStrings.Add("AnomaliesExpected.BeamTarget.State".Translate(beamTargetState == BeamTargetState.Searching ? "AnomaliesExpected.BeamTarget.StateSearching".Translate() : "AnomaliesExpected.BeamTarget.StateActivating".Translate()).RawText);
        //        }
        //        if (beamTargetState == BeamTargetState.Activating)
        //        {
        //            if (parent.ParentHolder is MinifiedThing)
        //            {
        //                inspectStrings.Add("AnomaliesExpected.BeamTarget.TimeTillBeam".Translate(Math.Max(TickNextState + Props.ticksWhenCarried - Find.TickManager.TicksGame, Props.ticksWhenCarried).ToStringTicksToPeriodVerbose()).RawText);
        //                inspectStrings.Add("AnomaliesExpected.BeamTarget.ButtonHold".Translate().RawText);
        //            }
        //            else
        //            {
        //                inspectStrings.Add("AnomaliesExpected.BeamTarget.TimeTillBeam".Translate(Math.Max(TickNextState - Find.TickManager.TicksGame, 0).ToStringTicksToPeriodVerbose()).RawText);
        //            }
        //        }
        //    }
        //    inspectStrings.Add(base.CompInspectStringExtra());
        //    return String.Join("\n", inspectStrings);
        //}
    }
}
