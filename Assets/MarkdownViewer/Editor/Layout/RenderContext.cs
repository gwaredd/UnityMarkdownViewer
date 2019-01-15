////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace MG.MDV
{
    public interface IRenderContext
    {
        bool     GetCharacter( char inChar, out char outChar, out float outWidth );
        GUIStyle Apply( LayoutStyle style );
        void     Reset();
    }


    ////////////////////////////////////////////////////////////////////////////////

    public class RenderContext : IRenderContext
    {
        private GUIStyle    mStyle;
        private Font        mFontVariable;
        private Font        mFontFixed;
        private FontInfo    mFontInfo;

        private bool        mFixed    = false;
        private int         mSize     = 0;
        private float       mFontSize = 11.0f;

        public RenderContext( GUISkin skin, Font fontVariable, Font fontFixed )
        {
            mStyle        = new GUIStyle( skin.label );
            mFontVariable = fontVariable;
            mFontFixed    = fontFixed;
            mFontSize     = mStyle.fontSize;
            mFontInfo     = Fonts.Variable;
            mFixed        = false;
        }

        public void Reset()
        {
            Apply( new LayoutStyle() );
        }


        //------------------------------------------------------------------------------

        public bool GetCharacter( char inChar, out char outChar, out float outWidth )
        {
            outWidth = 0.0f;
            outChar  = inChar;

            // chars to ignore

            if( inChar == '\r' )
            {
                return false;
            }

            // chars to covert

            if( char.IsWhiteSpace( inChar ) )
            {
                outChar = ' ';
            }

            // lookup glyph from font

#warning TODO: replace with GUIStyle.CalcSize!

            if( mFontInfo.GetAdvance( inChar, out outWidth, mFontSize, mStyle.fontStyle ) == false )
            {
                // if glyph not found

                outChar = '?';
                mFontInfo.GetAdvance( '?', out outWidth, mFontSize, mStyle.fontStyle );
            }

            return true;
        }


        //------------------------------------------------------------------------------

        float[] mHeaderSizeToFontSize = new float[7] { 11, 20, 18, 16, 14, 13, 12 };

        public GUIStyle Apply( LayoutStyle style )
        {
            if( style.Bold )
            {
                mStyle.fontStyle = style.Italic ? FontStyle.BoldAndItalic : FontStyle.Bold;
            }
            else
            {
                mStyle.fontStyle = style.Italic ? FontStyle.Italic : FontStyle.Normal;
            }

            mStyle.normal.textColor = style.Link ? Color.blue : Color.black;

            if( mSize != style.Size )
            {
                mSize           = style.Size;
                mFontSize       = mHeaderSizeToFontSize[ mSize ];
                mStyle.fontSize = (int) mFontSize;
            }

            if( style.Fixed != mFixed )
            {
                mFixed      = style.Fixed;
                mStyle.font = mFixed ? mFontFixed : mFontVariable;
                mFontInfo   = mFixed ? Fonts.Fixed : Fonts.Variable;
            }

            return mStyle;
        }
    }
}
