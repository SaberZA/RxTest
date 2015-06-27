package md55ba2aad808203a2e08daef6e680cd9fe;


public class ReactiveFragment_1
	extends md55ba2aad808203a2e08daef6e680cd9fe.ReactiveFragment
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ReactiveUI.ReactiveFragment`1, ReactiveUI, Version=6.5.0.0, Culture=neutral, PublicKeyToken=null", ReactiveFragment_1.class, __md_methods);
	}


	public ReactiveFragment_1 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ReactiveFragment_1.class)
			mono.android.TypeManager.Activate ("ReactiveUI.ReactiveFragment`1, ReactiveUI, Version=6.5.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
