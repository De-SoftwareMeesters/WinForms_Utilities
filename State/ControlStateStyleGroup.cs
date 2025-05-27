/// <summary>
/// Represents a group of DevExpress controls with conditional style logic.
/// </summary>
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Represents a group of <see cref="BaseEdit"/> controls whose style is conditionally determined at runtime.
    /// </summary>
    public class ControlStateStyleGroup
    {
        /// <summary>
        /// Creates a new <see cref="ControlStateStyleGroup"/> with the specified condition and styles.
        /// </summary>
        /// <param name="id">The unique identifier of the group.</param>
        /// <param name="condition">The function used to evaluate the group's condition.</param>
        /// <param name="styleWhenTrue">The style to apply when the condition evaluates to <c>true</c>.</param>
        /// <param name="styleWhenFalse">The style to apply when the condition evaluates to <c>false</c>.</param>
        /// <param name="controls">The DevExpress <see cref="BaseEdit"/> controls included in the group.</param>
        /// <returns>A new instance of <see cref="ControlStateStyleGroup"/>.</returns>
        public static ControlStateStyleGroup Create(int id, Func<bool> condition, ControlStateStyles styleWhenTrue, ControlStateStyles styleWhenFalse, params BaseEdit[] controls)
        {
            return new ControlStateStyleGroup(id, condition, styleWhenTrue, styleWhenFalse, controls);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStateStyleGroup"/> class.
        /// </summary>
        /// <param name="id">The group identifier.</param>
        /// <param name="condition">The condition that determines which style to apply.</param>
        /// <param name="styleWhenTrue">The style to apply when the condition is true.</param>
        /// <param name="styleWhenFalse">The style to apply when the condition is false.</param>
        /// <param name="controls">The list of controls included in the group.</param>
        public ControlStateStyleGroup(int id, Func<bool> condition, ControlStateStyles styleWhenTrue, ControlStateStyles styleWhenFalse, params BaseEdit[] controls)
        {
            Condition = condition;
            Controls = controls.ToList();
            StyleWhenTrue = styleWhenTrue;
            StyleWhenFalse = styleWhenFalse;
            GroupId = id;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the group.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the condition that determines which style should be applied.
        /// </summary>
        public Func<bool> Condition { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="BaseEdit"/> controls in this group.
        /// </summary>
        public List<BaseEdit> Controls { get; set; }

        /// <summary>
        /// Gets or sets the style to apply when the condition is <c>true</c>.
        /// </summary>
        public ControlStateStyles StyleWhenTrue { get; set; }

        /// <summary>
        /// Gets or sets the style to apply when the condition is <c>false</c>.
        /// </summary>
        public ControlStateStyles StyleWhenFalse { get; set; }

        /// <summary>
        /// Evaluates the condition asynchronously with a 1-second timeout.
        /// </summary>
        /// <returns><c>true</c> if the condition returns true within the timeout; otherwise, <c>false</c>.</returns>
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
