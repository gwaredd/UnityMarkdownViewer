////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    [CreateAssetMenu( )]
    public class StyleConfig : ScriptableObject
    {
        public Font    FontVariable;
        public Font    FontFixed;
        public float[] Sizes;

        public float GetSize( int size )
        {
            return Sizes[ size ];

        }
    }
}
