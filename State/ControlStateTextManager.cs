/// <summary>
/// Updates the displayed text of controls conditionally and binds actions to those states.
/// </summary>
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Manages the text and behavior of controls of type <typeparamref name="T"/> based on conditionally evaluated logic.
    /// </summary>
    /// <typeparam name="T">The type of control being managed (e.g., <see cref="Button"/>, <see cref="BarItem"/>).</typeparam>
    public class ControlStateTextManager<T>
    {
        /// <summary>
        /// Internal list of text groups managed by this manager.
        /// </summary>
        private List<ControlStateTextGroup<T>> groups;

        /// <summary>
        /// Creates a new instance of <see cref="ControlStateTextManager{T}"/>.
        /// </summary>
        /// <returns>A new <see cref="ControlStateTextManager{T}"/> instance.</returns>
        public static ControlStateTextManager<T> Create() => new ControlStateTextManager<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStateTextManager{T}"/> class.
        /// </summary>
        public ControlStateTextManager()
        {
            groups = new List<ControlStateTextGroup<T>>();
        }

        /// <summary>
        /// Adds a new group of controls with conditional text and behavior.
        /// </summary>
        /// <param name="condition">Function to evaluate the condition.</param>
        /// <param name="textWhenTrue">The text to display when the condition is <c>true</c>.</param>
        /// <param name="textWhenFalse">The text to display when the condition is <c>false</c>.</param>
        /// <param name="actionWhenTrue">The action to execute when the condition is <c>true</c>.</param>
        /// <param name="actionWhenFalse">The action to execute when the condition is <c>false</c>.</param>
        /// <param name="controls">The controls whose text and behavior will be managed.</param>
        public void AddGroup(Func<bool> condition, ControlStateTexts textWhenTrue, ControlStateTexts textWhenFalse, Action actionWhenTrue, Action actionWhenFalse, params T[] controls)
        {
            groups.Add(ControlStateTextGroup<T>.Create(groups.Count + 1, condition, textWhenTrue, textWhenFalse, actionWhenTrue, actionWhenFalse, controls));
        }

        /// <summary>
        /// Applies the condition, sets the text, and executes the action for the specified group.
        /// </summary>
        /// <param name="groupId">The ID of the group to evaluate.</param>
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
                        g.ActionWhenTrue?.Invoke();
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
                        g.ActionWhenFalse?.Invoke();
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Sets the specified text to the control based on its type (e.g., Control.Text or BarItem.Caption).
        /// </summary>
        /// <param name="c">The control to update.</param>
        /// <param name="text">The text to assign.</param>
        private void Set(T c, string text)
        {
            if (c is Control)
                c.GetType().GetProperty("Text")?.SetValue(c, text);
            if (c is BarItem)
                c.GetType().GetProperty("Caption")?.SetValue(c, text);
        }

        /// <summary>
        /// Applies state logic to all registered groups.
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
