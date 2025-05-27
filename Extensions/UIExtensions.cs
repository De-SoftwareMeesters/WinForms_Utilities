using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using DevExpress.XtraSplashScreen;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SoftwareMeesters.Windows.UI
{
    public static class UIExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lue"></param>
        /// <param name="dropdownRows">Number of rows the dropdown</param>
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

        public static void ApplyInlineLabel(this TextEdit edit, string title)
        {
            edit.Properties.AdvancedModeOptions.Label = title;
            edit.Properties.UseAdvancedMode = DevExpress.Utils.DefaultBoolean.True;
        }

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


        public static Color ControlEnter(this Control control)
        {
            Color _retval = control.BackColor;
            control.BackColor = Color.FromArgb(197, 251, 253);
            return _retval;
        }

        public static void ControlLeave(this Control control, Color originalBackColor = default(Color))
        {

            control.BackColor = originalBackColor == default(Color) ? Color.White : originalBackColor;
        }

        public static void PaintRound(this PanelControl panel, PaintEventArgs e, Color fillColor)
        {
            try
            {
                GraphicsPath path = RoundedRectangle.Create(2, 2, panel.Width - 1, panel.Height - 1, 20, RoundedRectangle.RectangleCorners.BottomRight);
                var brush = new SolidBrush(fillColor);

                e.Graphics.FillPath(brush, path);
            }
            catch { }
        }

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

        public static void ShowWait(this Form form)
        {
            SplashScreenManager.ShowForm(form, typeof(MyWaitForm), true, true, false);
        }
        public static void ShowWait(this Control form)
        {
            SplashScreenManager.ShowForm(typeof(MyWaitForm), true, true);
        }


        public static void HideWait(this Form form)
        {
            try
            {
                SplashScreenManager.CloseForm(false);
            }
            catch { }
        }

        public static void HideWait(this Control form)
        {
            try
            {
                SplashScreenManager.CloseForm(false);
            }
            catch { }
        }

        public static void WaitText(this Form form, string text)
        {
            try
            {
                SplashScreenManager.Default.SetWaitFormDescription(text);
            }
            catch { }
        }

        public static void WaitText(this Control form, string text)
        {
            try
            {
                SplashScreenManager.Default.SetWaitFormDescription(text);
            }
            catch { }
        }



    }
}
