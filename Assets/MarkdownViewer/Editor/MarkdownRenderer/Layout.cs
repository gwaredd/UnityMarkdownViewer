////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MG.MDV
{
    /******************************************************************************
     * Grid
     ******************************************************************************/

    public class LayoutGrid
    {
        public List<LayoutRow> Rows = new List<LayoutRow>();

        public void Add( LayoutRow row )
        {
            Rows.Add( row );
        }
    }

    public class LayoutRow
    {
        public List<LayoutCol> Cols = new List<LayoutCol>();

        public void Add( LayoutCol col )
        {
            Cols.Add( col );
        }
    }

    public class LayoutCol
    {
        public float MaxWidth = 100.0f;
#warning TODO: container width
        public float Width { get { return MaxWidth; } }

        public List<LayoutLine> Lines = new List<LayoutLine>();

        public bool IsEmpty { get { return Lines.Count == 0; } }

        public void NewLine()
        {
            if( Lines.Count == 0 || !Lines.Last().IsEmpty )
            {
                Lines.Add( new LayoutLine() );
            }
        }

        public void Add( LayoutSegment seg )
        {
            if( seg == null || seg.Width <= 0.0f )
            {
                return;
            }

            if( Lines.Count == 0 || Lines.Last().Width + seg.Width > MaxWidth )
            {
                NewLine();
            }

            Lines.Last().Add( seg );
        }

        public void Draw( Vector2 pos, IRenderContext context )
        {
            foreach( var line in Lines )
            {
                line.Draw( pos );
                pos.y += line.Height;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////////////

    public class LayoutLine
    {
        public List<LayoutSegment> Segments = new List<LayoutSegment>();

        public bool  IsEmpty { get { return Segments.Count == 0; } }

        public float Width   { get; protected set; }
        public float Height  { get; protected set; }

        public void Add( LayoutSegment seg )
        {
            Segments.Add( seg );
            Height = Mathf.Max( Height, seg.Height );
        }

        public void Draw( Vector2 pos )
        {
            foreach( var seg in Segments )
            {
                seg.Draw( pos );
                pos.x += seg.Width;
            }
        }
    }


    /******************************************************************************
     * Segments
     ******************************************************************************/

    public abstract class LayoutSegment
    {
        public Vector2 Size;
        public abstract void Draw( Vector2 pos );

        public float Width  { get { return Size.x; } }
        public float Height { get { return Size.y; } }

        protected Layout     mLayout { get; private set; }
        protected GUIContent mContent = new GUIContent();

        public LayoutSegment( Layout layout )
        {
            mLayout = layout;
        }
    }

    //------------------------------------------------------------------------------

    public class LayoutSegmentText : LayoutSegment
    {
        protected LayoutStyle mStyle;

        public LayoutSegmentText( Layout layout, float width, string text )
            : base( layout )
        {
            Size.x        = width;
            Size.y        = layout.CurrentLineHeight;
            mStyle        = layout.CurrentStyle;
            mContent.text = text;
        }

        public override void Draw( Vector2 pos )
        {
            GUI.Label( new Rect( pos, Size ), mContent, mLayout.Apply( mStyle ) );
        }
    }

    //------------------------------------------------------------------------------

    public class LayoutSegmentLink : LayoutSegmentText
    {
        public string URL = null;

        public LayoutSegmentLink( Layout layout, float width, string text )
            : base( layout, width, text )
        {
            mContent.tooltip = mLayout.ContextTooltip;
            URL = mLayout.ContextLink;
        }

        public override void Draw( Vector2 pos )
        {
            if( GUI.Button( new Rect( pos, Size ), mContent, mLayout.Apply( mStyle ) ) )
            {
                if( Regex.IsMatch( URL, @"^\w+:", RegexOptions.Singleline ) )
                {
                    Application.OpenURL( URL );
                }
                else
                {
                    mLayout.SelectPage( URL );
                }
            }
        }
    }

    //------------------------------------------------------------------------------

    public class LayoutSegmentImage : LayoutSegment
    {
        public LayoutSegmentImage( Layout layout, Texture tex, string tooltip )
            : base( layout )
        {
            mContent.text    = null;
            mContent.image   = tex;
            mContent.tooltip = tooltip;

            Size.x = tex.width;
            Size.y = tex.height;
        }

        public override void Draw( Vector2 pos )
        {
            GUI.Label( new Rect( pos, Size ), mContent );
        }
    }

    //------------------------------------------------------------------------------

    public class LayoutSegmentSeparator : LayoutSegment
    {
        public LayoutSegmentSeparator( Layout layout )
            : base( layout )
        {
        }

        public override void Draw( Vector2 pos )
        {
            var rect = new Rect( pos.x, pos.y, mLayout.CurrentContainerWidth, 1.0f );
            GUI.Label( rect, string.Empty, GUI.skin.GetStyle( "hr" ) );
        }
    }


    /******************************************************************************
     * Layout
     ******************************************************************************/

    public class Layout
    {
        IActionHandlers          mActions        = null;

        private List<LayoutGrid> mGrids          = new List<LayoutGrid>();
        private LayoutCol        mCurrentCol     = null;
        private float            mMaxWidth       = 300.0f;

        private IRenderContext   mContext        = null;
        private SegmentBuilder   mSegmentBuilder = null;

        public Layout( float maxWidth, IRenderContext context, IActionHandlers actions )
        {
            mActions         = actions;

            mContext         = context;
            mSegmentBuilder  = new SegmentBuilder( mContext );
            mMaxWidth        = maxWidth;

            mSegmentBuilder.MaxWidth = maxWidth; // TODO: ?

            var col  = new LayoutCol();
            var row  = new LayoutRow();
            var grid = new LayoutGrid();

            row.Add( col );
            grid.Add( row );
            mGrids.Add( grid );

            mCurrentCol = col;
            mCurrentCol.MaxWidth = maxWidth;

            mSegmentBuilder.OnCreate  = OnCreateSegment;
            mSegmentBuilder.OnNewLine = () => mCurrentCol.NewLine();
        }


        //------------------------------------------------------------------------------

        public void Draw()
        {
            mContext.Reset();

            foreach( var grid in mGrids )
            {
                foreach( var row in grid.Rows )
                {
                    foreach( var col in row.Cols )
                    {
                        mCurrentCol = col;
                        col.Draw( Vector2.zero, mContext );
                    }
                }
            }
        }


        //------------------------------------------------------------------------------

        LayoutStyle mLayoutStyle = new LayoutStyle();
        GUIStyle    mGUIStyle    = null;

        public LayoutStyle CurrentStyle          { get { return mLayoutStyle; } }
        public float       CurrentLineHeight     { get { return mGUIStyle.lineHeight; } }
        public float       CurrentContainerWidth { get { return mCurrentCol.Width; } }
        public string      ContextTooltip        { get; private set; }
        public string      ContextLink           { get; private set; }


        //------------------------------------------------------------------------------

        public void Text( string text, LayoutStyle style, string link, string toolTip )
        {
            mGUIStyle      = mContext.Apply( style );
            mLayoutStyle   = style;
            ContextLink    = link;
            ContextTooltip = toolTip;

            mSegmentBuilder.Print( mLayoutStyle, text );
        }

        private void OnCreateSegment( string text, float width )
        {
            if( string.IsNullOrWhiteSpace( ContextLink ) )
            {
                mCurrentCol.Add( new LayoutSegmentText( this, width, text ) );
            }
            else
            {
                mCurrentCol.Add( new LayoutSegmentLink( this, width, text ) );
            }
        }


        //------------------------------------------------------------------------------

        public void Image( string url, string alt, string title )
        {
            var tex = mActions.FetchImage( url );

            ContextTooltip = !string.IsNullOrEmpty( title ) ? title : alt;

            if( tex != null )
            {
                mCurrentCol.Add( new LayoutSegmentImage( this, tex, ContextTooltip ) );
            }
            else
            {
                var text = !string.IsNullOrEmpty( alt ) ? alt : url;
                mSegmentBuilder.Print( mLayoutStyle, $"[{text}]" );
            }
        }

        public void HorizontalLine()
        {
            mCurrentCol.NewLine();
            mCurrentCol.Add( new LayoutSegmentSeparator( this ) );
            mCurrentCol.NewLine();
        }

        public void NewLine()
        {
            mCurrentCol.NewLine();
        }

        public void Indent()
        {
            Debug.Log( "TODO: Indent" );
        }

        public void Outdent()
        {
            Debug.Log( "TODO: Outdent" );
        }

        public void Prefix( string marker )
        {
            Debug.Log( "TODO: Prefix" );
        }


        //------------------------------------------------------------------------------

        internal GUIStyle Apply( LayoutStyle mStyle )
        {
            return mContext.Apply( mStyle );
        }

        internal void SelectPage( string url )
        {
            mActions.SelectPage( url );
        }
    }
}
