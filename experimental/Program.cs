using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChannelEditingLib;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
namespace experimental
{
    class Program
    {
        static void Main(string[] args)
        {
            MergedLineups merged_lineups = new MergedLineups(ChannelEditing.object_store);
            MergedLineup main_lineup = merged_lineups.ToArray()[1];
            Lineup[] lineups = ChannelEditing.GetLineups();
            foreach (Lineup lineup in lineups)
            {
                if ((lineup.Name == "Scanned (Digital Cable (ClearQAM))") && lineup.ScanDevices.Empty) {
                    foreach (Channel ch in lineup.GetChannels())
                    {
                        lineup.RemoveChannel(ch);
                    }
                    lineup.Unlock();
                }
                if (!lineup.ScanDevices.Empty)
                {
                    lineup.PrimaryProvider = main_lineup;
                }
            }
            foreach (MergedLineup mergedlineup in merged_lineups)
            {
                if (mergedlineup.GetChannels().Count() == 0)
                {
                    mergedlineup.Unlock();
                    //merged_lineups.RemoveAllMatching(mergedlineup);
                }
            }
            Service[] services = new Services(ChannelEditing.object_store).ToArray();
            DeviceGroup[] device_groups = new DeviceGroups(ChannelEditing.object_store).ToArray();
            foreach (DeviceGroup device_group in device_groups)
            {
                Console.WriteLine("DeviceGroup: {0}", device_group.Name);
            }
            Device[] devices = new Devices(ChannelEditing.object_store).ToArray();
            foreach (Device d in devices)
            {
                Console.WriteLine("Device: {0}", d.Name);
            }
            DeviceType[] device_types = new DeviceTypes(ChannelEditing.object_store).ToArray();
            foreach (DeviceType dt in device_types)
            {
                Console.WriteLine("DeviceType: {0} DisplayName: {1} HeadendType: {2} Id: {3} IsSetTopBox: {4} NetworkType: {5} TuningSpaceName: {6} VideoSource: {7} ViewPriority: {8}", 
                    dt.Name, dt.DisplayName, dt.HeadendType, dt.Id, dt.IsSetTopBox, dt.NetworkType, dt.TuningSpaceName, dt.VideoSource, dt.ViewPriority);
                Microsoft.MediaCenter.TV.Tuning.TuningSpace tuning_space = dt.TuningSpace;
                if (tuning_space != null)
                {
                    Console.WriteLine("Tuning space CLSID: {0} FriendlyName: {1} UniqueName: {2}", tuning_space.CLSID, tuning_space.FriendlyName, tuning_space.UniqueName);
                }
            }
            foreach (Service service in services)
            {
                Console.WriteLine("Service: {0} IsCached: {1} IsMerged: {2}", service.Name, service.IsCached, service.IsMergedService);
            }
            if (false)
                foreach (Lineup lineup in lineups)
            {
                Console.WriteLine("Lineup: {0} ", lineup.Name);
                if (lineup.DeviceGroup != null)
                {
                    Console.WriteLine("DeviceGroup: {0}", lineup.DeviceGroup.Name);
                    if (lineup.DeviceGroup.Devices != null)
                        foreach(Device d in lineup.DeviceGroup.Devices)
                            Console.WriteLine(d.Name);
                }
                if (lineup.ScanDevices != null)
                    foreach (Device d in lineup.ScanDevices)
                        Console.WriteLine("ScanDevice: {0}", d);
                if (lineup.WmisDevices != null)
                {
                    foreach (Device d in lineup.WmisDevices)
                        Console.WriteLine("WmiDevice: {0}", d);
                    //if (lineup.WmisDevices.Empty && lineup.ScanDevices.Empty)
                        foreach (Channel ch in lineup.GetChannels())
                        {
                            if (ch.Service != null)
                            {
                                Console.WriteLine("Callsign: {0}, CHannelNumber {1}, Service Callsign {2}, Service Name {3}",
                                    ch.CallSign, ch.ChannelNumber, ch.Service.CallSign, ch.Service.Name);
                                if (!ch.Service.ScheduleEntries.Empty)
                                {
                                    foreach (ScheduleEntry entry in ch.Service.ScheduleEntries)
                                    {
                                        if (entry.Program != null)
                                        Console.WriteLine(entry.Program.Title);
                                    }
                                }
                            }
                        }
                }
 
             /*   foreach (Channel ch in lineup.GetChannels())
                {
                    Console.WriteLine("Channel: {0} Service: {1}", ch.Number, ch.Service);
                } */
            }
            MergedLineup merged_lineup = lineups[0].PrimaryProvider;
            Console.WriteLine("MergedLineup: {0}", merged_lineup);
            if (merged_lineup.SecondaryLineups != null)
                foreach (Lineup lineup in merged_lineup.SecondaryLineups)
                    Console.WriteLine("Secondary Lineup: {0}", lineup.Name);
            Console.ReadLine();
            FavoriteLineups favorite_lineups = new FavoriteLineups(ChannelEditing.object_store);
            foreach (FavoriteLineup favorite in favorite_lineups)
            {
                Console.WriteLine(favorite.Name);
            }
            PackageSubscriptions subscriptions = new PackageSubscriptions(ChannelEditing.object_store);
            foreach (PackageSubscription sub in subscriptions) {
                Console.WriteLine(sub.Package.Description);
            }
        }
    }
}
