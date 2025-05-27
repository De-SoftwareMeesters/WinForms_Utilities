/// <summary>
/// Represents a group of controls whose state is conditionally controlled.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Represents a group of controls of type <typeparamref name="T"/> whose enabled or visible state
    /// is controlled by a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of controls in the group.</typeparam>
    public class ControlStateGroup<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ControlStateGroup{T}"/>.
        /// </summary>
        /// <param name="id">The unique identifier for the control group.</param>
        /// <param name="condition">The condition that determines the state of the controls.</param>
        /// <param name="stateType">The type of state to control (e.g. Enabled or Visible). Defaults to <see cref="ControlStateTypes.Enabled"/>.</param>
        /// <param name="controls">The controls to include in this group.</param>
        /// <returns>A new <see cref="ControlStateGroup{T}"/> instance.</returns>
        public static ControlStateGroup<T> Create(int id, Func<bool> condition, ControlStateTypes stateType = ControlStateTypes.Enabled, params T[] controls)
        {
            return new ControlStateGroup<T>(id, condition, stateType, controls);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStateGroup{T}"/> class.
        /// </summary>
        /// <param name="id">The group identifier.</param>
        /// <param name="condition">The function that determines whether the group condition is met.</param>
        /// <param name="stateType">The type of state affected by the condition (e.g., Enabled or Visible).</param>
        /// <param name="controls">The list of controls that belong to the group.</param>
        public ControlStateGroup(int id, Func<bool> condition, ControlStateTypes stateType = ControlStateTypes.Enabled, params T[] controls)
        {
            Condition = condition;
            Controls = controls.ToList<T>();
            StateType = stateType;
            GroupId = id;
        }

        /// <summary>
        /// Gets or sets the unique identifier for this control group.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the condition function that determines the control state.
        /// </summary>
        public Func<bool> Condition { get; set; }

        /// <summary>
        /// Gets or sets the list of controls included in the group.
        /// </summary>
        public List<T> Controls { get; set; }

        /// <summary>
        /// Gets or sets the type of state (e.g., Enabled, Visible) affected by the condition.
        /// </summary>
        public ControlStateTypes StateType { get; set; }

        /// <summary>
        /// Evaluates the condition asynchronously with a 1-second timeout.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the condition evaluates to true within the timeout; otherwise, <c>false</c>.
        /// </returns>
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
            catch
            {
                return false;
            }
        }
    }
}
