public interface ICarryier
{
    bool CarryObject();
    void PickUp(ICarryiable carryiable);
    void PutDown();
}