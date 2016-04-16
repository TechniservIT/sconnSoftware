package mono.android.support.wearable.view;


public class GridViewPager_OnPageChangeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.GridViewPager.OnPageChangeListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageScrollStateChanged:(I)V:GetOnPageScrollStateChanged_IHandler:Android.Support.Wearable.Views.GridViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onPageScrolled:(IIFFII)V:GetOnPageScrolled_IIFFIIHandler:Android.Support.Wearable.Views.GridViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onPageSelected:(II)V:GetOnPageSelected_IIHandler:Android.Support.Wearable.Views.GridViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.GridViewPager+IOnPageChangeListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GridViewPager_OnPageChangeListenerImplementor.class, __md_methods);
	}


	public GridViewPager_OnPageChangeListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GridViewPager_OnPageChangeListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.GridViewPager+IOnPageChangeListenerImplementor, Xamarin.Android.Wearable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPageScrollStateChanged (int p0)
	{
		n_onPageScrollStateChanged (p0);
	}

	private native void n_onPageScrollStateChanged (int p0);


	public void onPageScrolled (int p0, int p1, float p2, float p3, int p4, int p5)
	{
		n_onPageScrolled (p0, p1, p2, p3, p4, p5);
	}

	private native void n_onPageScrolled (int p0, int p1, float p2, float p3, int p4, int p5);


	public void onPageSelected (int p0, int p1)
	{
		n_onPageSelected (p0, p1);
	}

	private native void n_onPageSelected (int p0, int p1);

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
