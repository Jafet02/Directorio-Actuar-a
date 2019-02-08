﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class DependenciesItemViewModel : Funcionarios
    {
        #region Commands
        public ICommand SelectAdsripCommand
        {
            get
            {
                return new RelayCommand(SelectName);
            }
        }
        #endregion

        private async void SelectName()
        {
            MainViewModel.GetInstance().Contact = new ContactViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new ContactPage());
        }
    }
}
