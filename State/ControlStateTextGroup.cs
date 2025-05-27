/// <summary>
/// Represents a group of controls with conditional text and behavior.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Represents a group of controls of type <typeparamref name="T"/> whose displayed text and behavior
    /// are controlled based on a runtime-evaluated condition.
    /// </summary>
    /// <typeparam name="T">The type of controls managed in the group (e.g., <see cref="Button"/>, <see cref="Label"/>).</typeparam>
    public class ControlStateTextGroup<T>
    {
        /// <summary>
        /// Creates a new <see cref="ControlStateTextGroup{T}"/> instance.
        /// </summary>
        /// <param name="id">The unique identifier for the group.</param>
        /// <param name="condition">A function that returns <c>true</c> or <c>false</c> to evaluate the group's state.</param>
        /// <param name="textWhenTrue">The text content to use when the condition is <c>true</c>.</param>
        /// <param name="textWhenFalse">The text content to use when the condition is <c>false</c>.</param>
        /// <param name="actionWhenTrue">The action to execute when the condition is <c>true</c>.</param>
        /// <param name="actionWhenFalse">The action to execute when the condition is <c>false</c>.</param>
        /// <param name="controls">The controls whose text and behavior are managed by this group.</param>
        /// <returns>A new instance of <see cref="ControlStateTextGroup{T}"/>.</returns>
        public static ControlStateTextGroup<T> Create(int id, Func<bool> condition, ControlStateTexts textWhenTrue, ControlStateTexts textWhenFalse,
            Action actionWhenTrue, Action actionWhenFalse, params T[] controls)
        {
            return new ControlStateTextGroup<T>(id, condition, textWhenTrue, textWhenFalse, actionWhenTrue, actionWhenFalse, controls);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStateTextGroup{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier for the group.</param>
        /// <param name="condition">A function that evaluates whether the state is true or false.</param>
        /// <param name="textWhenTrue">Text to apply when the condition is true.</param>
        /// <param name="textWhenFalse">Text to apply when the condition is false.</param>
        /// <param name="actionWhenTrue">Action to execute when the condition is true.</param>
        /// <param name="actionWhenFalse">Action to execute when the condition is false.</param>
        /// <param name="controls">The list of controls managed by this group.</param>
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

        /// <summary>
        /// Gets or sets the unique identifier for this control group.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the condition function that determines which text and behavior apply.
        /// </summary>
        public Func<bool> Condition { get; set; }

        /// <summary>
        /// Gets or sets the list of controls managed in this group.
        /// </summary>
        public List<T> Controls { get; set; }

        /// <summary>
        /// Gets or sets the action to execute when the condition is <c>true</c>.
        /// </summary>
        public Action ActionWhenTrue { get; set; }

        /// <summary>
        /// Gets or sets the action to execute when the condition is <c>false</c>.
        /// </summary>
        public Action ActionWhenFalse { get; set; }

        /// <summary>
        /// Gets or sets the text to apply to the controls when the condition is <c>true</c>.
        /// </summary>
        public ControlStateTexts TextWhenTrue { get; set; }

        /// <summary>
        /// Gets or sets the text to apply to the controls when the condition is <c>false</c>.
        /// </summary>
        public ControlStateTexts TextWhenFalse { get; set; }

        /// <summary>
        /// Evaluates the condition asynchronously with a 1-second timeout.
        /// </summary>
        /// <returns><c>true</c> if the condition evaluates to true within the timeout; otherwise, <c>false</c>.</returns>
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
