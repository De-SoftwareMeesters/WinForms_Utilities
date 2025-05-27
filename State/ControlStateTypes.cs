/// <summary>
/// Enum defining control states such as Enabled or Visible.
/// </summary>
namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Specifies the type of state that can be conditionally applied to a control.
    /// </summary>
    public enum ControlStateTypes
    {
        /// <summary>
        /// Indicates whether the control is enabled (can receive input).
        /// </summary>
        Enabled = 0,

        /// <summary>
        /// Indicates whether the control is visible (shown or hidden).
        /// </summary>
        Visible = 1
    }
}
