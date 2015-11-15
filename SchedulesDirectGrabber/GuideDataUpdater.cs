using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter.Guide;
using ChannelEditingLib;
using Microsoft.MediaCenter.Store;

namespace SchedulesDirectGrabber
{
    internal class GuideDataUpdater
    {
        public static GuideDataUpdater instance { get { return instance_; } }
        public void UpdateLineupAssociations() {
            // First locate the imported lineup to associate.
            Lineup importedLineup = FindMXFImportedLineup();

            foreach(var scannedLineupConfig in config_.scanned_lineups)
            {
                // Skip if no SDLineup has been assigned.
                if (string.IsNullOrEmpty(scannedLineupConfig.sd_lineup_id)) continue;
                // If no checkboxes selected, nothing to do.
                if (!scannedLineupConfig.addAssociation && !scannedLineupConfig.removeOtherAssociations) continue;
                // Find WMC scanned lineup.
                Lineup scannedLineup = LineupUtil.FindLineupById(scannedLineupConfig.id);
                // Loop through tuners in the scanned linup, applying the selected settings to each tuner.
                foreach(Device tuner in scannedLineup.ScanDevices)
                {
                    if (scannedLineupConfig.addAssociation)
                    {
                        if (!LineupUtil.IsLineupInLineups(tuner.WmisLineups, importedLineup))
                        {
                            tuner.WmisLineups.Add(importedLineup);
                        }
                    }
                    if (scannedLineupConfig.removeOtherAssociations)
                    {
                        List<Lineup> lineupsToRemove = new List<Lineup>();
                        foreach(Lineup l in tuner.WmisLineups)
                            if (!LineupUtil.AreSameLineup(l, importedLineup)) lineupsToRemove.Add(l);
                        foreach(Lineup l in lineupsToRemove)
                            tuner.WmisLineups.RemoveAllMatching(l);
                    }
                }
            }       
        }

        private Lineup FindMXFImportedLineup()
        {
            return LineupUtil.FindLineupByUID(ConfigManager.MXFLineup.GetLineupIdSuffix());
        }

        private ObjectStore objectStore_ { get { return ChannelEditing.object_store; } }
        private ConfigManager.SDGrabberConfig config_ { get { return ConfigManager.config; } }

        private static GuideDataUpdater instance_ = new GuideDataUpdater();
        private GuideDataUpdater() { }
    }
}
