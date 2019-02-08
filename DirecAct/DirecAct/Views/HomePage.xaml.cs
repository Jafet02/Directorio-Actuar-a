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
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();

            this.SizeChanged += HomePage_SizeChanged;
        }


        private void HomePage_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width > this.Height)
            {
                //stack_busqueda.Orientation = StackOrientation.Horizontal;
                Home_Page.BackgroundImage = "homewallpaper2.png";
                
                /*
                stack_notify.TranslationY = -150;
                stack_notify.TranslationX = 180;
                */
            }
            else
            {
                //stack_busqueda.Orientation = StackOrientation.Vertical;
                Home_Page.BackgroundImage = "homewallpaper.png";


                /*
                stack_notify.TranslationY = 0;
                stack_notify.TranslationX = 0;
                */
            }
        }


    }
}