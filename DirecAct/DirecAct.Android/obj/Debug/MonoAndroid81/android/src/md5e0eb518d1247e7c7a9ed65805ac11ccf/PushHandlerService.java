package md5e0eb518d1247e7c7a9ed65805ac11ccf;


public class PushHandlerService
	extends md513d040a829b3f298fbeeee5a6e2c042a.GcmServiceBase
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DirecAct.Droid.PushHandlerService, DirecAct.Android", PushHandlerService.class, __md_methods);
	}


	public PushHandlerService (java.lang.String p0)
	{
		super (p0);
		if (getClass () == PushHandlerService.class)
			mono.android.TypeManager.Activate ("DirecAct.Droid.PushHandlerService, DirecAct.Android", "System.String, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public PushHandlerService ()
	{
		super ();
		if (getClass () == PushHandlerService.class)
			mono.android.TypeManager.Activate ("DirecAct.Droid.PushHandlerService, DirecAct.Android", "", this, new java.lang.Object[] {  });
	}

	public PushHandlerService (java.lang.String[] p0)
	{
		super ();
		if (getClass () == PushHandlerService.class)
			mono.android.TypeManager.Activate ("DirecAct.Droid.PushHandlerService, DirecAct.Android", "System.String[], mscorlib", this, new java.lang.Object[] { p0 });
	}

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
