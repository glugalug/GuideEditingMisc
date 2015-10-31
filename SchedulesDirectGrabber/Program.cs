using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using ChannelEditingLib;

namespace SchedulesDirectGrabber
{
    static class Program
    {
        static Lineup FindQAMLineup()
        {
            Lineups lineups = new Lineups(ChannelEditing.object_store);
            foreach(Lineup lineup in lineups)
            {
                if (lineup.Name == "Scanned (Digital Cable (ClearQAM))")
                {
                    return lineup;
                }
            }
            throw new Exception("QAM lineup not found");
        }
        static void FindUnencryptedQAMChannels()
        {
            Lineup qam_lineup = FindQAMLineup();
            foreach(Channel ch in qam_lineup.GetChannels())
            {
                ChannelTuningInfo tuning_info = ch.TuningInfos.First as ChannelTuningInfo;
                if (tuning_info.IsEncrypted) continue;
                Console.WriteLine("ClearQAM channel: physical channel number: {0}.{1}, callsign: {2}",
                    tuning_info.PhysicalNumber, tuning_info.SubNumber, ch.CallSign);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach(var d in new Devices(ChannelEditing.object_store))
            {
                Device device = d as Device;
                Console.WriteLine("Device id: {0}\n\tname: {1}\n\ttype: {2}\n\tuids: {3}",
                    device.Id, device.Name, device.DeviceType, string.Join(",", device.UIds));
            }
//            FindUnencryptedQAMChannels();
            SDTokenManager token_manager = LoginForm.LoginAndGetTokenManager();
            StatusResponse status = new SDStatusReader(token_manager).GetSchedulesDirectStatus();
            JSONClient.DisplayJSON(status);
            if (!status.IsOnline())
            {
                MessageBox.Show("SchedulesDirect JSON API is currently offline.  Try again later.");
                Application.Exit();
            }
            /*            var schedule_responses = SDSchedules.GetStationScheduleResponses(new List<string>{
                            "58623", "62420" });
                        HashSet<string> programIDs = new HashSet<string>();
                        foreach(var schedule_response in schedule_responses)
                        {
                            if (schedule_response.programs != null)
                                foreach(var program in schedule_response.programs)
                                    programIDs.Add(program.programID);
                        }
                        List<SDProgram> programs = SDProgramFetcher.FetchPrograms(programIDs); */
            //            SDAccountManagement.AddLineupToAccount("USA-NY67791-QAM");
            Application.Run(new ConfigForm(token_manager, status));
        }
    }
}
