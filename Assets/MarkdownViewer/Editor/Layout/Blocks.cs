////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MG.MDV
{
    ////////////////////////////////////////////////////////////////////////////////
    // block

    public abstract class Block
    {
        public string   ID      = null;
        public Rect     Rect    = new Rect();
        public Block    Parent  = null;
        public float    Indent  = 0.0f;

        public abstract void Arrange( Context context, Vector2 anchor, float maxWidth );
        public abstract void Draw( Context context );

        public Block( float indent )
        {
            Indent = indent;
        }

        public virtual Block Find( string id )
        {
            return id.Equals( ID, StringComparison.Ordinal ) ? this : null;
        }
    }

    //------------------------------------------------------------------------------
    // <div>

    public class BlockContainer : Block
    {
        public bool Quote = false;

        List<Block> mBlocks = new List<Block>();

        public BlockContainer( float indent ) : base( indent ) { }

        public Block Add( Block block )
        {
            block.Parent = this;
            mBlocks.Add( block );
            return block;
        }

        public override Block Find( string id )
        {
            if( id.Equals( ID, StringComparison.Ordinal ) )
            {
                return this;
            }

            foreach( var block in mBlocks )
            {
                var match = block.Find( id );

                if( match != null )
                {
                    return match;
                }
            }

            return null;
        }

        public override void Arrange( Context context, Vector2 pos, float maxWidth )
        {
            float oy = pos.y;

            Rect.position = pos;

            var padding = 0.0f;

            if( Quote )
            {
                Rect.x += Indent;
                padding = context.LineHeight * 0.5f;
                pos += new Vector2( context.IndentSize, padding );
            }

            foreach( var block in mBlocks )
            {
                block.Arrange( context, pos, maxWidth );
                pos.y += block.Rect.height;
            }

            Rect.width  = maxWidth - Indent;
            Rect.height = pos.y - oy + padding;
        }

        public override void Draw( Context context )
        {
            if( Quote )
            {
                GUI.Box( Rect, string.Empty );
            }

            mBlocks.ForEach( block => block.Draw( context ) );
        }

        public void RemoveTrailingSpace()
        {
            if( mBlocks.Count > 0 && mBlocks[ mBlocks.Count - 1 ] is BlockSpace )
            {
                mBlocks.RemoveAt( mBlocks.Count - 1 );
            }
        }
    }


    //------------------------------------------------------------------------------
    // <hr/>

    public class BlockLine : Block
    {
        public BlockLine( float indent ) : base( indent ) {}

        public override void Draw( Context context )
        {
            var rect = new Rect( Rect.position.x, Rect.center.y, Rect.width, 1.0f );
            GUI.Label( rect, string.Empty, GUI.skin.GetStyle( "hr" ) );
        }

        public override void Arrange( Context context, Vector2 pos, float maxWidth )
        {
            Rect.position = pos;
            Rect.width    = maxWidth;
            Rect.height   = 10.0f;
        }
    }


    //------------------------------------------------------------------------------
    // <br/>

    public class BlockSpace : Block
    {
        public BlockSpace( float indent ) : base( indent ) { }

        public override void Draw( Context context )
        {
        }

        public override void Arrange( Context context, Vector2 pos, float maxWidth )
        {
            Rect.position = pos;
            Rect.width    = 1.0f;
            Rect.height   = context.LineHeight * 0.75f;
        }
    }


    //------------------------------------------------------------------------------
    // <p>

    public class BlockContent : Block
    {
        Content       mPrefix    = null;
        List<Content> mContent   = new List<Content>();

        public bool IsEmpty { get { return mContent.Count == 0; } }

        public BlockContent( float indent ) : base( indent ) { }

        public void Add( Content content )
        {
            mContent.Add( content );
        }

        public void Prefix( Content content )
        {
            mPrefix = content;
        }

        public override void Arrange( Context context, Vector2 pos, float maxWidth )
        {
            var origin = pos;

            pos.x += Indent;
            maxWidth = Mathf.Max( maxWidth - Indent, context.MinWidth );

            Rect.position = pos;

            // prefix

            if( mPrefix != null )
            {
                mPrefix.Location.x = pos.x - context.IndentSize * 0.5f;
                mPrefix.Location.y = pos.y;
            }

            // content

            if( mContent.Count == 0 )
            {
                Rect.width  = 0.0f;
                Rect.height = 0.0f;
                return;
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

            Rect.width  = maxWidth;
            Rect.height = pos.y - origin.y;
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
            mContent.ForEach( c => c.Draw( context ) );
            mPrefix?.Draw( context );
        }
    }


    ////////////////////////////////////////////////////////////////////////////////
    // Content

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

    //------------------------------------------------------------------------------

    public class ContentText : Content
    {
        public ContentText( GUIContent payload, Style style, string link )
            : base( payload, style, link )
        {
        }
    }

    //------------------------------------------------------------------------------

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

