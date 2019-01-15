////////////////////////////////////////////////////////////////////////////////

using System.Text;
using System;

namespace MG.MDV
{
    internal class SegmentBuilder
    {
        IRenderContext mContext;

        StringBuilder mWord         = new StringBuilder( 1024 );
        float         mWordWidth    = 0.0f;

        StringBuilder mSegment      = new StringBuilder( 1024 );
        float         mSegmentWidth = 0.0f;


        public SegmentBuilder( IRenderContext context )
        {
            mContext = context;
        }


        ////////////////////////////////////////////////////////////////////////////////
        // public

        public Action<string,float> OnCreate  = null;
        public Action               OnNewLine = null;

        public float MaxWidth
        {
            get;
            set;
        }


        //------------------------------------------------------------------------------

        public void Flush()
        {
            if( mWord.Length > 0 )
            {
                AddWord();
            }

            if( mSegment.Length > 0 )
            {
                CreateSegment();
            }
        }

        public void Print( LayoutStyle style, string text )
        {
            mContext.Apply( style );

            for( var i = 0; i < text.Length; i++ )
            {
                AddCharacter( text[ i ] );
            }

            Flush();
        }


        //------------------------------------------------------------------------------

        private void AddCharacter( char inChar )
        {
            if( inChar == '\n' )
            {
                Flush();
                OnNewLine?.Invoke();
                return;
            }

            float width;
            char  ch;

            if( mContext.GetCharacter( inChar, out ch, out width ) == false )
            {
                return;
            }

            mWord.Append( ch );
            mWordWidth += width;

            if( ch == ' ' )
            {
                AddWord();
            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        // private

        private void AddWord()
        {
            // TODO: split long words?
            // TODO: some safety for narrow windows?!

            if( mSegmentWidth + mWordWidth > MaxWidth )
            {
                CreateSegment();
            }

            mSegment.Append( mWord.ToString() );
            mSegmentWidth += mWordWidth;

            mWord.Clear();
            mWordWidth = 0.0f;
        }

        private void CreateSegment()
        {
            if( mSegment.Length == 0 )
            {
                return;
            }

            OnCreate?.Invoke( mSegment.ToString(), mSegmentWidth );

            mSegment.Clear();
            mSegmentWidth = 0.0f;
         }
    }
}
