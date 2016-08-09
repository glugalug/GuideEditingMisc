using Microsoft.MediaCenter.Guide;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MXFLoader
{
    class MergeProgramsInjector
    {
        // Placeholder to copy the original MergePrograms implmentation into
        public Microsoft.MediaCenter.Guide.Program OriginalMergePrograms(ScheduleEntry source, ScheduleEntry existing, Service targetService)
        {
            return null;
        }

        private static Type seType_ = typeof(ScheduleEntry);
        private static PropertyInfo[] scheduleEntryPropertiesToCompare_ = new PropertyInfo[] {
            // Can't do anything about payperview and repeat flags as they are strangely read-only,
            // maybe more reflection for this?
            seType_.GetProperty("Is3D"), seType_.GetProperty("IsBlackout"), seType_.GetProperty("IsCC"), seType_.GetProperty("IsClassroom"),
            seType_.GetProperty("IsDelay"), seType_.GetProperty("IsDvs"), seType_.GetProperty("IsEnhanced"), seType_.GetProperty("IsFinale"),
            seType_.GetProperty("IsHdtv"), seType_.GetProperty("IsHdtvSimulCast"), seType_.GetProperty("IsInProgress"), seType_.GetProperty("IsLetterbox"),
            seType_.GetProperty("IsLive"), seType_.GetProperty("IsLiveSports"), seType_.GetProperty("IsPremiere"), seType_.GetProperty("IsRepeatFlag"),
            seType_.GetProperty("IsSap"), seType_.GetProperty("IsSubtitled"), seType_.GetProperty("IsTape"), seType_.GetProperty("MinimumAge"),
            seType_.GetProperty("Part"), seType_.GetProperty("Parts"), seType_.GetProperty("TVRating")
        };
        public static bool ScheduleEntryFlagsMatch(ScheduleEntry se1, ScheduleEntry se2)
        {
            bool allPropertiesMatch = true;
            foreach (PropertyInfo property in scheduleEntryPropertiesToCompare_)
            {
                object val1 = property.GetValue(se1, null);
                object val2 = property.GetValue(se2, null);
                if ((val1 == null && val2 != null) || (val2 == null && val1 != null) || !val1.Equals(val2)) { 
                    Util.Trace(TraceLevel.Warning, "Mismatch found on {0} property for schedule entry {1}", property.Name, se1);
                    allPropertiesMatch = false;
                }
            }
            return allPropertiesMatch;
        }

        public static void UpdateScheduleEntryFlags(ScheduleEntry src, ScheduleEntry dst)
        {
            foreach(PropertyInfo property in scheduleEntryPropertiesToCompare_)
                property.SetValue(dst, property.GetValue(src, null), null);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Microsoft.MediaCenter.Guide.Program ReplacementMergePrograms(ScheduleEntry source, ScheduleEntry existing, Service targetService)
        {
            var program = OriginalMergePrograms(source, existing, targetService);
            if (program.GetUIdValue() == source.Program.GetUIdValue() && !ScheduleEntryFlagsMatch(source, existing))
            {
                if (!Program.options.safeMode)
                {
                    Util.Trace(TraceLevel.Warning, "ScheduleEntry flags do not match.  Updating flags in the db: MXF: {0} Store: {1}", source, existing);
                    UpdateScheduleEntryFlags(source, existing);
                    existing.Update();
                }
            }
            return program;
        }


        public static void ReplaceMergePrograms()
        {
            Util.Trace(TraceLevel.Info, "Replacing ProgramMerger.MergePrograms");
            Util.InjectMethod(typeof(ProgramMerger).GetMethod("MergePrograms").MethodHandle,
                typeof(MergeProgramsInjector).GetMethod("OriginalMergePrograms").MethodHandle);
            Util.InjectMethod(typeof(MergeProgramsInjector).GetMethod("ReplacementMergePrograms").MethodHandle,
                typeof(ProgramMerger).GetMethod("MergePrograms").MethodHandle);
        }

        // Placeholder to copy the original implementation to.
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool OriginalCheckIfProgramsMatch(ScheduleEntry entryToCompare, ScheduleEntry entryInStore, ScheduleEntry nextEntryInStore)
        {
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ReplacementCheckIfProgramsMatch(ScheduleEntry entryToCompare, ScheduleEntry entryInStore, ScheduleEntry nextEntryInStore)
        {
            if (!OriginalCheckIfProgramsMatch(entryToCompare, entryInStore, nextEntryInStore)) return false;
            bool flagsMatch = ScheduleEntryFlagsMatch(entryToCompare, entryInStore);
            if (Program.options.safeMode) return true;
            else return flagsMatch;
        }

        public static void ReplaceCheckIfProgramsMatch()
        {
            Util.Trace(TraceLevel.Info, "Replacing ProgramMatcher.CheckIfProgramsMatch");
            Util.InjectMethod(typeof(ProgramMatcher).GetMethod("CheckIfProgramsMatch").MethodHandle,
                typeof(MergeProgramsInjector).GetMethod("OriginalCheckIfProgramsMatch").MethodHandle);
            Util.InjectMethod(typeof(MergeProgramsInjector).GetMethod("ReplacementCheckIfProgramsMatch").MethodHandle,
                typeof(ProgramMatcher).GetMethod("CheckIfProgramsMatch").MethodHandle);
        }

    }
}
