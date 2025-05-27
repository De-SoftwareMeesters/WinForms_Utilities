/// <summary>
/// Contains extension methods for safe UI thread invocation and control helpers.
/// </summary>
using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SoftwareMeesters.Windows.UI
{
    /// <summary>
    /// Provides helper methods for WinForms controls, including thread-safe invocation,
    /// appearance handling, and control configuration.
    /// </summary>
    public static class UIExtensions
    {
        /// <summary>
        /// Applies default dropdown and prompt settings to a <see cref="LookUpEdit"/>.
        /// </summary>
        /// <param name="edit">The <see cref="LookUpEdit"/> control to configure.</param>
        /// <param name="dropdownRows">The number of dropdown rows to show.</param>
        /// <param name="title">Optional label to show above the control (advanced mode).</param>
        /// <param name="nullValuePrompt">Optional prompt to show when no value is selected.</param>
        public static void ApplyDefaultSettings(this LookUpEdit edit, int dropdownRows, string title = "", string nullValuePrompt = "")
        {
            edit.Properties.ShowHeader = false;
            edit.Properties.DropDownRows = dropdownRows;
            if (title != "")
            {
                edit.Properties.AdvancedModeOptions.Label = title;
                edit.Properties.UseAdvancedMode = DevExpress.Utils.DefaultBoolean.True;
            }
            if (nullValuePrompt != "")
            {
                edit.Properties.NullValuePrompt = null;
                edit.Properties.ShowNullValuePrompt = ShowNullValuePromptOptions.Default;
            }
        }

        /// <summary>
        /// Applies an inline label to a <see cref="TextEdit"/> control using DevExpress advanced mode.
        /// </summary>
        /// <param name="edit">The <see cref="TextEdit"/> control to update.</param>
        /// <param name="title">The label to display above the control.</param>
        public static void ApplyInlineLabel(this TextEdit edit, string title)
        {
            edit.Properties.AdvancedModeOptions.Label = title;
            edit.Properties.UseAdvancedMode = DevExpress.Utils.DefaultBoolean.True;
        }

        /// <summary>
        /// Executes the specified action on the control's UI thread if required.
        /// </summary>
        /// <param name="control">The control to act upon.</param>
        /// <param name="action">The delegate to invoke.</param>
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Changes the background color of a control to indicate focus, and returns the original color.
        /// </summary>
        /// <param name="control">The control to highlight.</param>
        /// <returns>The original background color before the change.</returns>
        public static Color ControlEnter(this Control control)
        {
            Color _retval = control.BackColor;
            control.BackColor = Color.FromArgb(197, 251, 253); // Light cyan
            return _retval;
        }

        /// <summary>
        /// Restores the background color of a control to its original value.
        /// </summary>
        /// <param name="control">The control to restore.</param>
        /// <param name="originalBackColor">The color to restore; if omitted, defaults to white.</param>
        public static void ControlLeave(this Control control, Color originalBackColor = default(Color))
        {
            control.BackColor = originalBackColor == default(Color) ? Color.White : originalBackColor;
        }

        /// <summary>
        /// Tries to execute an action on an object, returning <c>true</c> if successful, <c>false</c> if an exception occurs.
        /// </summary>
        /// <param name="o">The object context (not used but allows chaining).</param>
        /// <param name="action">The action to execute.</param>
        /// <returns><c>true</c> if the action executed without error; otherwise, <c>false</c>.</returns>
        public static bool TrySet(this object o, Action action)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
