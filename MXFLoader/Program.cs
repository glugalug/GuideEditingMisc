using CommandLine;
using CommandLine.Text;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using Microsoft.MediaCenter.Store.MXF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MXFLoader
{
    class Options
    {
        [Option('i', "input_mxf", Required = true,
         HelpText = "Path to input MXF file")]
        public string inputMxfPath { get; set; }
        [Option('s', "safe_mode", DefaultValue =false,
         HelpText = "Only make changes that the regular loadMXF would.  Things it misses may still be output.")]
        public bool safeMode { get; set; }
        [Option("skip_listing_consistency", DefaultValue = false,
         HelpText ="Don't make listings assignments of Channels and MergedChannels consistent. Making them consistent avoids failed update "+
            "attempts from becoming infinitely recursive and crashing the import.  Ignored if -safe_mode is true")]
        public bool dontUpdateServiceAssignments { get; set; }
        [Option('c', "console_loglevel", DefaultValue = 2,
         HelpText = "Trace level severity to output to console.  0=none, 1=error, 2=warning, 3=info, 4=verbose.  Logging to file through " +
            "ObjectStore tracing is controlled separately by registry value.")]
        public int consoleLoglevel { get; set; }
        [Option("reindex", DefaultValue =false,
         HelpText ="Kick off the ReindexSearchRoot task when import is completed.")]
        public bool reindexWhenDone { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    class Program
    {
        public static Options options = new Options();
        static void Main(string[] args)
        {
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Invalid command line options: {0}", options.GetUsage());
                return;
            }
            ScheduleEntriesInjector.InjectReplacementScheduleEntriesMethods();
            //MergeProgramsInjector.ReplaceMergePrograms();
            MergeProgramsInjector.ReplaceCheckIfProgramsMatch();
            MxfImporter.Import(new StreamReader(options.inputMxfPath).BaseStream, Util.object_store, MxfImportProgressCallback);
            if (options.reindexWhenDone)
            {
                reindexDatabase();
            }
        }

        private static bool MxfImportProgressCallback(int amountCompleted)
        {
            Console.WriteLine("{0}%", amountCompleted * 0.1);
            return true;
        }

        // SHamelessly copied from disassembly of epg123
        public static void reindexDatabase()
        {
            Console.WriteLine("\n\nKicking off ReindexSearchRoot task ...");
            Process process = new Process();
            process.StartInfo.FileName = Environment.ExpandEnvironmentVariables("%WINDIR%") + @"\ehome\ehPrivJob.exe";
            process.StartInfo.Arguments = "/DoReindexSearchRoot";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.OutputDataReceived += new DataReceivedEventHandler(Program.task_OutputDataReceived);
            process.BeginOutputReadLine();
            process.ErrorDataReceived += new DataReceivedEventHandler(Program.task_ErrorDataReceived);
            process.BeginErrorReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new Exception(string.Format("Error using ehPrivJob.exe to start ReindeSearchRoot task.  Exit code: {0}", process.ExitCode));
        }

        private static void task_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string str = e.Data.ToString();
                if (str.Length > 0)
                {
                    Console.WriteLine(string.Format("{0}", str));
                }
            }
        }

        private static void task_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string str = e.Data.ToString();
                if (str.Length > 0)
                {
                    Console.WriteLine(string.Format("{0}", str));
                }
            }
        }
    }
}
