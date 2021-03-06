﻿using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MXFLoader
{
    class ScheduleEntriesInjector
    {
        private ScheduleEntries ThisAsScheduleEntries()
        {
            return (ScheduleEntries)(object)this;
        }

        private static Type thisType { get { return typeof(ScheduleEntriesInjector); } }

        #region constructor replacment
        // Placeholder for the original ScheduleEntries constructor.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void OriginalConstructor(ObjectStore objStore)
        {
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ReplacementConstructor(ObjectStore objStore)
        {
            OriginalConstructor(objStore);
            ScheduleEntries scheduleEntries = ThisAsScheduleEntries();
            scheduleEntries.ProgramMerger.AlwaysReplace = true;
            scheduleEntries.ProgramMerger.NeverReplace = false;
            scheduleEntries.ProgramMatcher.MatchTitleEpisode = false;
        }
        
        private static void ReplaceScheduleEntriesConstructor()
        {
            if (Environment.OSVersion.Version.Major > 6 || Environment.OSVersion.Version.Minor > 1)
            {
                Util.Trace(TraceLevel.Info,
                    "Not replacing ScheduleEntries constructor because this only seems to work with Windows 7.  " +
                    "It is ultimately redundant with the ScheduleEntries.MergeScheduleEntries replacement.");
                return;
            }
            Util.Trace(TraceLevel.Info,"Replacing ScheduleEntries(ObjectStore) constructor");
            RuntimeMethodHandle targetConstructor = typeof(ScheduleEntries).GetConstructor(new Type[] { typeof(ObjectStore) }).MethodHandle;
            Util.InjectMethod(
                targetConstructor,
                thisType.GetMethod("OriginalConstructor", BindingFlags.NonPublic | BindingFlags.Instance).MethodHandle);
            Util.InjectMethod(
                thisType.GetMethod("ReplacementConstructor", BindingFlags.NonPublic | BindingFlags.Instance).MethodHandle,
                targetConstructor);
        }
        #endregion

        #region invoked method wrappers
        private static MethodInfo FindStartScheduleEntry_ =
            typeof(ScheduleEntries).GetMethod("FindStartScheduleEntry", Util.KitchenSinkMethodBindingFlags);
        private static MethodInfo NextScheduleEntryForService_ =
            typeof(ScheduleEntries).GetMethod("NextScheduleEntryForService", Util.KitchenSinkMethodBindingFlags);
        private static MethodInfo ApplyMergedScheduleEntriesToStore_ =
            typeof(ScheduleEntries).GetMethod("ApplyMergedScheduleEntriesToStore", Util.KitchenSinkMethodBindingFlags);

        private ScheduleEntry FindStartScheduleEntry(StoredObjectsEnumerator<ScheduleEntry> enumerator, ScheduleEntry scheduleEntryToMerge)
        {
            ScheduleEntry result = (ScheduleEntry)FindStartScheduleEntry_.Invoke(
                ThisAsScheduleEntries(), new object[] { enumerator, scheduleEntryToMerge });
            if (result == null)
            {
                Util.Trace(TraceLevel.Error, "FindStartScheduleEntry returned null for ScheduleEntry {0}", scheduleEntryToMerge);
                result = enumerator.Current;
            }
            return result;
        }

        private ScheduleEntry NextScheduleEntryForService(StoredObjectsEnumerator<ScheduleEntry> enumerator, long serviceId)
        {
            return (ScheduleEntry)NextScheduleEntryForService_.Invoke(ThisAsScheduleEntries(), new object[] { enumerator, serviceId });
        }

        private void ApplyMergedScheduleEntriesToStore(Service targetService, List<ScheduleEntry> updates, List<ScheduleEntry> adds, List<ScheduleEntry> deletes)
        {
            ApplyMergedScheduleEntriesToStore_.Invoke(ThisAsScheduleEntries(), new object[] { targetService, updates, adds, deletes });
        }
        private void ApplyUpdatedScheduleEntryToStore(ScheduleEntry entry)
        {
            ApplyMergedScheduleEntriesToStore(
                entry.Service, new List<ScheduleEntry> { entry }, new List<ScheduleEntry>(), new List<ScheduleEntry>());
        }
        #endregion

        private ScheduleEntry SmartFindStartScheduleEntry(StoredObjectsEnumerator<ScheduleEntry> enumerator, ScheduleEntry entryToMerge)
        {
            ScheduleEntry current = null;
            ServiceStartTimeKey key = new ServiceStartTimeKey(entryToMerge.Service, entryToMerge.StartTime - TimeSpan.FromSeconds(1));
            if (enumerator.Seek(key, SeekType.BeforeEQ))
            {
                current = enumerator.Current;
                if (current.Service == null || current.Service.Id != entryToMerge.Service.Id ||
                    current.EndTime < entryToMerge.StartTime + TimeSpan.FromSeconds(1))
                {
                    current = this.NextScheduleEntryForService(enumerator, entryToMerge.Service.Id);
                }
                return current;
            }
            enumerator.Reset();
            return this.NextScheduleEntryForService(enumerator, entryToMerge.Service.Id);
        }

        private List<ScheduleEntry> GetScheduleEntriesForService(ScheduleEntries entriesInStore, Service service)
        {
            List<ScheduleEntry> entries = new List<ScheduleEntry>();
            ServiceStartTimeKey start_key = new ServiceStartTimeKey(service, new DateTime(0));
            long id = service.Id;
            using (StoredObjectsEnumerator<ScheduleEntry> enumerator = entriesInStore.GetStoredObjectsEnumerator())
            {
                if (enumerator.Seek(start_key, SeekType.AfterEQ))
                {
                    do
                    {
                        ScheduleEntry entry = enumerator.Current;
                        if (entry.Service == null)
                        {
                            Util.Trace(TraceLevel.Error, "Schedule entry with no service assignment: {0}", entry);
                            continue;
                        }
                        if (entry.Service.Id != id) break;
                        entries.Add(entry);
                    } while (enumerator.MoveNext());

                }
            }
            return entries;
        }

        public static bool ScheduleEntryProgramsAndTimeslotMatch(ScheduleEntry mxfEntry, ScheduleEntry storeEntry)
        {
            return mxfEntry.Program != null && storeEntry.Program != null &&
                mxfEntry.Program.Id == storeEntry.Program.Id && TimeslotsMatch(mxfEntry, storeEntry);
        }

        #region scheduleEntriesToMerge replacement
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void OriginalMergeScheduleEntries(ScheduleEntry[] scheduleEntriesToMerge)
        {

        }

        static private void UpdateChannelService(Channel ch, Service s)
        {
            if (ch.Service == null || ch.Service.Id != s.Id)
            {
                if (ch == null)
                {
                    Util.Trace(TraceLevel.Error, "Null channel object, service {0}!", s);
                    return;
                }
                if (ch.ChannelType == ChannelType.UserAdded)
                {
                    Util.Trace(TraceLevel.Info, "Skipping service update for channel {0} because it is user added", ch);
                    return;
                }
                Util.Trace(TraceLevel.Warning, "Updating service for channel {0} to {1}", ch, s);
                ch.Service = s;
                ch.Update();
            }
        }

        private static void FixServiceAssignmentsForServiceChannels(Service targetService)
        {
            if (Program.options.safeMode || Program.options.dontUpdateServiceAssignments) return;
            if (targetService.ReferencingChannels != null)
            {
                foreach (Channel ch in targetService.ReferencingChannels)
                {
                    UpdateChannelService(ch, targetService);
                    if (ch == null) continue;
                    if (ch.ReferencingPrimaryChannels != null)
                    {
                        foreach (Channel referenced in ch.ReferencingPrimaryChannels)
                        {
                            UpdateChannelService(referenced, targetService);
                        }
                    }
                    if (ch.ReferencingSecondaryChannels != null)
                    {
                        foreach (Channel referenced in ch.ReferencingSecondaryChannels)
                        {
                            UpdateChannelService(referenced, targetService);
                        }
                    }
                }
            }

        }

        private static bool TimeIsAboutAtOrBefore(DateTime first, DateTime second)
        {
            return first - second < TimeSpan.FromSeconds(0.5);
        }

        private string ListScheduleEntrys(IEnumerable<ScheduleEntry> entrys)
        {
            StringBuilder entryList = new StringBuilder();
            foreach(var entry in entrys)
            {
                entryList.Append(string.Format("{0},", entry));
            }
            return entryList.ToString();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ReplacementMergeScheduleEntries(ScheduleEntry[] scheduleEntriesToMerge)
        {
            ReplacementMergeScheduleEntriesInner(scheduleEntriesToMerge);
        }

        private static int CompareScheduleEntryStartTimes(ScheduleEntry entry1, ScheduleEntry entry2)
        {
            if (entry1.StartTime < entry2.StartTime) return -1;
            if (entry2.StartTime < entry1.StartTime) return 1;
            return 0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ReplacementMergeScheduleEntriesInner(ScheduleEntry[] scheduleEntriesToMerge)
        {
            if (scheduleEntriesToMerge.Count() == 0) return; // nothing to do.
            Util.WaitForBackgroundThreads();
            Service targetService = scheduleEntriesToMerge[0].Service;
            Util.Trace(TraceLevel.Info, "Processing schedule entries for {0}", targetService);
            long id = targetService.Id;
            FixServiceAssignmentsForServiceChannels(targetService);
            Util.WaitForBackgroundThreads();
            OriginalMergeScheduleEntries(scheduleEntriesToMerge);
            Util.WaitForBackgroundThreads();

            ScheduleEntries self = ThisAsScheduleEntries();
            Mutex mutex = null;
            try
            {
                mutex = targetService.AcquireUpdateLock();
                List<ScheduleEntry> storeEntries =
                    GetScheduleEntriesForService(new ScheduleEntries(self.ObjectStore), targetService);
                storeEntries.Sort(CompareScheduleEntryStartTimes);
                Util.Trace(TraceLevel.Verbose, "store: {0} mxf: {1}", 
                    ListScheduleEntrys(storeEntries), ListScheduleEntrys(scheduleEntriesToMerge));

                #region pad store entry list with bogus begin/end time entries
                // Create bogus store entries at the beginning and end of time to make the loop below simpler.
                ScheduleEntry endOfTimeEntry = new ScheduleEntry();
                endOfTimeEntry.StartTime = DateTime.MaxValue;
                endOfTimeEntry.EndTime = DateTime.MaxValue;
                storeEntries.Add(endOfTimeEntry);
                ScheduleEntry beginningOfTimeEntry = new ScheduleEntry();
                beginningOfTimeEntry.StartTime = DateTime.MinValue;
                beginningOfTimeEntry.StartTime = DateTime.MinValue;
                storeEntries.Insert(0, beginningOfTimeEntry);
                #endregion

                List<ScheduleEntry> adds = new List<ScheduleEntry>();
                List<ScheduleEntry> updates = new List<ScheduleEntry>();
                List<ScheduleEntry> deletes = new List<ScheduleEntry>();

                int mxfIndex = 0; int storeIndex = 1;
                DateTime lastMXFEndTime = new DateTime(0);
                while (mxfIndex < scheduleEntriesToMerge.Count())
                {
                    ScheduleEntry mxfEntry = scheduleEntriesToMerge[mxfIndex];
                    ScheduleEntry storeEntry = storeEntries[storeIndex];
                    if (lastMXFEndTime > storeEntry.StartTime)
                    {
                        ScheduleEntry lastMxfEntry = scheduleEntriesToMerge[mxfIndex - 1];
                        if (lastMXFEndTime > storeEntry.EndTime)
                        {
                            Util.Trace(TraceLevel.Verbose, "Store entry is contained within timeslot of previous MXF entry, removing it.  store: {0} mxf: {1}", storeEntry, mxfEntry);
                            if (!Program.options.safeMode)
                            {
                                deletes.Add(storeEntry);
                            }
                            ++storeIndex;
                            continue;
                        }
                        Util.Trace(TraceLevel.Verbose, "Store entry overlaps with previous MXF entry. Truncating start. store: {0} mxf: {1}", storeEntry, lastMxfEntry);
                        if (!Program.options.safeMode)
                        {
                            storeEntry.StartTime = lastMxfEntry.EndTime;
                            updates.Add(storeEntry);
                            ++storeIndex;
                            continue;
                        }
                    }
                    if (TimeIsAboutAtOrBefore(storeEntry.EndTime, mxfEntry.StartTime))
                    {
                        Util.Trace(TraceLevel.Verbose, "Ignoring store ScheduleEntry {0} as it does not overlap the current MXF entry : {1}", storeEntry, mxfEntry);
                        ++storeIndex;
                        continue;
                    }
                    if (TimeslotsMatch(mxfEntry, storeEntry))
                    {
                        lastMXFEndTime = storeEntry.EndTime;
                        if (mxfEntry.Program.Id != storeEntry.Program.Id)
                        {
                            SeriesInfo mxfSeries = mxfEntry.Program.Series;
                            SeriesInfo storeSeries = storeEntry.Program.Series;
                            if (mxfSeries != null && storeSeries != null && mxfSeries.Id == storeSeries.Id && mxfEntry.Program.IsGeneric && !storeEntry.Program.IsGeneric)
                            {
                                Util.Trace(TraceLevel.Info, "Skipping update because the MXF has a generic entry while DB already has a specific episode: store: {0} mxf: {1}", storeEntry, mxfEntry);
                            }
                            else
                            {
                                Util.Trace(TraceLevel.Warning, "Programs do not match after normal ScheduleEntriesToMerge, queueing update.  Store: {0} MXF: {1}", storeEntry, mxfEntry);
                                if (!Program.options.safeMode)
                                {
                                    storeEntry.Program = mxfEntry.Program;
                                    MergeProgramsInjector.UpdateScheduleEntryFlags(mxfEntry, storeEntry);
                                    updates.Add(storeEntry);
                                }
                            }
                        }
                        else if (!MergeProgramsInjector.ScheduleEntryFlagsMatch(storeEntry, mxfEntry))
                        {
                            Util.Trace(TraceLevel.Warning, "Mismatched scheduleEntry flags found, queueing update.  store: {0} MXF: {1}", storeEntry, mxfEntry);
                            if (!Program.options.safeMode)
                            {
                                MergeProgramsInjector.UpdateScheduleEntryFlags(mxfEntry, storeEntry);
                                // MS MergeScheduleEntries treats this as an add/remove rather than an update.  I don't like this, hopefully
                                // this way can work.
                                updates.Add(storeEntry);
                            }
                        }
                        else
                        {
                            Util.Trace(TraceLevel.Verbose, "Entry for store and MXF already match: {0}", storeEntry);
                        }
                        ++mxfIndex;
                        ++storeIndex;
                        continue;
                    }
                    ScheduleEntry lastStoreEntry = storeEntries[storeIndex - 1];
                    // If we get here, store entry ends after mxf entry starts.  Find out if the program ID matches; it may be only a timeslot change.
                    if (storeEntry.Program.Id == mxfEntry.Program.Id)
                    {
                        Util.Trace(TraceLevel.Warning, "DB entry overlapping the MXF ScheduleEntry has the same program.  Updating the timeslot and possibly flags. store: {0} MXF: {1}", storeEntry, mxfEntry);
                        if (!Program.options.safeMode)
                        {
                            if (TimeApproxEquals(mxfEntry.StartTime, lastStoreEntry.EndTime))
                            {
                                storeEntry.StartTime = lastStoreEntry.EndTime;
                            } else
                            {
                                storeEntry.StartTime = mxfEntry.StartTime;
                            }
                            ScheduleEntry nextStoreEntry = storeEntries[storeIndex + 1];
                            if (TimeApproxEquals(mxfEntry.EndTime, nextStoreEntry.StartTime))
                            {
                                storeEntry.EndTime = nextStoreEntry.StartTime;
                            } else
                            {
                                storeEntry.EndTime = mxfEntry.EndTime;
                            }
                            MergeProgramsInjector.UpdateScheduleEntryFlags(mxfEntry, storeEntry);
                            updates.Add(storeEntry);
                        }
                        lastMXFEndTime = storeEntry.EndTime;
                        ++mxfIndex;
                        ++storeIndex;
                        continue;
                    }

                    if (storeEntry.StartTime < mxfEntry.StartTime && TimeIsAboutAtOrBefore(storeEntry.EndTime, mxfEntry.EndTime))
                    {
                        Util.Trace(TraceLevel.Warning, "DB entry overlapping MXF ScheduleEntry is earlier.  Truncating the end of it.  store: {0} mxf: {1}", storeEntry, mxfEntry);
                        if (!Program.options.safeMode)
                        {
                            storeEntry.EndTime = mxfEntry.StartTime;
                            updates.Add(storeEntry);
                        }
                        ++storeIndex;
                        continue;
                    }
                    if (TimeIsAboutAtOrBefore(mxfEntry.StartTime, storeEntry.StartTime) && TimeIsAboutAtOrBefore(storeEntry.EndTime, mxfEntry.EndTime))
                    {
                        Util.Trace(TraceLevel.Warning, "DB entry timeslot is within MXF ScheduleEntry timeslot.  Deleting it.  store: {0} mxf: {1}", storeEntry, mxfEntry);
                        if (!Program.options.safeMode)
                        {
                            deletes.Add(storeEntry);
                        }
                        ++storeIndex;
                        continue;
                    }
                    if (TimeIsAboutAtOrBefore(mxfEntry.EndTime, storeEntry.StartTime))
                    {
                        Util.Trace(TraceLevel.Warning, "Next db entry is entirely after the MXF entry.  Adding the MXF entry. store: {0} mxf: {1}", storeEntry, mxfEntry);
                        if (!Program.options.safeMode)
                        {
                            // check for rounding error.
                            if (mxfEntry.EndTime > storeEntry.StartTime) mxfEntry.EndTime = storeEntry.StartTime;
                            adds.Add(mxfEntry);
                        }
                        lastMXFEndTime = mxfEntry.EndTime;
                        ++mxfIndex;
                        continue;
                    }
                    // If we ge there, store entry starts within MXF entry and ends later.
                    Util.Trace(TraceLevel.Warning, "Overlapping store entry is at end of MXF ScheduleEntry.  Truncating the start.  store: {0} mxf: {1}", storeEntry, mxfEntry);
                    if (!Program.options.safeMode)
                    {
                        storeEntry.StartTime = mxfEntry.EndTime;
                        updates.Add(storeEntry);
                    }
                    ++storeIndex;
                }

                if (!Program.options.safeMode && updates.Count + adds.Count + deletes.Count > 0)
                {
                    List<ScheduleEntry> emptyList = new List<ScheduleEntry>();
                    ApplyMergedScheduleEntriesToStore(targetService, emptyList, emptyList, deletes);
                    ApplyMergedScheduleEntriesToStore(targetService, updates, emptyList, emptyList);
                    ApplyMergedScheduleEntriesToStore(targetService, emptyList, adds, emptyList);
                }
            }
            finally
            {
                targetService.ReleaseUpdateLock(mutex);
            }
        }

        private static bool TimeApproxEquals(DateTime t1, DateTime t2)
        {
            return Math.Abs((t1 - t2).TotalSeconds) < 0.5;
        }

        private static bool TimeslotsMatch(ScheduleEntry mxfEntry, ScheduleEntry storeEntry)
        {
            return TimeApproxEquals(mxfEntry.StartTime, storeEntry.StartTime) &&
                TimeApproxEquals(mxfEntry.EndTime, storeEntry.EndTime);
        }

        private static void ReplaceMergeScheduleEntries()
        {
            Util.Trace(TraceLevel.Info, "Replacing ScheduleEntries.MergeScheduleEntries");
            RuntimeMethodHandle target = typeof(ScheduleEntries).GetMethod("MergeScheduleEntries", BindingFlags.NonPublic | BindingFlags.Instance).MethodHandle;
            Util.InjectMethod(target, thisType.GetMethod("OriginalMergeScheduleEntries", BindingFlags.NonPublic | BindingFlags.Instance).MethodHandle);
            Util.InjectMethod(thisType.GetMethod("ReplacementMergeScheduleEntries", BindingFlags.NonPublic | BindingFlags.Instance).MethodHandle, target);
        }
        #endregion

        private bool ProgramIdsMatch(ScheduleEntry mxfEntry, ScheduleEntry storeEntry)
        {
            return mxfEntry.Program.GetUIdValue() == storeEntry.Program.GetUIdValue();
        }

        private bool TimeslotsExactMatch(ScheduleEntry mxfEntry, ScheduleEntry storeEntry)
        {
            return (mxfEntry.StartTime == storeEntry.StartTime) && (mxfEntry.EndTime == storeEntry.EndTime);
        }

        private bool TimeIsBetween(DateTime time, DateTime start, DateTime end)
        {
            return start < time && time < end;
        }
        private bool TimeslotsOverlap(ScheduleEntry mxfEntry, ScheduleEntry storeEntry)
        {
            return TimeIsBetween(mxfEntry.StartTime, storeEntry.StartTime, storeEntry.EndTime) ||
                TimeIsBetween(mxfEntry.EndTime, storeEntry.StartTime, storeEntry.EndTime) ||
                TimeIsBetween(storeEntry.StartTime, mxfEntry.StartTime, mxfEntry.EndTime) || TimeslotsExactMatch(mxfEntry, storeEntry);
        }

        public static void InjectReplacementScheduleEntriesMethods()
        {
            if (!Program.options.safeMode)
            {
                ReplaceScheduleEntriesConstructor();
            }
            ReplaceMergeScheduleEntries();
        }
    }
}
