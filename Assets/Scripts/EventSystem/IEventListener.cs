namespace ShapeReplica.EventSystem
{
    public interface IEventListener
    {
        void OnEnable();
        void OnDisable();
        void OnEventRaised();
    }

    public interface IEventListener<T>
    {
        void OnDisable();
        void OnEventRaised(T value);
        void OnEnable();
    }
}