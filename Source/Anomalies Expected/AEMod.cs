﻿using UnityEngine;
using RimWorld;
using Verse;

namespace AnomaliesExpected
{
    public class AEMod : Mod
    {
        public static AESettings Settings { get; private set; }

        public AEMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<AESettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard options = new Listing_Standard();
            options.Begin(inRect);
            options.Label("AnomaliesExpected.Settings.BakingPies.ReplicationLimit".Translate(Settings.ReplicationLimit.ToString()));
            Settings.ReplicationLimit = (int)options.Slider(Settings.ReplicationLimit, 0, 5000);
            options.CheckboxLabeled("AnomaliesExpected.Settings.BakingPies.DespawnPiecesOnDestroy".Translate().RawText, ref Settings.DespawnPiecesOnDestroy);
            options.CheckboxLabeled("AnomaliesExpected.Settings.BakingPies.SpawnPiePieceNotification".Translate().RawText, ref Settings.SpawnPiePieceNotification);
            options.GapLine();
            options.CheckboxLabeled("AnomaliesExpected.Settings.BeamTarget.PoweBeamRequireBeamTarget".Translate().RawText, ref Settings.PoweBeamRequireBeamTarget);
            options.CheckboxLabeled("AnomaliesExpected.Settings.BeamTarget.BeamTargetLetter".Translate().RawText, ref Settings.BeamTargetLetter);
            if (Current.Game != null && options.ButtonText("AnomaliesExpected.Settings.ResearchTab.Unlock".Translate().RawText))
            {
                Find.ResearchManager.Notify_MonolithLevelChanged(Find.Anomaly.Level);
            }
            options.End();
        }

        public override string SettingsCategory()
        {
            return "AnomaliesExpected.Settings.Title".Translate().RawText;
        }
    }
}
