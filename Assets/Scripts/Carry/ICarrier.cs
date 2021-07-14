public interface ICarrier
{
    bool CarryObject();
    void PickUp(ICarriable carriable);
    void PutDown();
}