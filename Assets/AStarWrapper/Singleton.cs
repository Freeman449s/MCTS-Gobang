

public class Singleton<T> where T :class,new()
{
    private static T instance;
    private static readonly object locker = new object();
    public static T Instance {
        get {

            lock (locker) {

                if (instance == null) {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}
