using UnityEngine;
using System.Collections;

public class BuffCarrier : Attack {
    private Buff buff;

	public BuffCarrier(float damage, Character index, Buff buff) : base(damage, index)
    {
        this.buff = buff;
    }
	
    protected override void Effects(Character c)
    {
        buff.Activate(c);
    }
}
