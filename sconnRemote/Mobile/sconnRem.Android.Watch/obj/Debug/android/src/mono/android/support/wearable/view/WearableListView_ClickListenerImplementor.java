package mono.android.support.wearable.view;


public class WearableListView_ClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.WearableListView.ClickListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/support/wearable/view/WearableListView$ViewHolder;)V:GetOnClick_Landroid_support_wearable_view_WearableListView_ViewHolder_Handler:Android.Support.Wearable.Views.WearableListView/IClickListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onTopEmptyRegionClick:()V:GetOnTopEmptyRegionClickHandler:Android.Support.Wearable.Views.WearableListView/IClickListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.WearableListView+IClickListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", WearableListView_ClickListenerImplementor.class, __md_methods);
	}


	public WearableListView_ClickListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == WearableListView_ClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.WearableListView+IClickListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onClick (android.support.wearable.view.WearableListView.ViewHolder p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.support.wearable.view.WearableListView.ViewHolder p0);


	public void onTopEmptyRegionClick ()
	{
		n_onTopEmptyRegionClick ();
	}

	private native void n_onTopEmptyRegionClick ();

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
