using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using Services;
    using Models;
    using Xamarin.Forms;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    public class NotifyViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion
        //private ApiService apiServices;
        #region Attributes
        private ObservableCollection<Notificaciones> lista_completa;
        private bool isRefreshing;
        //private List<Notificaciones> notificationListaux;
        #endregion

        #region Properties
        public ObservableCollection<Notificaciones> Lista_completa
        {
            //Esto gracias a la BaseViewModel
            get { return this.lista_completa; }
            set { SetValue(ref this.lista_completa, value); }
        }
        public bool IsRefreshing
        {
            //Esto gracias a la BaseViewModel
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion
        #region Constructor
        public NotifyViewModel()
        {
            this.LoadNotify();
            this.apiService = new ApiService();
        }
        #endregion

        #region Methods
        private async void LoadNotify()
        {
            this.IsRefreshing = true;
            Droid.ServiceDirectorio.Directorio serviceout = new Droid.ServiceDirectorio.Directorio();
            serviceout.ConsultaNotificacionesCompleted += Serviceout_ConsultaNotificacionesCompleted;
            serviceout.ConsultaNotificacionesAsync();
        }

        private async void Serviceout_ConsultaNotificacionesCompleted(object sender, Droid.ServiceDirectorio.ConsultaNotificacionesCompletedEventArgs e)
        {
            //Revisar conexion a internet
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Directorio Acatlán",
                    connection.Message,
                    "OK");
                //await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
            }

            try
            {
                var listaux = new List<Notificaciones>();

                System.Data.DataSet wsResultado = e.Result;
                //this.lista_completa.Clear();
                foreach (System.Data.DataRow row in wsResultado.Tables[0].Rows)
                {
                    Notificaciones notificacion = new Notificaciones()
                    {
                        ID = Convert.ToInt32(row[0]),
                        Texto = row[1].ToString(),
                        Remitente = row[2].ToString(),
                        Destino = row[3].ToString(),
                        Fecha = Convert.ToDateTime(row[4]),
                    };
                    listaux.Add(notificacion);
                }
                this.Lista_completa = new ObservableCollection<Notificaciones>(listaux);
                this.IsRefreshing = false;
            }
            catch
            {
                //                          23 Enero 2019
                //Revisar como añadir tiempo máximo porque tarda demasiado
                //Significa que el servidor esta apagado
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                   "Directorio Acatlán",
                   "El servidor se encuentra deshabilitado. Intente más tarde.",
                   "OK");
                //await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadNotify);
            }
        }
        #endregion
    }
}
