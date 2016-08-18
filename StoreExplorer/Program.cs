using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Crazy hack to get administrative ObjectStore connection from this thread:
            // https://social.msdn.microsoft.com/Forums/en-US/ea979075-f602-475d-b485-3a4f787dcb70/new-media-center-addin-x64-microsoftmediacenterguidesubscribed?forum=netfx64bit
            byte[] bytes = Convert.FromBase64String("FAAODBUITwADRicSARc=");
            byte[] buffer2 = Encoding.ASCII.GetBytes("Unable upgrade recording state.");
            for (int i = 0; i != bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ buffer2[i]);
            }
            string FriendlyName = Encoding.ASCII.GetString(bytes);
            string clientId = Microsoft.MediaCenter.Store.ObjectStore.GetClientId(true);
            Console.WriteLine("ClientID={0}", clientId);
            byte[] buffer = Encoding.Unicode.GetBytes(clientId);
            string DisplayName = Convert.ToBase64String(new SHA256Managed().ComputeHash(buffer));

            Assembly assembly = Assembly.LoadFile(@"C:\windows\ehome\mcstore.dll");
            Module module = assembly.GetModules().First();
            Type formType = module.GetType("Microsoft.MediaCenter.Store.Explorer.StoreExplorerForm");
            Type objectStoreType = module.GetType("Microsoft.MediaCenter.Store.ObjectStore");

            PropertyInfo friendlyNameProperty = objectStoreType.GetProperty("FriendlyName", BindingFlags.Static | BindingFlags.Public);
            friendlyNameProperty.SetValue(null, FriendlyName, null);
            PropertyInfo displayNameProperty = objectStoreType.GetProperty("DisplayName", BindingFlags.Static | BindingFlags.Public);
            displayNameProperty.SetValue(null, DisplayName, null);

            MethodInfo defaultMethod = objectStoreType.GetMethod("get_DefaultSingleton", BindingFlags.Static | BindingFlags.Public);
            object store = defaultMethod.Invoke(null, null);
            ConstructorInfo constructor = formType.GetConstructor(new Type[] { objectStoreType });
            Form form = (Form)constructor.Invoke(new object[] { store });
            form.ShowDialog();
        }
    }
}
