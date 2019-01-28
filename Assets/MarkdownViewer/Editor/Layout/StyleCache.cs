////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    public class StyleCache
    {
        private Style       mStyleCurrentLayout = new Style();

        private GUIStyle[]  mStyles;
        private Font        mFontVariable;
        private Font        mFontFixed;
        private Texture2D   mHighlight;

        public StyleCache( GUISkin skin )
        {
            mStyles = new GUIStyle[ 7 ];

            mStyles[ 0 ] = new GUIStyle( skin.label );
            mStyles[ 1 ] = new GUIStyle( skin.GetStyle( "h1" ) );
            mStyles[ 2 ] = new GUIStyle( skin.GetStyle( "h2" ) );
            mStyles[ 3 ] = new GUIStyle( skin.GetStyle( "h3" ) );
            mStyles[ 4 ] = new GUIStyle( skin.GetStyle( "h4" ) );
            mStyles[ 5 ] = new GUIStyle( skin.GetStyle( "h5" ) );
            mStyles[ 6 ] = new GUIStyle( skin.GetStyle( "h6" ) );

            var fixedStyle = skin.GetStyle( "fixed" );

            mFontVariable = skin.label.font;
            mFontFixed    = fixedStyle.font;
            mHighlight    = fixedStyle.normal.background;
        }


        //------------------------------------------------------------------------------

        public GUIStyle Apply( Style style )
        {
            // if( mStyleDoc == style )
            // {
            //     return mStyleGUI;
            // }

            mStyleCurrentLayout = style;
            var guiStyle = mStyles[ style.Size ];

            // font

            guiStyle.font = style.Fixed ? mFontFixed : mFontVariable;


            // font style

            if( style.Bold )
            {
                guiStyle.fontStyle = style.Italic ? FontStyle.BoldAndItalic : FontStyle.Bold;

            }
            else
            {
                guiStyle.fontStyle = style.Italic ? FontStyle.Italic : FontStyle.Normal;
            }


            // highlight

            //guiStyle.normal.background = style.Highlight ? mHighlight : null;

            // color

            if( style.Link )
            {
                guiStyle.normal.textColor = Color.blue;
            }
            else if( style.Fixed )
            {
                guiStyle.normal.textColor = Color.grey;
            }
            else
            {
                guiStyle.normal.textColor = Color.black;
            }

            return guiStyle;
        }
    }
}
