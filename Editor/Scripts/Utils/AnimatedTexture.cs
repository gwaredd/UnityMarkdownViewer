using UnityEngine;

namespace MG.MDV
{
    public class AnimatedTexture
    {
        public Texture2D[]  Textures;
        public float[]      Delays;

        public int NumFrames
        {
            get
            {
                return Textures != null ? Textures.Length : 0;
            }
        }

        public Texture FirstFrame
        {
            get
            {
                return Textures != null ? Textures[0] : null;
            }
        }
    }
}
