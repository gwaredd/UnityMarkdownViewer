////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace MG.MDV
{
    public class StyleCache
    {
        private Style       mStyleDoc = new Style();
        private GUIStyle    mStyleGUI = null;
        private StyleConfig mConfig   = null;

        private bool        mFixed  = false;
        private int         mSize   = 0;

        public StyleConfig  Config { get { return mConfig; } }
        public GUIStyle     Active { get { return mStyleGUI; } }

        public StyleCache( GUIStyle style, StyleConfig config )
        {
            mStyleGUI = new GUIStyle( style );
            mConfig   = config;
        }

        public GUIStyle Reset()
        {
            return Apply( new Style() );
        }


        //------------------------------------------------------------------------------

        public GUIStyle Apply( Style style )
        {
//             if( mStyleDoc == style )
//             {
//                 return mStyleGUI;
//             }

            if( style.Bold )
            {
                mStyleGUI.fontStyle = style.Italic ? FontStyle.BoldAndItalic : FontStyle.Bold;

            }
            else
            {
                mStyleGUI.fontStyle = style.Italic ? FontStyle.Italic : FontStyle.Normal;
            }

            mStyleGUI.normal.textColor = style.Link ? Color.blue : Color.black;

//             if( mSize != style.Size )
            {
                mSize = style.Size;
                mStyleGUI.fontSize = (int) mConfig.GetFontSize( mSize );
            }

            if( style.Fixed != mFixed )
            {
                mFixed = style.Fixed;
                mStyleGUI.font = mFixed ? mConfig.FontFixed : mConfig.FontVariable;
            }

            mStyleDoc = style;
            return mStyleGUI;
        }
    }
}
