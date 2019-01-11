////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace MG.MDV
{
    public class RenderContext
    {
        private void UpdateStyle()
        {
            if( mBold )
            {
                Style.fontStyle = mItalic ? FontStyle.BoldAndItalic : FontStyle.Bold;
            }
            else
            {
                Style.fontStyle = mItalic ? FontStyle.Italic : FontStyle.Normal;
            }
        }

        bool mBold = false;

        public bool Bold
        {
            get
            {
                return mBold;
            }

            set
            {
                mBold = value;
                UpdateStyle();
            }
        }

        bool mItalic = false;

        public bool Italic
        {
            get
            {
                return mItalic;
            }

            set
            {
                mItalic = value;
                UpdateStyle();
            }
        }

        bool mFixed = false;

        public bool Fixed
        {
            get
            {
                return mFixed;
            }

            set
            {
                mFixed = value;

                if( mFixed )
                {
                    Style.font = mFontFixed;
                    mFontInfo = Fonts.Fixed;
                }
                else
                {
                    Style.font = mFontVariable;
                    mFontInfo = Fonts.Variable;
                }
            }
        }

        float[] mSizes    = new float[7] { 11, 20, 18, 16, 14, 13, 12 };
        float   mFontSize = 11.0f;
        int     mSize     = 0;

        public int Size
        {
            get
            {
                return mSize;
            }

            set
            {
                mSize     = Mathf.Clamp( value, 0, mSizes.Length );
                mFontSize = mSizes[ mSize ];

                Style.fontSize = (int) mFontSize;
            }
        }

        string mLink = null;

        public string Link
        {
            get
            {
                return mLink;
            }

            set
            {
                mLink = value;
                Style.normal.textColor = string.IsNullOrEmpty( mLink ) ? Color.black : Color.blue;
            }
        }

        public string ToolTip;
        public GUIStyle Style;


        //------------------------------------------------------------------------------

        private Font        mFontVariable;
        private Font        mFontFixed;
        private FontInfo    mFontInfo;

        public RenderContext( GUISkin skin, Font fontVariable, Font fontFixed )
        {
            Style = new GUIStyle( skin.label );
            mFontVariable  = fontVariable;
            mFontFixed     = fontFixed;
        }

        internal bool CharacterWidth( char ch, out float advance )
        {
            return mFontInfo.GetAdvance( ch, out advance, mFontSize, Style.fontStyle );
        }

        internal void Reset()
        {
            Bold    = false;
            Italic  = false;
            Fixed   = false;
            Size    = 0;
            Link    = null;
            ToolTip = null;
        }
    }
}

