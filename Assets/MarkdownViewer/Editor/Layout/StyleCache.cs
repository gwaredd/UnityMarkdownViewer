////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace MG.MDV
{
    public class StyleCache
    {
        private GUIStyle    mStyleCurrentGUI    = null;
        private Style       mStyleCurrentLayout = new Style();

        public StyleConfig  Config { get; } = null;
        public GUIStyle     Active { get { return mStyleCurrentGUI; } }

        private GUIStyle[]  mStyles;

        public StyleCache( GUISkin skin, StyleConfig config )
        {
            Config = config;

            mStyles = new GUIStyle[ 7 ];

            mStyles[ 0 ] = new GUIStyle( skin.label );
            mStyles[ 1 ] = new GUIStyle( skin.GetStyle( "h1" ) );
            mStyles[ 2 ] = new GUIStyle( skin.GetStyle( "h2" ) );
            mStyles[ 3 ] = new GUIStyle( skin.GetStyle( "h3" ) );
            mStyles[ 4 ] = new GUIStyle( skin.GetStyle( "h4" ) );
            mStyles[ 5 ] = new GUIStyle( skin.GetStyle( "h5" ) );
            mStyles[ 6 ] = new GUIStyle( skin.GetStyle( "h6" ) );

            mStyleCurrentGUI = mStyles[ 0 ];
        }

        public GUIStyle Reset()
        {
            return Apply( new Style(), true );
        }


        //------------------------------------------------------------------------------

        public GUIStyle Apply( Style style, bool force = false )
        {
            //             if( mStyleDoc == style )
            //             {
            //                 return mStyleGUI;
            //             }

            mStyleCurrentLayout = style;
            mStyleCurrentGUI    = mStyles[ style.Size ];

            // font

            mStyleCurrentGUI.font = style.Fixed ? Config.FontFixed : Config.FontVariable;


            // font style

            if( style.Bold )
            {
                mStyleCurrentGUI.fontStyle = style.Italic ? FontStyle.BoldAndItalic : FontStyle.Bold;

            }
            else
            {
                mStyleCurrentGUI.fontStyle = style.Italic ? FontStyle.Italic : FontStyle.Normal;
            }


            // highlight

            mStyleCurrentGUI.normal.background = style.Highlight ? Config.Background : null;

            // color

            mStyleCurrentGUI.normal.textColor = style.Link ? Color.blue : Color.black;

            return mStyleCurrentGUI;
        }
    }
}
