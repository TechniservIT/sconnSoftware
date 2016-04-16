package mono.android.support.v7.widget;


public class RecyclerView_OnScrollListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.v7.widget.RecyclerView.OnScrollListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onScrollStateChanged:(I)V:GetOnScrollStateChanged_IHandler:Android.Support.V7.Widget.RecyclerView/IOnScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onScrolled:(II)V:GetOnScrolled_IIHandler:Android.Support.V7.Widget.RecyclerView/IOnScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.V7.Widget.RecyclerView+IOnScrollListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RecyclerView_OnScrollListenerImplementor.class, __md_methods);
	}


	public RecyclerView_OnScrollListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RecyclerView_OnScrollListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.V7.Widget.RecyclerView+IOnScrollListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onScrollStateChanged (int p0)
	{
		n_onScrollStateChanged (p0);
	}

	private native void n_onScrollStateChanged (int p0);


	public void onScrolled (int p0, int p1)
	{
		n_onScrolled (p0, p1);
	}

	private native void n_onScrolled (int p0, int p1);

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
