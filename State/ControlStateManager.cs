/// <summary>
/// Manages the state (Enabled/Visible) of a group of WinForms or DevExpress controls based on conditions.
/// </summary>
using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Provides functionality to manage the enabled or visible state of controls of type <typeparamref name="T"/>
    /// based on runtime-evaluated conditions.
    /// </summary>
    /// <typeparam name="T">The control type (e.g., Control, BarItem, GridColumn) being managed.</typeparam>
    public class ControlStateManager<T>
    {
        /// <summary>
        /// The internal list of control state groups.
        /// </summary>
        private List<ControlStateGroup<T>> groups;

        /// <summary>
        /// Creates a new instance of <see cref="ControlStateManager{T}"/>.
        /// </summary>
        /// <returns>A new <see cref="ControlStateManager{T}"/> object.</returns>
        public static ControlStateManager<T> Create() => new ControlStateManager<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStateManager{T}"/> class.
        /// </summary>
        public ControlStateManager()
        {
            groups = new List<ControlStateGroup<T>>();
        }

        /// <summary>
        /// Adds a new group of controls with a condition that determines their state.
        /// </summary>
        /// <param name="condition">A delegate that returns <c>true</c> or <c>false</c> to determine the state of the controls.</param>
        /// <param name="stateType">The type of state to manage (Enabled or Visible).</param>
        /// <param name="controls">The controls to include in the group.</param>
        public void AddGroup(Func<bool> condition, ControlStateTypes stateType = ControlStateTypes.Enabled, params T[] controls)
        {
            groups.Add(ControlStateGroup<T>.Create(groups.Count + 1, condition, stateType, controls));
        }

        /// <summary>
        /// Applies the condition and sets the state for the specified control group.
        /// </summary>
        /// <param name="groupId">The ID of the group to update.</param>
        public void SetState(int groupId)
        {
            var g = groups.SingleOrDefault(x => x.GroupId == groupId);
            if (g == null) return;

            var state = g.IsTrue();
            foreach (var c in g.Controls)
            {
                switch (g.StateType)
                {
                    case ControlStateTypes.Enabled:
                        if (c is Control || c is BarItem)
                            c.GetType().GetProperty("Enabled").SetValue(c, state);
                        break;

                    case ControlStateTypes.Visible:
                        if (c is BarItem)
                            c.GetType().GetProperty("Visibility").SetValue(c, state ? BarItemVisibility.Always : BarItemVisibility.Never);
                        if (c is Control)
                            c.GetType().GetProperty("Visible").SetValue(c, state);
                        if (c is GridColumn)
                            c.GetType().GetProperty("Visible").SetValue(c, state);
                        break;

                    default:
                        if (c is Control || c is BarItem)
                            c.GetType().GetProperty("Enabled").SetValue(c, state);
                        break;
                }
            }
        }

        /// <summary>
        /// Applies all conditions and updates the state for every registered control group.
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
