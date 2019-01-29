////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MG.MDV
{
    public class Container
    {
        public List<Row> Rows = new List<Row>();

        public Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var row in Rows )
            {
                var size = row.Arrange( context, pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public class Row
    {
        public List<Col> Cols = new List<Col>();

        public Row Add( Col col )
        {
            Cols.Add( col );
            return this;
        }

        public Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var col in Cols )
            {
                var size = col.Arrange( context, pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public class Col
    {
        public List<Block> Blocks = new List<Block>();

        public bool IsEmpty { get { return Blocks.Count == 0; } }

        public Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            foreach( var block in Blocks )
            {
                var size = block.Arrange( context, pos, maxWidth );
                pos.y += size.y;
            }

            return new Vector2( maxWidth, pos.y - oy );
        }
    }

    //------------------------------------------------------------------------------

    public abstract class Block
    {
        public Block Parent = null;
        public float Indent = 0.0f;

        public abstract Vector2 Arrange( Context context, Vector2 pos, float maxWidth );
        public abstract void Draw( Context context );
    }

    public class BlockContainer : Block
    {
        public bool Quote = false;
        List<Block> mBlocks = new List<Block>();

        public override Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            throw new System.NotImplementedException();
        }

        public override void Draw( Context context )
        {
            throw new System.NotImplementedException();
        }

        public Block Add( Block block )
        {
            block.Parent = this;
            mBlocks.Add( block );
            return block;
        }
    }

    //------------------------------------------------------------------------------
    // hr

    public class BlockLine : Block
    {
        private Rect Rect = new Rect();

        public override void Draw( Context context )
        {
            var rect = new Rect( Rect.position.x, Rect.center.y, Rect.width, 1.0f );
            GUI.Label( rect, string.Empty, GUI.skin.GetStyle( "hr" ) );
        }

        public override Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            Rect.position = pos;
            Rect.width = maxWidth;
            Rect.height = 10.0f;

            return Rect.size;
        }
    }

    //------------------------------------------------------------------------------

    public class BlockSpace : Block
    {
        public override void Draw( Context context )
        {
        }

        public override Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            return new Vector2( 1.0f, context.LineHeight );
        }
    }

    //------------------------------------------------------------------------------
    // <div>..</div>

    public class BlockContent : Block
    {
        Rect          mRect      = new Rect();
        Content       mPrefix    = null;
        List<Content> mContent   = new List<Content>();

        public bool IsEmpty { get { return mContent.Count == 0; } }
        public bool Highlight  = false;

        public BlockContent( float indent )
        {
            Indent = indent;
        }

        public void Add( Content content )
        {
            mContent.Add( content );
        }

        public void Prefix( Content content )
        {
            mPrefix = content;
        }

        public override Vector2 Arrange( Context context, Vector2 pos, float maxWidth )
        {
            var origin = pos;

            mRect.position = pos;

            pos.x += Indent;
            maxWidth = Mathf.Max( maxWidth - Indent, context.MinWidth );

            // prefix

            if( mPrefix != null )
            {
                mPrefix.Location.x = pos.x - context.IndentSize * 0.5f;
                mPrefix.Location.y = pos.y;
            }

            // content

            if( mContent.Count == 0 )
            {
                return Vector2.zero;
            }

            mContent.ForEach( c => c.Update( context ) );

            var rowWidth   = mContent[0].Width;
            var rowHeight  = mContent[0].Height;
            var startIndex = 0;

            for( var i = 1; i < mContent.Count; i++ )
            {
                var content = mContent[i];

                if( rowWidth + content.Width > maxWidth )
                {
                    LayoutRow( pos, startIndex, i, rowHeight );
                    pos.y += rowHeight;

                    startIndex = i;
                    rowWidth = content.Width;
                    rowHeight = content.Height;
                }
                else
                {
                    rowWidth += content.Width;
                    rowHeight = Mathf.Max( rowHeight, content.Height );
                }
            }

            if( startIndex < mContent.Count )
            {
                LayoutRow( pos, startIndex, mContent.Count, rowHeight );
                pos.y += rowHeight;
            }

            mRect.size = new Vector2( maxWidth, pos.y - origin.y );

            return mRect.size;
        }

        void LayoutRow( Vector2 pos, int from, int until, float rowHeight )
        {
            for( var i = from; i < until; i++ )
            {
                var content = mContent[i];

                content.Location.x = pos.x;
                content.Location.y = pos.y + rowHeight - content.Height;

                pos.x += content.Width;
            }
        }

        public override void Draw( Context context )
        {
            if( Highlight )
            {
                GUI.Box( mRect, string.Empty );
            }

            mContent.ForEach( c => c.Draw( context ) );
            mPrefix?.Draw( context );
        }
    }

    //------------------------------------------------------------------------------

    public abstract class Content
    {
        public Rect         Location;
        public Style        Style;
        public GUIContent   Payload;
        public string       Link;

        public float Width  { get { return Location.width; } }
        public float Height { get { return Location.height; } }

        public Content( GUIContent payload, Style style, string link )
        {
            Payload = payload;
            Style   = style;
            Link    = link;
        }

        public void CalcSize( Context context )
        {
            Location.size = context.CalcSize( Payload );
        }

        public void Draw( Context context )
        {
            if( string.IsNullOrEmpty( Link ) )
            {
                GUI.Label( Location, Payload, context.Apply( Style ) );
            }
            else if( GUI.Button( Location, Payload, context.Apply( Style ) ) )
            {
                if( Regex.IsMatch( Link, @"^\w+:", RegexOptions.Singleline ) )
                {
                    Application.OpenURL( Link );
                }
                else
                {
                    context.Actions.SelectPage( Link );
                }
            }
        }

        public virtual void Update( Context context )
        {
        }
    }

    public class ContentText : Content
    {
        public ContentText( GUIContent payload, Style style, string link )
            : base( payload, style, link )
        {
        }
    }

    public class ContentImage : Content
    {
        public string URL;
        public string Alt;

        public ContentImage( GUIContent payload, Style style, string link )
            : base( payload, style, link )
        {
        }

        public override void Update( Context context )
        {
            Payload.image = context.Actions.FetchImage( URL );
            Payload.text = null;

            if( Payload.image == null )
            {
                context.Apply( Style );
                var text = !string.IsNullOrEmpty( Alt ) ? Alt : URL;
                Payload.text = $"[{text}]";
            }

            Location.size = context.CalcSize( Payload );
        }
    }
}

