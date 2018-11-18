using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watchable<T>
{
	public delegate void Update(T value);
	public Update frame_update;
	public Update fast_update;

	public Watchable()
	{
		WatchableInstance.frame_end += OnFrameEnd;
	}

	public void Watch(Update u)
	{
		frame_update += u;
		u(internal_data);
	}

	public void FastWatch(Update u)
	{
		fast_update += u;
		u(internal_data);
	}

	private T internal_data;
	public T data
	{
		get
		{
			return internal_data;
		}
		set
		{
			internal_data = value;
			if(fast_update != null)
				fast_update(value);
		}
	}

	public void OnFrameEnd()
	{
		if(frame_update != null)
			frame_update(internal_data);
	}
}
