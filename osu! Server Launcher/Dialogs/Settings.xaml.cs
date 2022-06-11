using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using osuserverlauncher.Infrastructure;
using osuserverlauncher.Models;

// To learn more about osuserverlauncher, the osuserverlauncher project structure,
// and more about our project templates, see: http://aka.ms/osuserverlauncher-project-info.

namespace osuserverlauncher.Dialogs;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsDialog : ContentDialog
{
  public SettingsDialog()
  {
    this.InitializeComponent();
  }
}
