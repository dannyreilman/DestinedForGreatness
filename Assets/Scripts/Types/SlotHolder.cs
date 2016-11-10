//An interface that allows an object to sit in a slot in either party
public interface SlotHolder : AttackTarget
{
    int index
    {
        get;
        set;
    }

    bool side
    {
        get;
        set;
    }

    bool EntranceComplete();
    void StartEntrance(float v);
    void AllReady();
    void Flip();
    void ActivateAI();
    void DoAttack(int attack);
    bool CanAttack();
    void SAP();
    bool IsAlive();
    bool CanBeAttacked();
    void Removed();
    void Added();
}
