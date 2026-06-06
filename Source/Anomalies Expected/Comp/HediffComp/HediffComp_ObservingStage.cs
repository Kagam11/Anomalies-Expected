using System.Collections.Generic;
using UnityEngine;
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
                    Vector2 pos = (Vector2)Find.Camera.WorldToScreenPoint(Pawn.DrawPos) / Prefs.UIScale;
                    if (pos.x >= 0 && pos.y >= 0 && pos.x <= Screen.width && pos.y <= Screen.height)
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
            if (DebugSettings.ShowDevGizmos)
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
