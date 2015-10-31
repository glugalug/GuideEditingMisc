using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Security.Permissions;

namespace GuideEditor2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread,
         SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(UnhandledThreadExceptionHandler);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static void UnhandledThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Console.WriteLine("Unhandled exception occured, Sender: {0}, exception: {1} message: {2} call stack: {3}",
                sender, e.Exception, e.Exception.Message, e.Exception.StackTrace);
            new ErrorReporter.ErrorReportingForm("Unhandled thread exception", e.Exception);
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Unhandled exception occured, Sender: {0}, exception: {1}", sender, e.ExceptionObject);
            ErrorReporter.ErrorReportingForm errorForm = new ErrorReporter.ErrorReportingForm(string.Format("Unhandled exception {0}", e.ExceptionObject), e.ExceptionObject as Exception, true);
            if (e.IsTerminating)
            {
                MessageBox.Show("App will terminate when you click OK", "Fatal Exception", MessageBoxButtons.OK);
            }
        }
    }
}
