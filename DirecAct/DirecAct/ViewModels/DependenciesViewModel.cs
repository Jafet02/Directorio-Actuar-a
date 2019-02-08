using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using Services;
    using System.Collections.ObjectModel;
    using Models;
    using Xamarin.Forms;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    public class DependenciesViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion
        //private ApiService apiServices;
        #region Attributes
        private ObservableCollection<DependenciesItemViewModel> dependencias_list;
        private bool isRefreshing;
        private string filter;
        ObservableCollection<Funcionarios> listaDependencias = new ObservableCollection<Funcionarios>();
        #endregion

        #region Properties
        //Aqui se recibe el area que se seleccinoo
        public Areas Area_selected
        {
            get;
            set;
        }

        public ObservableCollection<DependenciesItemViewModel> Dependencias_list
        {
            //Esto gracias a la BaseViewModel
            get { return this.dependencias_list; }
            set { SetValue(ref this.dependencias_list, value); }
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
                //this.Search();
            }
        }
        #endregion

        #region Constructor
        //Aqui recivimos el are seleccionada al instancias esta View 
        public DependenciesViewModel(Areas area_selected)
        {
            this.Area_selected = area_selected;
            this.LoadDependencies();
            this.apiService = new ApiService();
        }
        #endregion

        #region Methods
        private async void LoadDependencies()
        {
            this.IsRefreshing = true;

            Droid.ServiceDirectorio.Directorio dependencies_services = new Droid.ServiceDirectorio.Directorio();
            dependencies_services.DatosFuncAreaCompleted += Dependencies_services_DatosFuncAreaCompleted;
            //Esta variable es de la seleccionada
            dependencies_services.DatosFuncAreaAsync(this.Area_selected.ID_Padre);

        }

        private async void Dependencies_services_DatosFuncAreaCompleted(object sender, Droid.ServiceDirectorio.DatosFuncAreaCompletedEventArgs e)
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
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            try
            {
                System.Data.DataSet wsResultado = e.Result;
                this.listaDependencias.Clear();
                foreach (System.Data.DataRow row in wsResultado.Tables[0].Rows)
                {
                    Funcionarios funcionario = new Funcionarios()
                    {
                        RFC = row[0].ToString(),
                        Nombre = row[1].ToString(),
                        Cargo = row[2].ToString(),
                        Correo = row[3].ToString(),
                        Telefono = row[4].ToString(),
                        ID_Padre = Convert.ToInt32(row[5]),
                        ID_Adscripcion = Convert.ToInt32(row[6]),
                        Celular = row[7].ToString(),
                        //NO SOLICITAR PASS             cambiar esto en el servicio
                        //Pass = row[8].ToString(),
                        Foto = row[9].ToString(),
                        Padre = row[10].ToString(),
                        Adscripcion = row[11].ToString(),

                    };
                    if ((string.IsNullOrEmpty(funcionario.Nombre)) || (funcionario.Nombre != " "))
                    {
                        //Añadir la Adscripción principal al inicio, los demás ordenarlos alfabéticamente
                        if (funcionario.ID_Adscripcion == this.Area_selected.ID_Padre)
                        {
                            listaDependencias.Insert(0, funcionario);
                        }
                        else
                        {
                            listaDependencias.Add(funcionario);
                        }
                    }
                }
                this.Dependencias_list = new ObservableCollection<DependenciesItemViewModel>(
                    this.ToNamesItemViewModel());

                this.IsRefreshing = false;
            }
            catch
            {
                this.IsRefreshing = false;
            }
        }

        #endregion


        #region Methods
        private IEnumerable<DependenciesItemViewModel> ToNamesItemViewModel()
        {
            return this.listaDependencias.Select(n => new DependenciesItemViewModel
            {
                RFC = n.RFC,
                Nombre = n.Nombre,
                Cargo = n.Cargo,
                Correo = n.Correo,
                Telefono = n.Telefono,
                Foto = n.Foto,
                ID_Padre = n.ID_Padre,
                ID_Adscripcion = n.ID_Adscripcion,
                Celular = n.Celular,
                Pass = n.Pass,
                Padre = n.Padre,
                Adscripcion = n.Adscripcion,
            });
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadDependencies);
            }
        }


        #endregion


    }
}
