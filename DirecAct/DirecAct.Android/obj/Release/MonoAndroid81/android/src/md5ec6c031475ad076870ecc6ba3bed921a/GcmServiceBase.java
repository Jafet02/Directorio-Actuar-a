package md5ec6c031475ad076870ecc6ba3bed921a;


public abstract class GcmServiceBase
	extends mono.android.app.IntentService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onHandleIntent:(Landroid/content/Intent;)V:GetOnHandleIntent_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Gcm.GcmServiceBase, Microsoft.WindowsAzure.Messaging.Android", GcmServiceBase.class, __md_methods);
	}


	public GcmServiceBase (java.lang.String p0)
	{
		super (p0);
		if (getClass () == GcmServiceBase.class)
			mono.android.TypeManager.Activate ("Gcm.GcmServiceBase, Microsoft.WindowsAzure.Messaging.Android", "System.String, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public GcmServiceBase ()
	{
		super ();
		if (getClass () == GcmServiceBase.class)
			mono.android.TypeManager.Activate ("Gcm.GcmServiceBase, Microsoft.WindowsAzure.Messaging.Android", "", this, new java.lang.Object[] {  });
	}

	public GcmServiceBase (java.lang.String[] p0)
	{
		super ();
		if (getClass () == GcmServiceBase.class)
			mono.android.TypeManager.Activate ("Gcm.GcmServiceBase, Microsoft.WindowsAzure.Messaging.Android", "System.String[], mscorlib", this, new java.lang.Object[] { p0 });
	}


	public void onHandleIntent (android.content.Intent p0)
	{
		n_onHandleIntent (p0);
	}

	private native void n_onHandleIntent (android.content.Intent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
