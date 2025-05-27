using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareMeesters.Windows.UI.State
{
    public class ControlStateStyleManager
    {
        List<ControlStateStyleGroup> groups;
        public static ControlStateStyleManager Create() => new ControlStateStyleManager();
        public ControlStateStyleManager()
        {
            groups = new List<ControlStateStyleGroup>();
        }

        public void AddGroup(Func<bool> condition, ControlStateStyles styleWhenTrue, ControlStateStyles styleWhenFalse, params BaseEdit[] controls)
        {
            groups.Add(ControlStateStyleGroup.Create(groups.Count + 1, condition, styleWhenTrue, styleWhenFalse, controls));
        }
        public void SetState(int groupId)
        {
            var g = groups.SingleOrDefault(x => x.GroupId == groupId);
            if (g == null) return;

            var state = g.IsTrue();
            foreach (var c in g.Controls)
            {
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


        void SetReadOnly(BaseEdit c)
        {
            c.InvokeIfRequired(() =>
            {
                c.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                c.ReadOnly = true;
                c.Enabled = true;
            });
        }

        void SetDisabled(BaseEdit c)
        {
            c.InvokeIfRequired(() =>
            {
                c.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                c.ReadOnly = true;
                c.Enabled = false;
            });
        }

        void SetEditable(BaseEdit c)
        {
            c.InvokeIfRequired(() =>
            {
                c.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                c.ReadOnly = false;
                c.Enabled = true;
            });
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
