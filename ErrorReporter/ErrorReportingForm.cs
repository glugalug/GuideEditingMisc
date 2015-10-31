using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace ErrorReporter
{
    public partial class ErrorReportingForm : Form
    {
        public ErrorReportingForm(string error_description, Exception exception) : this(error_description, exception, false)
        {
        }

        public ErrorReportingForm(string error_description, Exception exception, bool showAsDialog)
        {
            error_description_ = error_description;
            exception_ = exception;
            InitializeComponent();
            GeneratedErrorDescriptionInput.AppendText(BuildErrorMessage(error_description_, null, exception_));
            if (showAsDialog)
                this.ShowDialog();
            else this.Show();
        }

        private static string StringValueOrNull(object s)
        {
            return (s == null) ? "Null" : s.ToString();
        }

        private const string crlf = "\r\n";

        public static string SerializeException(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Exception Message: " + StringValueOrNull(exception.Message) + crlf);
            if (exception.Data != null)
            {
                sb.Append("Exception data key-values: " + crlf);
                foreach(object key in exception.Data.Keys)
                    sb.Append(key.ToString() + " = " + StringValueOrNull(exception.Data[key]) + crlf);
            }
            sb.Append("Source: " + StringValueOrNull(exception.Source) + crlf);
            sb.Append("TargetSite: " + StringValueOrNull(exception.TargetSite) + crlf);
            sb.Append("Call stack: " + StringValueOrNull(exception.StackTrace) + crlf);
            if (exception.InnerException != null && exception.InnerException != exception)
            {
                sb.Append("Inner exception: " + crlf + SerializeException(exception.InnerException));
            }
            return sb.ToString();
        }

        private string BuildErrorMessage(
            string error_description, string user_description, 
            Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Description: " + StringValueOrNull(error_description) + crlf + crlf);
            if (!string.IsNullOrEmpty(user_description))
                sb.Append("User description: " + user_description + crlf + crlf);
            if (exception != null)
                sb.Append(SerializeException(exception));
            return sb.ToString();
        }

        private string error_description_;
        private Exception exception_;

        private void CancelReportButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SendReportButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class ExceptionWithKeyValues : Exception
    {
        Dictionary<object, object> key_values_ = null;
        public ExceptionWithKeyValues(Exception innerException)
            : base(innerException.Message, innerException)
        {
            key_values_ = new Dictionary<object, object>();
        }
        public override System.Collections.IDictionary Data
        {
            get
            {
                return key_values_;
            }
        }
        public void AddKeyValue(object key, object value)
        {
            key_values_.Add(key, value);
        }
    }
}
