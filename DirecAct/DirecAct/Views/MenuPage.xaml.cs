using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DirecAct.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        public MenuPage()
        {
            InitializeComponent();

            this.SizeChanged += MenuPage_SizeChanged;
        }

        private void MenuPage_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width > this.Height)
            {
                //stack_busqueda.Orientation = StackOrientation.Horizontal;
                //Menu_Page.BackgroundImage = "menu2.jpg";


                Stack_btns_menu.TranslationX = 20;
                Stack_btns_menu.TranslationY = 20;
                stacinitial.TranslationY = 0;
                btn_notify.TranslationY = 100;

                //Se quitaron las opciones de registro  a servicio de notificciones
                /*
                btn_register.TranslationX = 320;
                btn_register.TranslationY = -30;
                btn_register.Scale = .6;
                */

                /*
                stack_notify.TranslationY = -150;
                stack_notify.TranslationX = 180;
                */
            }
            else
            {
                //stack_busqueda.Orientation = StackOrientation.Vertical;
                //Menu_Page.BackgroundImage = "menu.jpg";
                Stack_btns_menu.TranslationX = -10;
                Stack_btns_menu.TranslationY = 40;
                stacinitial.TranslationY = 80;
                btn_notify.TranslationY = 180;

                //Se quitaron las opciones de registro  a servicio de notificciones
                /*
                btn_register.TranslationX = 25;
                btn_register.TranslationY = 0;
                btn_register.Scale = .7;
                */
                
                
                /*
                stack_notify.TranslationY = 0;
                stack_notify.TranslationX = 0;
                */
            }
        }
    }
}