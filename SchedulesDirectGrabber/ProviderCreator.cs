using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter.Store;
using ChannelEditingLib;
using Microsoft.MediaCenter.Guide;

namespace SchedulesDirectGrabber
{
    static class ProviderCreator
    {
        internal static void ListProviders()
        {
            using (Providers providers = new Providers(ChannelEditing.object_store))
            {
                foreach(Provider provider in providers)
                {
                    Console.WriteLine("Copyright: {0} DisplayName: {1} Id: {2} Name: {3} Provider: {4} Uids: {5}", 
                        provider.Copyright, provider.DisplayName, provider.Id, provider.Name, provider.Provider,
                        Misc.UidsToString(provider.UIds));
                }
            }
        }

        const string kProviderUIDValue = "GSDProvider";

    }
}
