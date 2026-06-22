using System.Collections.Generic;
using Verse;

namespace AnomaliesExpected
{
    public class HediffComp_ObservingStage : HediffComp
    {
        private bool isIgnoringObserver;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (Pawn.IsHashIntervalTick(20))
            {
                if (!(AEMod.Settings.IsIgnoringObserver || isIgnoringObserver) && Pawn.SpawnedOrAnyParentSpawned)
                {
                    if (Find.CameraDriver.CurrentViewRect.Contains(Pawn.PositionHeld))
                    {
                        parent.Severity = 0.5f;
                        return;
                    }
                }
                parent.Severity = 1;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (DebugSettings.ShowDevGizmos && AEMod.Settings.DevModeInfo)
            {
                yield return new Command_Action
                {
                    action = delegate
                    {
                        isIgnoringObserver = !isIgnoringObserver;
                    },
                    defaultLabel = "Dev: Ignore Camera",
                    defaultDesc = $"Will not consider player camera as observer for effect. [{isIgnoringObserver}]"
                };
            }
        }
    }
}
