using System;

namespace Friedforfun.ContextSteering.Tests
{
    public static class TestUtilities
    {
        public static float DOTPRODTOLERANCE = 0.000001f;

        // source: Kkica - https://stackoverflow.com/questions/60067717/privateobject-in-visual-studio
        public static T CallNonPublicMethod<T>(this object o, string methodName, params object[] args)
        {
            var type = o.GetType();
            var mi = type.GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (mi != null)
            {
                return (T)mi.Invoke(o, args);
            }

            throw new Exception($"Method {methodName} does not exist on type {type}");
        }

        public static T CallNonPublicProperty<T>(this object o, string methodName)
        {
            var type = o.GetType();
            var mi = type.GetProperty(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (mi != null)
            {
                return (T)mi.GetValue(o);
            }

            throw new Exception($"Property {methodName} does not exist on type {type}");
        }
    }

}
