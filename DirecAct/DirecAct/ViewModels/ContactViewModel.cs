using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.ViewModels
{
    using Models;
    using System;
    using Xamarin.Forms;

    public class ContactViewModel : BaseViewModel
    {
        #region Attributes
        private ImageSource image_contact;
        #endregion

        #region Propperties
        public Funcionarios Funcionario
        {
            get;
            set;
        }
        //Este es mi objeto bindado
        public ImageSource Image_contact
        {
            //Esto gracias a la BaseViewModel
            get { return this.image_contact; }
            set { SetValue(ref this.image_contact, value); }
        }
        #endregion

        #region Constructors
        public ContactViewModel(Funcionarios funcionario)
        {
            this.Funcionario = funcionario;
            this.image_contact = ImageSource.FromUri(new Uri(this.Funcionario.Foto));
            //Foto_contacto.Source = ImageSource.FromUri(new Uri(Funcionario.Foto));
        }
        #endregion
    }
}
