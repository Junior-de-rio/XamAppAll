package crc640eb27f8010e29079;


public class LocationBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MyXamarinApp.Services.LocationBinder, MyXamarinApp", LocationBinder.class, __md_methods);
	}


	public LocationBinder ()
	{
		super ();
		if (getClass () == LocationBinder.class)
			mono.android.TypeManager.Activate ("MyXamarinApp.Services.LocationBinder, MyXamarinApp", "", this, new java.lang.Object[] {  });
	}

	public LocationBinder (com.xamarin.LocationService p0)
	{
		super ();
		if (getClass () == LocationBinder.class)
			mono.android.TypeManager.Activate ("MyXamarinApp.Services.LocationBinder, MyXamarinApp", "MyXamarinApp.Services.LocationService, MyXamarinApp", this, new java.lang.Object[] { p0 });
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
