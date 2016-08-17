using Microsoft.MediaCenter.Pvr;
using Microsoft.MediaCenter.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace MXFLoader
{
    class Util
    {
        public static ObjectStore object_store
        {
            get
            {
                if (object_store_ == null)
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
                    ObjectStore.FriendlyName = FriendlyName;
                    ObjectStore.DisplayName = DisplayName;
                    object_store_ = ObjectStore.DefaultSingleton; //Microsoft.MediaCenter.Store.ObjectStore.Open("", FriendlyName, DisplayName, true);
                    Util.Trace(TraceLevel.Info, "ObjectStore instance created with FriendlyName='{0}' DisplayName='{1}'",
                        ObjectStore.FriendlyName, ObjectStore.DisplayName);
                }
                return object_store_;
            }
        }
        private static ObjectStore object_store_;

        // Based on LogMan's answer to this StackOverflow question:
        // http://stackoverflow.com/questions/7299097/dynamically-replace-the-contents-of-a-c-sharp-method
        public static void InjectMethod(RuntimeMethodHandle methodToInject, RuntimeMethodHandle methodToReplace)
        {
            RuntimeHelpers.PrepareMethod(methodToReplace);
            RuntimeHelpers.PrepareMethod(methodToInject);
            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.Value.ToPointer() + 2;
                    *tar = *inj;
                }
                else
                {

                    long* inj = (long*)methodToInject.Value.ToPointer() + 1;
                    long* tar = (long*)methodToReplace.Value.ToPointer() + 1;
                    *tar = *inj;
                }
            }

        }

        public static BindingFlags KitchenSinkMethodBindingFlags {  get { return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static; } }

        public static void Trace(TraceLevel level, string formattedMessage, params object[] args)
        {
            if ((int)level <= Program.options.consoleLoglevel)
            {
                Console.WriteLine(formattedMessage, args);
            }
            ObjectStore.Trace.WriteLine(level, formattedMessage, args);
        }

        public static void WaitForBackgroundThreads()
        {
            Util.Trace(TraceLevel.Verbose, "Waiting for background threads.");
            try
            {
                ObjectStore.WaitForThenBlockBackgroundThreads(int.MaxValue);
            }
            finally
            {
                ObjectStore.UnblockBackgroundThreads();
            }
            Util.Trace(TraceLevel.Verbose, "Done waiting for background threads.");
        }
    }
}
