namespace TinyTrails.Utils
{
    public class Util
    {
        public static int RandomNumberLessMiddle(int a, int b)
        {
            var middle = ((b - a) / 2) + a;
            int value;

            while (true)
            {
                value = UnityEngine.Random.Range(a, b);

                if (value != middle) break;
            }

            return value;
        }
    }
}