namespace App.Scripts.Libs.NodeArchitecture
{
    public interface IContextUpdate
    {
        void OnUpdate();
    }

    public interface IContextFixedUpdate
    {
        void OnFixedUpdate();
    }

    public interface IContextLateUpdate
    {
        void OnLateUpdate();
    }
}