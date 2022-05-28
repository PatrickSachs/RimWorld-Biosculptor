using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Lilith.RimWorld.Biosculptor {
    public class BiosculptorSettings : ModSettings {
        public HashSet<string> AddictionDefs = new HashSet<string>();
        public bool DidAutoinit = false;
        private Vector2 _scrollPosition;
        private string _search;

        public override void ExposeData() {
            Scribe_Collections.Look(ref AddictionDefs, "AddictionDefs");
            Scribe_Values.Look(ref DidAutoinit, "DidAutoinit", defaultValue: false);
            base.ExposeData();
        }

        public string SettingsCategory() {
            return "Lilith_Biosculptor_SettingsCategory".Translate();
        }

        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
        public void DoSettingsWindowContents(Rect inRect)
        {
            if (!DidAutoinit) {
                Autoinit();
            }
            var outRect = inRect.TopPart(0.9f);
            var rect = new Rect(0f, 0f, outRect.width - 18f, 1500f);
            Widgets.BeginScrollView(outRect, ref this._scrollPosition, rect, true);
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(rect);
            if (listingStandard.ButtonTextLabeled("Lilith_Biosculptor_Reset_Label".Translate(), "Lilith_Biosculptor_Reset_Button".Translate())) {
                DidAutoinit = false;
            }
            listingStandard.Label("Lilith_Biosculptor_Reset_Count".Translate(new NamedArgument(AddictionDefs.Count, "count")));
            _search = listingStandard.TextEntryLabeled("Lilith_Biosculptor_Search".Translate(), _search).ToLowerInvariant();
            foreach (var hediff in DefDatabase<HediffDef>.AllDefs) {
                if (!hediff.label.ToLowerInvariant().Contains(_search)) {
                    continue;
                }
                var enabled = AddictionDefs.Contains(hediff.defName);;
                listingStandard.CheckboxLabeled(hediff.label, ref enabled, hediff.description);
                if (enabled) {
                    AddictionDefs.Add(hediff.defName);
                } else {
                    AddictionDefs.Remove(hediff.defName);
                }
            }
            listingStandard.End();
            Widgets.EndScrollView();
        }

        public void Autoinit() {
            DidAutoinit = true;
            AddictionDefs.Clear();
            foreach (var hediff in DefDatabase<HediffDef>.AllDefs) {
                var name = hediff.defName;
                var use = (hediff.IsAddiction || name.EndsWith("Addiction") || name.EndsWith("Withdrawal") ||
                           name.EndsWith("Tolerance") || name.EndsWith("High") || name.EndsWith("Hangover") || name.EndsWith("Overdose")) 
                          && !name.Contains("Luciferium") && !name.Contains("Psychic");

                if (use) {
                    Log.Message("Autoinit found hediff for biosculptor removal: " + hediff.defName);
                    AddictionDefs.Add(hediff.defName);
                }
            }
        }
    }
}