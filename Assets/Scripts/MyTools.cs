using UnityEngine;

namespace Mannaz
{
    public static class MyTools
    {
        internal static bool Random50()
        {
            return Random.Range(0, 100) >= 49;
        }

        internal static bool Random30()
        {
            return Random.Range(0, 100) >= 69;
        }
    }
}