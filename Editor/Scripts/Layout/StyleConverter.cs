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
        const int FixedInline   = 11;

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
            "fixed_inline",
            "fixed_inline_bold",
            "fixed_inline_italic",
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
                int font   = src.Fixed ? FixedInline : Variable;
                int offset = 0;

                style.fontStyle = FontStyle.Normal;

                if( src.Bold && src.Italic )
                {
                    offset = 2;
                    style.fontStyle = FontStyle.Bold;
                }
                else if( src.Bold )
                {
                    offset = 1;
                }
                else if( src.Italic )
                {
                    offset = 2;
                }

                style.font = mReference[ font + offset ].font;
                style.normal.textColor = mReference[ font + offset ].normal.textColor;

                if( src.Link )
                {
                    style.normal.textColor = linkColor;
                }

                mCurrentStyle = src;
            }

            return style;
        }
    }
}
