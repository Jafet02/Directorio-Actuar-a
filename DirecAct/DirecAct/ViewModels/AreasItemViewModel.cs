using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class AreasItemViewModel : Areas
    {
        #region Commands
        public ICommand SelectAreaCommand
        {
            get
            {
                return new RelayCommand(SelectArea);
            }
        }
        #endregion

        private async void SelectArea()
        {
            MainViewModel.GetInstance().Dependence = new DependenciesViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new DependenciesPage());
        }
    }
}
