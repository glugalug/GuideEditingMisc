using Microsoft.MediaCenter.Pvr;
using Microsoft.MediaCenter.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MXFLoader
{
    class SchedulerWorkerInjector
    {
        public SchedulerWorkerInjector()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Scheduler));
            Module module = assembly.GetModules().First();
            schedulerWorkerType_ = module.GetType("Microsoft.MediaCenter.Pvr.SchedulerWorker");
            schedulerField_ = GetField(schedulerWorkerType_, "scheduler");
            bulkUpdateLockField_ = GetField(schedulerWorkerType_, "bulkUpdateLock");
            mutexLockType_ = module.GetType("Microsoft.MediaCenter.Pvr.MutexLock");
            mutexLockConstructor_ = mutexLockType_.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(ObjectStore), typeof(string) }, null);
        }

        private FieldInfo GetField(Type type, string memberName)
        {
            return type.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Public);
        }

        static private Type schedulerWorkerType_ = null;
        static private Type mutexLockType_ = null;
        static private FieldInfo schedulerField_ = null;
        static private FieldInfo bulkUpdateLockField_ = null;
        static private ConstructorInfo mutexLockConstructor_ = null;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ReplacementThreadSetup()
        {            
            schedulerField_.SetValue(this, new Scheduler(Util.object_store, ScheduleConflictSource.AutomaticUpdate));
            bulkUpdateLockField_.SetValue(this, mutexLockConstructor_.Invoke(new object[] { Util.object_store, Scheduler.SchedulerWorkerMutexName }));
        }

        public void ReplaceThreadSetup()
        {
            Util.InjectMethod(typeof(SchedulerWorkerInjector).GetMethod("ReplacementThreadSetup", Util.KitchenSinkMethodBindingFlags).MethodHandle,
                schedulerWorkerType_.GetMethod("ThreadSetup", Util.KitchenSinkMethodBindingFlags).MethodHandle);
        }
    }
}
