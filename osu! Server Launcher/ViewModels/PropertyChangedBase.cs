using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace osuserverlauncher.ViewModels;
public abstract class PropertyChangedBase : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler PropertyChanged;
  private SynchronizationContext m_context;

  public PropertyChangedBase()
  {
    m_context = SynchronizationContext.Current;
  }

  public void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
  {
    if (EqualityComparer<T>.Default.Equals(field, value))
      return;

    field = value;
    m_context.Post(_ => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)), null);
  }
}
