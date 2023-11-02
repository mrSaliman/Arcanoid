namespace App.Scripts.Libs.ObjectPool
{
    public interface IPoolable
    {
        void Activate();
        void Deactivate();
    }
}