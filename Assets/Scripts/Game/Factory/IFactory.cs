namespace Base.Game.Factory
{
    public interface IFactory<T>
    {
        T GetObject();
        int GetTotalObject();
    }
}
