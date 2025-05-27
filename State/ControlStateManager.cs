using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;

namespace SoftwareMeesters.Windows.UI.State
{
    public class ControlStateManager<T>
    {
        List<ControlStateGroup<T>> groups;

        public static ControlStateManager<T> Create() => new ControlStateManager<T>();

        public ControlStateManager()
        {
            groups = new List<ControlStateGroup<T>>();
        }

        public void AddGroup(Func<bool> condition, ControlStateTypes stateType = ControlStateTypes.Enabled, params T[] controls)
        {
            groups.Add(ControlStateGroup<T>.Create(groups.Count + 1, condition, stateType, controls));
        }


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

        public void SetState()
        {
            foreach (var g in groups)
            {
                SetState(g.GroupId);
            }
        }
    }
}
