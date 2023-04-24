////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    public class StyleConverter
    {
        private Style       mCurrentStyle = Style.Default;
        private GUIStyle[]  mWorking;
        private GUIStyle[]  mReference;

        Color linkColor         = new Color(0.41f, 0.71f, 1.0f, 1.0f);
        const int FixedBlock    = 7;
        const int Variable      = 8;
        const int FixedInline   = 12;

        static readonly string[] CustomStyles = new string[] {
            "variable",
            "h1",
            "h2",
            "h3",
            "h4",
            "h5",
            "h6",
            "fixed_block",
            "variable",
            "variable_bold",
            "variable_italic",
            "variable_bolditalic",
            "fixed_inline",
            "fixed_inline_bold",
            "fixed_inline_italic",
            "fixed_inline_bolditalic",
        };

        public StyleConverter( GUISkin skin )
        {
            mReference  = new GUIStyle[ CustomStyles.Length ];
            mWorking    = new GUIStyle[ CustomStyles.Length ];

            for( var i = 0; i < CustomStyles.Length; i++ )
            {
                mReference[ i ] = skin.GetStyle( CustomStyles[ i ] );
                mWorking[ i ]   = new GUIStyle( mReference[ i ] );
            }
        }


        //------------------------------------------------------------------------------

        public GUIStyle Apply( Style src )
        {
            if( src.Block )
            {
                return mWorking[ FixedBlock ];
            }

            var style = mWorking[ src.Size ];

            if( mCurrentStyle != src )
            {
                var font = ( src.Fixed ? FixedInline : Variable ) + ( src.Bold ? 1 : 0 ) + ( src.Italic ? 2 : 0 );

                style.font             = mReference[ font ].font;
                style.fontStyle        = mReference[ font ].fontStyle;
                style.normal.textColor = src.Link ? linkColor : mReference[ font ].normal.textColor;

                mCurrentStyle = src;
            }

            return style;
        }
    }
}
