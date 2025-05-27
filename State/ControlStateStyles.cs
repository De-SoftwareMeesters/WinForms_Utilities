/// <summary>
/// Enum defining the style states for DevExpress controls.
/// </summary>
namespace SoftwareMeesters.Windows.UI.State
{
    /// <summary>
    /// Specifies the possible style states for a control.
    /// </summary>
    public enum ControlStateStyles
    {
        /// <summary>
        /// The control is disabled and cannot be interacted with.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// The control is read-only and its contents cannot be modified.
        /// </summary>
        ReadOnly = 1,

        /// <summary>
        /// The control is editable and can be modified by the user.
        /// </summary>
        Editable = 2,
    }
}
