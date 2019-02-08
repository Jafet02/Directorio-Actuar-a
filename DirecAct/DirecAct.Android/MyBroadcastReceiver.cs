using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;
using WindowsAzure.Messaging;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace DirecAct.Droid
{
    //using Services;
    using ViewModels;

    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
    {
        public static string[] SENDER_IDS = new string[] { Constans.SenderID };

        public const string TAG = "MyBroadcastReceiver-GCM";
    }

    [Service]
    public class PushHandlerService : GcmServiceBase
    {

        #region Properties
        public NotificationHub Hub { get; set; }
        public static string RegistrationID { get; private set; }
        #endregion

        #region Methods
        public PushHandlerService() : base(Constans.SenderID)
        {
            Log.Info(MyBroadcastReceiver.TAG, "PushHandlerService() constructor");
        }

        //Cuando se recibe un mensaje   Sirve para perzonalizar las notificaciones
        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GCM Message Received!");

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            var message = intent.Extras.GetString("Message");
            var type = intent.Extras.GetString("Type");

            if (!string.IsNullOrEmpty(message))
            {
                var notification = intent.Extras.GetString("Notificación");
                createNotification("Directorio Actuaria App", string.Format("{0}", message));
            }
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(MyBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(MyBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }

        //Cuando registra la app el el servicio
        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;

            Hub = new NotificationHub(Constans.NotificationHubName, Constans.ListenConnectionString, context);

            try
            {
                Hub.UnregisterAll(registrationId);
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }

            var tags = new List<string>() { };

            var mainviewModel = MainViewModel.GetInstance();
            //if(mainviewModel.Curr)
            //En vez de Login    era User   esta bien, es de mi  model
            // Aqui podemos crear grupos para notificaciones,   varios tags, etc`s
            //No hay token BD SQLITE para evaluar el token 
            //if (mainviewModel.Token != null)
            //{
            //var userId = mainviewModel.Login.User;
            //tags.Add("userId:" + userId);

            //Cambiaremos el IF

            if (mainviewModel.Home.Carrera != null)
            {
                var carreraId = mainviewModel.Home.Carrera;
                tags.Add("carreraId:" + carreraId);
            }

            //GUardamos el tag paraa identificar el ID de los usuarios
            //}

            try
            {
                var hubRegistration = Hub.Register(registrationId, tags.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);

            createNotification("GSScore", "El dispositivo no ha sido registrado!");
        }

        void createNotification(string title, string desc)
        {
            //Crear notificación
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show UI    crea un intentó para mostrar la interfáz de usuario
            //var uiIntent = new Intent(this, typeof(MainActivity));
            var uiIntent = new Intent(this, typeof(MainActivity));


            //Create the notification     crea la notificación
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            //Auto-cancel will remove the notification once the user touches it
            //Auto-cancel eliminará la notificación una vez que el usuario la toque
            notification.Flags = NotificationFlags.AutoCancel;
            //Añadir sonido a la notificación
            notification.Defaults = NotificationDefaults.All;


            //Set the notification info
            //we use the pending intent, passing our ui intent over, which will get called
            //when the notification is tapped.

            // Establecer la información de notificación
            // usamos la intención pendiente, pasando nuestra intención ui, que será llamada cuando se toca la notificación.
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            
            //Show the notification    Muestra la notificación
            notificationManager.Notify(1, notification);
            
            //Muesta el alert
            dialogNotify(title, desc);
        }

        protected void dialogNotify(String title, String message)
        {
            var mainActivity = MainActivity.GetInstance();
            mainActivity.RunOnUiThread(() =>
            {
                try
                {
                    AlertDialog.Builder dlg = new AlertDialog.Builder(mainActivity);
                    AlertDialog alert = dlg.Create();
                    alert.SetTitle(title);
                    alert.SetButton("OK", delegate
                    {
                        alert.Dismiss();
                    });
                    // Es ic_launcher
                    alert.SetIcon(Resource.Drawable.ic_dialog_close_dark);
                    alert.SetMessage(message);
                    //Si la app no esta corriendo aqui marca error de la notificación   alert.Show()

                    //Prueba creando una instancia de MainActivity
                    if (mainActivity.ApplicationInfo.Enabled)
                    {
                        alert.Show();
                    }
                    else
                    {
                        //Si la app no esta corriendo necesitamos mostrar la alerta   y qui es
                        MainViewModel.GetInstance().Home = new HomeViewModel();

                        alert.Show();
                    }
                }
                catch
                {
                   
                    
                }
            });
        }
        #endregion
    }
}