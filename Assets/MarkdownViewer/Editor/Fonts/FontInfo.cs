////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

namespace MG.MDV
{
    public class FontInfo
    {
        public string Name;
        public float  Size;
        public Dictionary<int,float> Advance = new Dictionary<int, float>(); // unicode => advance

        public bool GetAdvance( char ch, out float advance, float size, FontStyle style )
        {
            if( Advance.TryGetValue( ch, out advance ) )
            {
                advance *= size / Size;
                return true;
            }

            return false;
        }
    }

    public static class Fonts
    {
        public static FontInfo Variable = new FontRobotoLight();
        public static FontInfo Fixed    = new FontRobotoMonoLight();
    }
}

