namespace ClassLibrary1
{
    using System;

    public class Class1
    {
        public static void DoWork()
        {
            Method((object)new object());
        }

        private static void Method(object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
