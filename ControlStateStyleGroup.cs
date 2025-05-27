using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SoftwareMeesters.Windows.UI
{
    public class ControlStateStyleGroup
    {
        public static ControlStateStyleGroup Create(int id, Func<bool> condition, ControlStateStyles styleWhenTrue, ControlStateStyles styleWhenFalse, params BaseEdit[] controls)
        {
            return new ControlStateStyleGroup(id, condition, styleWhenTrue, styleWhenFalse, controls);
        }

        public ControlStateStyleGroup(int id, Func<bool> condition, ControlStateStyles styleWhenTrue, ControlStateStyles styleWhenFalse, params BaseEdit[] controls)
        {
            Condition = condition;
            Controls = controls.ToList();
            StyleWhenTrue = styleWhenTrue;
            StyleWhenFalse = styleWhenFalse;
            GroupId = id;
        }

        public int GroupId { get; set; }

        public Func<bool> Condition { get; set; }
        public List<BaseEdit> Controls { get; set; }

        public ControlStateStyles StyleWhenTrue { get; set; }
        public ControlStateStyles StyleWhenFalse { get; set; }

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
