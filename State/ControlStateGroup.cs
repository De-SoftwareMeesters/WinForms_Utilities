using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SoftwareMeesters.Windows.UI.State
{
    public class ControlStateGroup<T>
    {

        public static ControlStateGroup<T> Create(int id, Func<bool> condition, ControlStateTypes stateType = ControlStateTypes.Enabled, params T[] controls)
        {
            return new ControlStateGroup<T>(id, condition, stateType, controls);
        }

        public ControlStateGroup(int id, Func<bool> condition, ControlStateTypes stateType = ControlStateTypes.Enabled, params T[] controls)
        {
            Condition = condition;
            Controls = controls.ToList<T>();
            StateType = stateType;
            GroupId = id;
        }

        public int GroupId { get; set; }

        public Func<bool> Condition { get; set; }
        public List<T> Controls { get; set; }

        public ControlStateTypes StateType { get; set; }


        public bool IsTrue()
        {
            try
            {
                var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
                AsyncCallback callback = ar => waitHandle.Set();

                IAsyncResult result = Condition.BeginInvoke(callback, null);

                if (!waitHandle.WaitOne(1000))
                    throw new TimeoutException("Timeout.");

                return Condition.EndInvoke(result);
            }
            catch { return false; }
        }
    }

   
}
