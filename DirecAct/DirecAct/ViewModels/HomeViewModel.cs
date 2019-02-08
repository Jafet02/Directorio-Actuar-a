using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using Services;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class HomeViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isEnableHome;
        private bool isEnableCarrera;
        private bool isVisibleCarrera;

        private string carrera;
        #endregion

        #region Propertyes
        public string Carrera
        {
            //Esto gracias a la BaseViewModel
            get { return this.carrera; }
            set { SetValue(ref this.carrera, value); }
        }
        public bool IsEnableHome
        {
            //Esto gracias a la BaseViewModel
            get { return this.isEnableHome; }
            set { SetValue(ref this.isEnableHome, value); }
        }
        public bool IsEnableCarrera
        {
            //Esto gracias a la BaseViewModel
            get { return this.isEnableCarrera; }
            set { SetValue(ref this.isEnableCarrera, value); }
        }
        public bool IsVisibleCarrera
        {
            //Esto gracias a la BaseViewModel
            get { return this.isVisibleCarrera; }
            set { SetValue(ref this.isVisibleCarrera, value); }
        }
        #endregion

        #region Constructor
        public HomeViewModel()
        {
            this.IsEnableHome = true;
            this.apiService = new ApiService();

            if (!Application.Current.Properties.ContainsKey("Idcarrera"))
            {
                this.IsEnableCarrera = true;
                this.IsVisibleCarrera = true;
            }
            else
            {
                this.IsEnableCarrera = false;
                this.IsVisibleCarrera = false;
            }
            
            
        }
        #endregion

        #region Command
        public ICommand Menu_Command
        {
            get
            {
                return new RelayCommand(Start_menu);
            }
        }
        public ICommand RegisterC_Command
        {
            get
            {
                return new RelayCommand(Register_carrera);
            }
        }
        #endregion

        #region Functions
        private async void Start_menu()
        {
            this.IsEnableHome = false;
            
            //Revisar conexión si no, no enviar por el registro aal servicion de notificaciones
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnableHome = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Directorio Acatlán",
                    connection.Message,
                    "OK");

                return;
            }

            MainViewModel.GetInstance().Menu = new MenuViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new MenuPage());
            
            this.IsEnableHome = true;

            return;
        }

        private async void Register_carrera()
        {
            try
            {
                //Revisar conexión para garantizar el registro a las notificaciones
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsEnableHome = true;

                    await Application.Current.MainPage.DisplayAlert(
                        "Directorio Acatlán",
                        connection.Message,
                        "OK");

                    return;
                }


                string res;

                res = await Application.Current.MainPage.DisplayActionSheet(
                "Selecciona tu carrera",
                null,
                "", "Actuaría" /*, "Ingenieria civíl", "Matemáticas Aplicadas y Computación" */);

                if (string.IsNullOrEmpty(res))
                {
                    return;
                }

                //Especificar area con ID      cn  modelo????????
                string rescarrera = CarreraID(res);

                Application.Current.Properties["Idcarrera"] = rescarrera;


                var mainViewModel = MainViewModel.GetInstance();
                this.Carrera = rescarrera;
                //mainViewModel.Menu.Carrera = rescarrera;
                mainViewModel.RegisterDevice();



                await Application.Current.MainPage.DisplayAlert(
                        "Directorio Acatlán",
                        "Se ha suscrito a las notificaciones emitidas para " + res + ".",
                        "OK");

                //Desactivar botón Carrera
                this.IsEnableCarrera = false;
                this.IsVisibleCarrera = false;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Directorio Acatlán",
                        "Ocurrió un error intente más tarde.",
                        "OK");
            }
            

        }
        private string CarreraID(string carrera)
        {
            switch (carrera)
            {
                case "Actuaría":
                    return "actuaria";
                case "Ingenieria civíl":
                    return "ingenieriacivil";
                case "Matemáticas Aplicadas y Computación":
                    return "mac";


                default: return "singrupo";
            }

        }
        #endregion
    }
}
