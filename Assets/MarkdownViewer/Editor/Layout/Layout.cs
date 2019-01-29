////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    public class Layout
    {
        public float Height = 100.0f;

        Context        mContext;
        BlockContainer mDocument;

        public Layout( Context context, BlockContainer doc )
        {
            mContext  = context;
            mDocument = doc;
        }

        public void Arrange( float maxWidth )
        {
            mContext.Reset();
            var size = mDocument.Arrange( mContext, Vector2.zero, maxWidth );
            Height = size.y;
        }

        public void Draw()
        {
            mContext.Reset();
            mDocument.Draw( mContext );
        }
    }
}

