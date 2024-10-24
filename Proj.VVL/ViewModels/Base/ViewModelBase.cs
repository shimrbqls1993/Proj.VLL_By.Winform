using CommunityToolkit.Mvvm.ComponentModel;
using Proj.VVL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.ViewModels.Base
{
    public class ViewModelBase : ObservableObject
    {
        string _Text = string.Empty;
        public string Text
        {
            get { return _Text; }
            set
            {
                SetProperty(ref _Text, value);
            }
        }
    }
}
