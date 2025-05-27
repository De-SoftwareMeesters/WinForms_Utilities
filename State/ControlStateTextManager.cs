using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareMeesters.Windows.UI.State
{
    public class ControlStateTextManager<T>
    {
        List<ControlStateTextGroup<T>> groups;
        public static ControlStateTextManager<T> Create() => new ControlStateTextManager<T>();
        public ControlStateTextManager()
        {
            groups = new List<ControlStateTextGroup<T>>();
        }

        public void AddGroup(Func<bool> condition, ControlStateTexts textWhenTrue, ControlStateTexts textWhenFalse, Action actionWhenTrue, Action actionWhenFalse, params T[] controls)
        {
            groups.Add(ControlStateTextGroup<T>.Create(groups.Count + 1, condition, textWhenTrue, textWhenFalse, actionWhenTrue, actionWhenFalse, controls));
        }
        public void SetState(int groupId)
        {
            var g = groups.SingleOrDefault(x => x.GroupId == groupId);
            if (g == null) return;

            var state = g.IsTrue();
            foreach (var c in g.Controls)
            {
                if (state)
                {
                    switch (g.TextWhenTrue)
                    {
                        case ControlStateTexts.Edit:
                            Set(c, "Bewerken");
                            break;
                        case ControlStateTexts.Save:
                            Set(c, "Opslaan");
                            break;
                        case ControlStateTexts.Cancel:
                            Set(c, "Annuleren");
                            break;
                        default:
                            Set(c, "Bewerken");
                            break;
                    }

                    try
                    {
                        if (g.ActionWhenTrue != null)
                            g.ActionWhenTrue.Invoke();
                    }
                    catch { }
                }
                else
                {
                    switch (g.TextWhenFalse)
                    {
                        case ControlStateTexts.Edit:
                            Set(c, "Bewerken");
                            break;
                        case ControlStateTexts.Save:
                            Set(c, "Opslaan");
                            break;
                        case ControlStateTexts.Cancel:
                            Set(c, "Annuleren");
                            break;
                        default:
                            Set(c, "Bewerken");
                            break;
                    }

                    try
                    {
                        if (g.ActionWhenFalse != null)
                            g.ActionWhenFalse.Invoke();
                    }
                    catch { }
                }
            }
        }


        void Set(T c, string text)
        {
            if (c is Control)
                c.GetType().GetProperty("Text").SetValue(c, text);
            if (c is BarItem)
                c.GetType().GetProperty("Caption").SetValue(c, text);
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
