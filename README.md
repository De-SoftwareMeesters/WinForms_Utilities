# WinForms Utilities

A collection of utility classes to simplify Windows Forms development with DevExpress.  
This library helps manage UI control states, styles, and dynamic texts in a clean and structured way.

> 💡 Built for [DevExpress WinForms controls](https://www.devexpress.com/products/net/controls/winforms/), but extensible to other control libraries.

---

## Features

✅ Group-based control state management  
✅ Centralized style switching (ReadOnly, Editable, Disabled)  
✅ Contextual UI text updates (e.g., "Edit" ↔ "Save")  
✅ Declarative logic via conditions (`Func<bool>`)  
✅ Works with `Control`, `BarItem`, `BaseEdit`, `GridColumn`, etc.

---

## Installation

Install via source (no NuGet package yet):

```bash
git clone https://github.com/De-SoftwareMeesters/winforms-utilities.git
````

Then include the relevant `.cs` files from the `Shared.Windows.UI` namespace in your project.

---

## Usage Example

```csharp
var stateManager = ControlStateManager<Control>.Create();
stateManager.AddGroup(
    condition: () => isEditMode,
    stateType: ControlStateTypes.Enabled,
    controls: new[] { txtName, txtEmail, btnSave }
);

// Dynamically toggle state based on condition
stateManager.SetState();
```

Or use the `ControlStateTextManager` to update button text with action bindings:

```csharp
var textManager = ControlStateTextManager<Button>.Create();
textManager.AddGroup(
    condition: () => isEditMode,
    textWhenTrue: ControlStateTexts.Save,
    textWhenFalse: ControlStateTexts.Edit,
    actionWhenTrue: SaveChanges,
    actionWhenFalse: EnterEditMode,
    controls: new[] { btnToggleEdit }
);

textManager.SetState();
```

---

## Components

| Component                    | Purpose                                                              |
| ---------------------------- | -------------------------------------------------------------------- |
| `ControlStateManager<T>`     | Toggles `Enabled` or `Visible` state of grouped controls             |
| `ControlStateStyleManager`   | Applies styles like `ReadOnly`, `Editable`, or `Disabled`            |
| `ControlStateTextManager<T>` | Updates control captions (e.g., `Text`, `Caption`) and binds actions |
| `ControlStateGroup<T>`       | Encapsulates conditionally-controlled controls                       |
| `ControlStateTypes`          | Enum for `Enabled` / `Visible`                                       |
| `ControlStateStyles`         | Enum for `Editable` / `ReadOnly` / `Disabled`                        |
| `ControlStateTexts`          | Enum for `Edit` / `Save` / `Cancel`                                  |

---

## DevExpress Compatibility

This library is designed and tested with DevExpress WinForms components, including:

* `BarItem`
* `BaseEdit`
* `GridColumn`
* Standard `Control`

---

## License

[MIT License](LICENSE)

---

## Maintained by

**De SoftwareMeesters**
[https://github.com/De-SoftwareMeesters](https://github.com/De-SoftwareMeesters)

