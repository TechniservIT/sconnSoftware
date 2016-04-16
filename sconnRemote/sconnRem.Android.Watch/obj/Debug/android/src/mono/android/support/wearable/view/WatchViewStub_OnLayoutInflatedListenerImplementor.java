package mono.android.support.wearable.view;


public class WatchViewStub_OnLayoutInflatedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.WatchViewStub.OnLayoutInflatedListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onLayoutInflated:(Landroid/support/wearable/view/WatchViewStub;)V:GetOnLayoutInflated_Landroid_support_wearable_view_WatchViewStub_Handler:Android.Support.Wearable.Views.WatchViewStub/IOnLayoutInflatedListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.WatchViewStub+IOnLayoutInflatedListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", WatchViewStub_OnLayoutInflatedListenerImplementor.class, __md_methods);
	}


	public WatchViewStub_OnLayoutInflatedListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == WatchViewStub_OnLayoutInflatedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.WatchViewStub+IOnLayoutInflatedListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onLayoutInflated (android.support.wearable.view.WatchViewStub p0)
	{
		n_onLayoutInflated (p0);
	}

	private native void n_onLayoutInflated (android.support.wearable.view.WatchViewStub p0);

	java.util.ArrayList refList;
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
