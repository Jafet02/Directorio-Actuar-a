using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Services;
    using Models;
    using Xamarin.Forms;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class NamesViewModel : BaseViewModel
    {

        #region Services
        private ApiService apiService;
        #endregion
        //private ApiService apiServices;
        #region Attributes
        private ObservableCollection<NamesItemViewModel> funcio_list;
        private bool isRefreshing;
        private string filter;
        ObservableCollection<Funcionarios> listaFuncionarios = new ObservableCollection<Funcionarios>();
        #endregion

        #region Properties
        public ObservableCollection<NamesItemViewModel> Funcio_list
        {
            //Esto gracias a la BaseViewModel
            get { return this.funcio_list; }
            set { SetValue(ref this.funcio_list, value); }
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
        public NamesViewModel()
        {
            this.LoadFuncionarios();
            this.apiService = new ApiService();
        }
        #endregion

        #region Methods
        private async void LoadFuncionarios()
        {
            this.IsRefreshing = true;


            Droid.ServiceDirectorio.Directorio funcionarios_services = new Droid.ServiceDirectorio.Directorio();
            funcionarios_services.DatosFuncionariosCompleted += Funcionarios_services_DatosFuncionariosCompleted;
            funcionarios_services.DatosFuncionariosAsync();
        }

        private async void Funcionarios_services_DatosFuncionariosCompleted(object sender, Droid.ServiceDirectorio.DatosFuncionariosCompletedEventArgs e)
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
                this.listaFuncionarios.Clear();
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
                        this.listaFuncionarios.Add(funcionario);
                    }
                }
                this.Funcio_list = new ObservableCollection<NamesItemViewModel>(
                    this.ToNamesItemViewModel());

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

        #region Methods
        private IEnumerable<NamesItemViewModel> ToNamesItemViewModel()
        {
            return this.listaFuncionarios.Select(n => new NamesItemViewModel
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

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadFuncionarios);
            }
        }
        public ICommand SearchFunc
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        #endregion

        #region Functions
        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Funcio_list = new ObservableCollection<NamesItemViewModel>(
                    this.ToNamesItemViewModel());
            }
            else
            {
                /*
                this.Funcio_list = new ObservableCollection<NamesItemViewModel>(
                    this.ToNamesItemViewModel().Where(
                        f => f.Nombre.ToLower().Contains(this.Filter.ToLower())));*/
                //Añadie indistinción entre aceentos

                this.Funcio_list = new ObservableCollection<NamesItemViewModel>(
                    this.ToNamesItemViewModel().Where(
                        f => QuitAccents(f.Nombre.ToLower()).Contains(QuitAccents(this.Filter.ToLower()))));
            }
        }
        #endregion
    }
}
