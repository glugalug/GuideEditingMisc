using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter.Guide;
using ChannelEditingLib;
using Microsoft.MediaCenter.Store;

namespace SchedulesDirectGrabber
{
    internal static class LineupUtil
    {
        public static Lineup FindLineupByUID(string uid)
        {
            using (Lineups lineups = new Lineups(objectStore_))
            {
                foreach (Lineup lineup in lineups)
                {
                    if (Misc.UidsContainsIdValue(lineup, uid))
                        return lineup;
                }
                return null;
            }
        }

        public static Lineup FindLineupById(long id)
        {
            using (Lineups lineups = new Lineups(objectStore_))
            {
                return lineups[id];
            }
        }

        public static bool AreSameLineup(Lineup l1, Lineup l2)
        {
            return l1.Id == l2.Id && l1.Name == l2.Name;
        }

        public static bool IsLineupInLineups(Lineups lineups, Lineup lineup)
        {
            foreach (Lineup l in lineups)
                if (AreSameLineup(l, lineup)) return true;
            return false;
        }

        private static ObjectStore objectStore_ { get { return ChannelEditing.object_store; } }
    }
}
