using Microsoft.MediaCenter.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MXFLoader
{
    class StoredObjectsEnumeratorInjector
    {
        private static Type thisType = typeof(StoredObjectsEnumeratorInjector);
        private static Assembly storeAssembly = Assembly.GetAssembly(typeof(ObjectStore));
        private static Module module = storeAssembly.GetModules().First();

        private static Type targetType = module.GetType("MediaCenter.Store.OleDB.StoredObjectsEnumerator");
        private static FieldInfo dataFieldBindingsField = targetType.GetField("dataFieldBindings", Util.KitchenSinkMethodBindingFlags);

        private static Type fieldBindingsType = module.GetType("Microsoft.MediaCenter.Store.FieldBindings");
        private static PropertyInfo fieldBindingsFieldTypeProperty = fieldBindingsType.GetProperty("FieldType");
        private static PropertyInfo fieldBindingsNameProperty = fieldBindingsType.GetProperty("Name");

        private StoredObjectsEnumerator self {  get { return (StoredObjectsEnumerator)(object)this; } }

        // Placeholder to copy the normal implementation to
        private void OriginalUpdate(object obj, object fieldBindings)
        {

        }

        private void ReplacementUpdate(object obj, object fieldBindings)
        {
            if (fieldBindings != null)
            {
                object dataFieldBindings = dataFieldBindingsField.GetValue(self);
                if (dataFieldBindings != fieldBindings)
                {
                    string errorMessage = string.Format(
                        "Skipping update in background thread because it would crash, as fieldBindings and dataFieldBindings do not match. " +
                        "fieldBindings: {0} dataFieldBindings: {1} object: {2}", 
                        FieldBindingsToString(fieldBindings), FieldBindingsToString(dataFieldBindings), obj);
                    Console.WriteLine(errorMessage);
                    Util.Trace(TraceLevel.Error, errorMessage);
                    return;
                }
            }
            OriginalUpdate(obj, fieldBindings);
        }

        private static string FieldBindingsToString(object fieldBindings)
        {
            StringBuilder result = new StringBuilder();
            IEnumerable enumerable = (IEnumerable)fieldBindings;
            foreach(object fieldBindingObj in enumerable)
            {
                result.AppendFormat("{0}:{1},", fieldBindingsNameProperty.GetValue(fieldBindingObj, null),
                    fieldBindingsFieldTypeProperty.GetValue(fieldBindingObj, null));
            }
            return result.ToString();
        }

        private static RuntimeMethodHandle GetUpdateMethodHandle()
        {
            return targetType.GetMethod("Update", Util.KitchenSinkMethodBindingFlags, null, new Type[] { typeof(object), fieldBindingsType }, null).MethodHandle;
        }

        public static void ReplaceUpdate()
        {
            RuntimeMethodHandle targetHandle = GetUpdateMethodHandle();
            Util.InjectMethod(targetHandle, thisType.GetMethod("OriginalUpdate", Util.KitchenSinkMethodBindingFlags).MethodHandle);
            Util.InjectMethod(thisType.GetMethod("ReplacementUpdate", Util.KitchenSinkMethodBindingFlags).MethodHandle, targetHandle);
        }
    }
}
