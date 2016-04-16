package mono.android.support.wearable.view;


public class DelayedConfirmationView_DelayedConfirmationListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.DelayedConfirmationView.DelayedConfirmationListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimerFinished:(Landroid/view/View;)V:GetOnTimerFinished_Landroid_view_View_Handler:Android.Support.Wearable.Views.DelayedConfirmationView/IDelayedConfirmationListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onTimerSelected:(Landroid/view/View;)V:GetOnTimerSelected_Landroid_view_View_Handler:Android.Support.Wearable.Views.DelayedConfirmationView/IDelayedConfirmationListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.DelayedConfirmationView+IDelayedConfirmationListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DelayedConfirmationView_DelayedConfirmationListenerImplementor.class, __md_methods);
	}


	public DelayedConfirmationView_DelayedConfirmationListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DelayedConfirmationView_DelayedConfirmationListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.DelayedConfirmationView+IDelayedConfirmationListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTimerFinished (android.view.View p0)
	{
		n_onTimerFinished (p0);
	}

	private native void n_onTimerFinished (android.view.View p0);


	public void onTimerSelected (android.view.View p0)
	{
		n_onTimerSelected (p0);
	}

	private native void n_onTimerSelected (android.view.View p0);

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
