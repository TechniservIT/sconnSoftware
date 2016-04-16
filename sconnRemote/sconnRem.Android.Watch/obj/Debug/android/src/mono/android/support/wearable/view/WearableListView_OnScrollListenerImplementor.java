package mono.android.support.wearable.view;


public class WearableListView_OnScrollListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.WearableListView.OnScrollListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onAbsoluteScrollChange:(I)V:GetOnAbsoluteScrollChange_IHandler:Android.Support.Wearable.Views.WearableListView/IOnScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onCentralPositionChanged:(I)V:GetOnCentralPositionChanged_IHandler:Android.Support.Wearable.Views.WearableListView/IOnScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onScroll:(I)V:GetOnScroll_IHandler:Android.Support.Wearable.Views.WearableListView/IOnScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onScrollStateChanged:(I)V:GetOnScrollStateChanged_IHandler:Android.Support.Wearable.Views.WearableListView/IOnScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.WearableListView+IOnScrollListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", WearableListView_OnScrollListenerImplementor.class, __md_methods);
	}


	public WearableListView_OnScrollListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == WearableListView_OnScrollListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.WearableListView+IOnScrollListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onAbsoluteScrollChange (int p0)
	{
		n_onAbsoluteScrollChange (p0);
	}

	private native void n_onAbsoluteScrollChange (int p0);


	public void onCentralPositionChanged (int p0)
	{
		n_onCentralPositionChanged (p0);
	}

	private native void n_onCentralPositionChanged (int p0);


	public void onScroll (int p0)
	{
		n_onScroll (p0);
	}

	private native void n_onScroll (int p0);


	public void onScrollStateChanged (int p0)
	{
		n_onScrollStateChanged (p0);
	}

	private native void n_onScrollStateChanged (int p0);

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
