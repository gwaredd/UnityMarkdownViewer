////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    [CreateAssetMenu( )]
    public class StyleConfig : ScriptableObject
    {
        public Font      FontVariable;
        public Font      FontFixed;
        public float[]   Sizes;
        public Vector2[] Margins;

        public float GetFontSize( int size )
        {
            return Sizes[ size ];
        }

        public Vector2 GetMargin( int size )
        {
            return Margins[ size ];
        }
    }
}
