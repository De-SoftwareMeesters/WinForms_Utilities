/// <summary>
/// Manages control styles such as Editable, ReadOnly, and Disabled using DevExpress BaseEdit controls.
/// </summary>
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Provides functionality to apply visual and interaction styles (Editable, ReadOnly, Disabled) 
    /// to a group of <see cref="BaseEdit"/> controls based on evaluated conditions.
    /// </summary>
    public class ControlStateStyleManager
    {
        /// <summary>
        /// Internal list of style groups managed by this instance.
        /// </summary>
        private List<ControlStateStyleGroup> groups;

        /// <summary>
        /// Creates a new instance of <see cref="ControlStateStyleManager"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ControlStateStyleManager"/>.</returns>
        public static ControlStateStyleManager Create() => new ControlStateStyleManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStateStyleManager"/> class.
        /// </summary>
        public ControlStateStyleManager()
        {
            groups = new List<ControlStateStyleGroup>();
        }

        /// <summary>
        /// Adds a group of controls that will have their style updated based on a condition.
        /// </summary>
        /// <param name="condition">Function that determines the current state (true/false).</param>
        /// <param name="styleWhenTrue">The style to apply when the condition is true.</param>
        /// <param name="styleWhenFalse">The style to apply when the condition is false.</param>
        /// <param name="controls">The <see cref="BaseEdit"/> controls to manage.</param>
        public void AddGroup(Func<bool> condition, ControlStateStyles styleWhenTrue, ControlStateStyles styleWhenFalse, params BaseEdit[] controls)
        {
            groups.Add(ControlStateStyleGroup.Create(groups.Count + 1, condition, styleWhenTrue, styleWhenFalse, controls));
        }

        /// <summary>
        /// Applies the appropriate style to the specified group of controls based on the condition's result.
        /// </summary>
        /// <param name="groupId">The identifier of the group to update.</param>
        public void SetState(int groupId)
        {
            var g = groups.SingleOrDefault(x => x.GroupId == groupId);
            if (g == null) return;

            var state = g.IsTrue();
            foreach (var c in g.Controls)
            {
                // Reset visual appearance before applying style
                c.Properties.AppearanceReadOnly.BackColor = SystemColors.Control;
                c.BackColor = Color.LemonChiffon;

                if (state)
                {
                    switch (g.StyleWhenTrue)
                    {
                        case ControlStateStyles.Disabled:
                            SetDisabled(c);
                            break;
                        case ControlStateStyles.ReadOnly:
                            SetReadOnly(c);
                            break;
                        case ControlStateStyles.Editable:
                            SetEditable(c);
                            break;
                        default:
                            SetReadOnly(c);
                            break;
                    }
                }
                else
                {
                    switch (g.StyleWhenFalse)
                    {
                        case ControlStateStyles.Disabled:
                            SetDisabled(c);
                            break;
                        case ControlStateStyles.ReadOnly:
                            SetReadOnly(c);
                            break;
                        case ControlStateStyles.Editable:
                            SetEditable(c);
                            break;
                        default:
                            SetReadOnly(c);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the control to read-only style: enabled but not editable, with no border.
        /// </summary>
        /// <param name="c">The control to modify.</param>
        private void SetReadOnly(BaseEdit c)
        {
            c.InvokeIfRequired(() =>
            {
                c.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                c.ReadOnly = true;
                c.Enabled = true;
            });
        }

        /// <summary>
        /// Sets the control to disabled style: not editable and not enabled, with no border.
        /// </summary>
        /// <param name="c">The control to modify.</param>
        private void SetDisabled(BaseEdit c)
        {
            c.InvokeIfRequired(() =>
            {
                c.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                c.ReadOnly = true;
                c.Enabled = false;
            });
        }

        /// <summary>
        /// Sets the control to editable style: enabled and editable with default border.
        /// </summary>
        /// <param name="c">The control to modify.</param>
        private void SetEditable(BaseEdit c)
        {
            c.InvokeIfRequired(() =>
            {
                c.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                c.ReadOnly = false;
                c.Enabled = true;
            });
        }

        /// <summary>
        /// Applies styles to all registered control groups.
        /// </summary>
        public void SetState()
        {
            foreach (var g in groups)
            {
                SetState(g.GroupId);
            }
        }
    }
}
