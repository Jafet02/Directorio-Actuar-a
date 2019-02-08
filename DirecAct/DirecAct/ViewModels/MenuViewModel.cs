using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;
    using Services;

    public class MenuViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isEnableArea;
        private bool isEnableName;
        private bool isRunning;
        private bool isEnableNotify;
        private bool isEnableSelect;
        private bool isVisibleSelect;
        private bool isVisibleLabelSelect;

        private string carrera;
        #endregion

        #region Properties
        public string Carrera
        {
            //Esto gracias a la BaseViewModel
            get { return this.carrera; }
            set { SetValue(ref this.carrera, value); }
        }
        public bool IsVisibleLabelSelect
        {
            //Esto gracias a la BaseViewModel
            get { return this.isVisibleLabelSelect; }
            set { SetValue(ref this.isVisibleLabelSelect, value); }
        }
        public bool IsVisibleSelect
        {
            //Esto gracias a la BaseViewModel
            get { return this.isVisibleSelect; }
            set { SetValue(ref this.isVisibleSelect, value); }
        }
        public bool IsEnableSelect
        {
            //Esto gracias a la BaseViewModel
            get { return this.isEnableSelect; }
            set { SetValue(ref this.isEnableSelect, value); }
        }
        public bool IsEnableNotify
        {
            //Esto gracias a la BaseViewModel
            get { return this.isEnableNotify; }
            set { SetValue(ref this.isEnableNotify, value); }
        }
        public bool IsEnableArea
        {
            //Esto gracias a la BaseViewModel
            get { return this.isEnableArea; }
            set { SetValue(ref this.isEnableArea, value); }
        }
        public bool IsEnableName
        {
            get { return this.isEnableName; }
            set { SetValue(ref this.isEnableName, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Constructors
        public MenuViewModel()
        {
            this.IsRunning = false;
            this.IsEnableArea = true;
            this.IsEnableName = true;
            this.IsEnableNotify = true;
            this.apiService = new ApiService();
            //this.Carrera = "1";

            //Debe ir un if si ya se guardo la carrera no mostrar el selecciona carrera
            /*
            if(!Application.Current.Properties.ContainsKey("Idcarrera"))
            {
                Select_group();
            }
            */
            //Mientras será único para Actuaría.......
            /*

            var mainViewModel = MainViewModel.GetInstance();
            //mainViewModel.Menu.Carrera = "1";
            mainViewModel.RegisterDevice();


            */
        }
        #endregion

        #region Command
        public ICommand B_AreaCommand
        {
            get
            {
                return new RelayCommand(SearchArea);
            }
        }
        public ICommand B_NameCommand
        {
            get
            {
                return new RelayCommand(SearchName);
            }
        }
        public ICommand Notify_Command
        {
            get
            {
                //Añadir pagina de notificaciones
                return new RelayCommand(HistoryNotify);
            }
        }
        //Es el comando paar suscribirse en el servicio de notificaciones
        /*
        public ICommand Group_Command
        {
            get
            {
                //Añadir pagina de notificaciones
                return new RelayCommand(Select_group);
            }
        }
        */
        #endregion

        #region Functions

        #region POculta
        //Se comentan las 2 funciones para suscribir al 
        //servicio de notificaciones despues del comndo Select_group y Carrera_ID
        /*
        public async void Select_group()
        {
            //Al seleccionar debe dessaparecer la opcion y guardar la respuesta para simepre
            
             // Tal vez guardar la respuesta en la mainviewmodel o donde 
             // Tambien se debe registrar el dispositivi al servicio de notificacioness
             // cambiar eso aqui.....
             //
             

            //Hacer un Do while con es Display
            string res;
            do
            {
                res = await Application.Current.MainPage.DisplayActionSheet(
                "Selecciona tu carrera",

                null,
                "", "Actuaría", "Ingenieria civíl", "Matemáticas Aplicadas y Computación");
            }
            while (string.IsNullOrEmpty(res));

            //Especificar area con ID      cn  modelo????????
            int rescarrera = CarreraID(res);

            Application.Current.Properties["Idcarrera"] = rescarrera;
            

            var mainViewModel = MainViewModel.GetInstance();
            //mainViewModel.Menu.Carrera = rescarrera;
            mainViewModel.RegisterDevice();



            await Application.Current.MainPage.DisplayAlert(
                    "Directorio Acatlán",
                    "Se ha suscrito a las notificaciones emitidas para " + res +".",
                    "OK");
            this.IsVisibleSelect = false;
            this.IsVisibleLabelSelect = false;
        }

        private int CarreraID(string carrera)
        {
            switch(carrera)
            {
                case "Actuaría":
                    return 1;
                case "Ingenieria civíl":
                    return 2;
                case "Matemáticas Aplicadas y Computación":
                    return 3;


                default: return 0;
            }
            
        }
            */
        #endregion

        private async void HistoryNotify()
        {
            this.IsRunning = true;
            this.IsEnableArea = false;
            this.IsEnableName = false;
            this.IsEnableNotify = false;
            this.IsEnableSelect = false;

            //Revisar conexión si no, ni enviar
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnableArea = true;
                this.IsEnableName = true;
                this.IsEnableNotify = true;
                this.IsEnableSelect = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Directorio Acatlán",
                    connection.Message,
                    "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            MainViewModel.GetInstance().Notify = new NotifyViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new NotifyPage());
            
            this.IsRunning = false;
            this.IsEnableArea = true;
            this.IsEnableName = true;
            this.IsEnableNotify = true;
            this.IsEnableSelect = true;

            return;
        }


        private async void SearchArea()
        {
            this.IsRunning = true;
            this.IsEnableArea = false;
            this.IsEnableName = false;
            this.IsEnableNotify = false;
            this.IsEnableSelect = false;
            //Revisar conexión si no, ni enviar
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnableArea = true;
                this.IsEnableName = true;
                this.IsEnableNotify = true;
                this.IsEnableSelect = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Directorio Acatlán",
                    connection.Message,
                    "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            MainViewModel.GetInstance().Areas = new AreasViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AreasPage());

            this.IsRunning = false;
            this.IsEnableArea = true;
            this.IsEnableName = true;
            this.IsEnableNotify = true;
            this.IsEnableSelect = true;

            return;
        }


        private async void SearchName()
        {
            this.IsRunning = true;
            this.IsEnableArea = false;
            this.IsEnableName = false;
            this.IsEnableNotify = false;
            this.IsEnableSelect = false;
            //Revisar conexión si no, ni enviar
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnableArea = true;
                this.IsEnableName = true;
                this.IsEnableNotify = true;
                this.IsEnableSelect = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Directorio Acatlán",
                    connection.Message,
                    "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            MainViewModel.GetInstance().Names = new NamesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new NamesPage());

            this.IsRunning = false;
            this.IsEnableArea = true;
            this.IsEnableName = true;
            this.IsEnableNotify = true;
            this.IsEnableSelect = true;

            return;
        }
        
        #endregion
        
    }
}
