using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Models;
    using Services;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class AreasViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion
        //private ApiService apiServices;
        #region Attributes
        private ObservableCollection<AreasItemViewModel> areas;
        private bool isRefreshing;
        private string filter;
        ObservableCollection<Areas> listaAreas = new ObservableCollection<Areas>();
        #endregion

        #region Properties
        public ObservableCollection<AreasItemViewModel> Areas
        {
            //Esto gracias a la BaseViewModel
            get { return this.areas; }
            set { SetValue(ref this.areas, value); }
        }
        public bool IsRefreshing
        {
            //Esto gracias a la BaseViewModel
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        public string Filter
        {
            //Esto gracias a la BaseViewModel
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }
        #endregion

        #region Constructor
        public AreasViewModel()
        {
            this.LoadAreas();
            this.apiService = new ApiService();
        }
        #endregion

        #region Methods
        private IEnumerable<AreasItemViewModel> ToAreasItemViewModel()
        {
            return this.listaAreas.Select(n => new AreasItemViewModel
            {
                ID_Padre = n.ID_Padre,
                Padre = n.Padre,
            });
        }
        private string QuitAccents(string inputString)
        {
            Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);

            inputString = a.Replace(inputString, "a");
            inputString = e.Replace(inputString, "e");
            inputString = i.Replace(inputString, "i");
            inputString = o.Replace(inputString, "o");
            inputString = u.Replace(inputString, "u");

            return inputString;
        }
        #endregion

        #region Functions
        private async void LoadAreas()
        {
            this.IsRefreshing = true;

            Droid.ServiceDirectorio.Directorio areas_services = new Droid.ServiceDirectorio.Directorio();
            areas_services.DatosAreasCompleted += Areas_services_DatosAreasCompleted;
            areas_services.DatosAreasAsync();
            //si
            //Aqui revisariamos la coneccion de internet con la función del services

        }

        private async void Areas_services_DatosAreasCompleted(object sender, Droid.ServiceDirectorio.DatosAreasCompletedEventArgs e)
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
                System.Data.DataSet wsResultado = e.Result;
                this.listaAreas.Clear();
                foreach (System.Data.DataRow row in wsResultado.Tables[0].Rows)
                {
                    Areas area = new Areas()
                    {
                        ID_Padre = Convert.ToInt32(row[0]),
                        Padre = row[1].ToString(),
                    };
                    this.listaAreas.Add(area);
                }
                //this.Areas = new ObservableCollection<Areas>(this.listaAreas);
                this.Areas = new ObservableCollection<AreasItemViewModel>(
                    this.ToAreasItemViewModel());

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
                return new RelayCommand(LoadAreas);
            }
        }

        public ICommand SearchArea
        {
            get
            {
                return new RelayCommand(Search);
            }
        }
        #endregion

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Areas = new ObservableCollection<AreasItemViewModel>(
                    this.ToAreasItemViewModel());
            }
            else
            {
                this.Areas = new ObservableCollection<AreasItemViewModel>(
                    this.ToAreasItemViewModel().Where(
                        f => QuitAccents(f.Padre.ToLower()).Contains(QuitAccents(this.Filter.ToLower()))));
            }
        }
    }
}
