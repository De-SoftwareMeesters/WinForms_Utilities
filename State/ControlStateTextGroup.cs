using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SoftwareMeesters.Windows.UI.State
{
    public class ControlStateTextGroup<T>
    {
        public static ControlStateTextGroup<T> Create(int id, Func<bool> condition, ControlStateTexts textWhenTrue, ControlStateTexts textWhenFalse,
            Action actionWhenTrue, Action actionWhenFalse, params T[] controls)
        {
            return new ControlStateTextGroup<T>(id, condition, textWhenTrue, textWhenFalse, actionWhenTrue, actionWhenFalse, controls);
        }

        public ControlStateTextGroup(int id, Func<bool> condition, ControlStateTexts textWhenTrue, ControlStateTexts textWhenFalse,
            Action actionWhenTrue, Action actionWhenFalse, params T[] controls)
        {
            Condition = condition;
            Controls = controls.ToList<T>();
            TextWhenTrue = textWhenTrue;
            TextWhenFalse = textWhenFalse;
            ActionWhenTrue = actionWhenTrue;
            ActionWhenFalse = actionWhenFalse;
            GroupId = id;
        }

        public int GroupId { get; set; }


        public Func<bool> Condition { get; set; }
        public List<T> Controls { get; set; }

        public Action ActionWhenTrue { get; set; }
        public Action ActionWhenFalse { get; set; }

        public ControlStateTexts TextWhenTrue { get; set; }
        public ControlStateTexts TextWhenFalse { get; set; }

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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
