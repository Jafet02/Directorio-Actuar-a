using DirecAct.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DirecAct.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels
        /*public LoginViewModel Login
        {
            get;
            set;
        }
        */
        public HomeViewModel Home
        {
            get;
            set;
        }
        public MenuViewModel Menu
        {
            get;
            set;
        }

        public AreasViewModel Areas
        {
            get;
            set;
        }
        public NamesViewModel Names
        {
            get;
            set;
        }
        public ContactViewModel Contact
        {
            get;
            set;
        }
        public DependenciesViewModel Dependence
        {
            get;
            set;
        }
        public NotifyViewModel Notify
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            //Cambió de    this.Menu = new MenuViewModel();    a:
            this.Home = new HomeViewModel();
        }
        #endregion

        #region Singleyton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        public void RegisterDevice()
        {
            var register = DependencyService.Get<IRegisterDevice>();
            register.RegisterDevice();
        }
        #endregion
    }
}
