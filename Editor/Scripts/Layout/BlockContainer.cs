using System;
using System.Collections.Generic;
using System.Linq;
using Markdig.Syntax;
using UnityEngine;

namespace MG.MDV
{
    public class BlockContainer : Block
    {
        public bool Quoted    = false;
        public bool Highlight = false;
        public bool Horizontal = false;
        public bool IsTableRow = false;
        public bool IsTableHeader = false;

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
            Rect.position = new Vector2( pos.x + Indent, pos.y );
            Rect.width = maxWidth - Indent - context.IndentSize;

            var paddingBottom = 0.0f;
            var paddingVertical = 0.0f;

            if( Highlight || IsTableHeader || IsTableRow )
            {
                GUIStyle style;

                if( Highlight )
                {
                    style = GUI.skin.GetStyle( Quoted ? "blockquote" : "blockcode" );
                }
                else
                {
                    style = GUI.skin.GetStyle( IsTableHeader ? "th" : "tr" );
                }

                pos.x += style.padding.left;
                pos.y += style.padding.top;

                maxWidth -= style.padding.horizontal;
                paddingBottom = style.padding.bottom;
                paddingVertical = style.padding.vertical;
            }

            if( Horizontal )
            {
                Rect.height = 0;
                maxWidth = mBlocks.Count == 0 ? maxWidth : maxWidth / mBlocks.Count;

                foreach( var block in mBlocks )
                {
                    block.Arrange( context, pos, maxWidth );
                    pos.x += block.Rect.width;
                    Rect.height = Mathf.Max( Rect.height, block.Rect.height );
                }

                Rect.height += paddingVertical;
            }
            else
            {
                foreach( var block in mBlocks )
                {
                    block.Arrange( context, pos, maxWidth );
                    pos.y += block.Rect.height;
                }

                Rect.height = pos.y - Rect.position.y + paddingBottom;
            }
        }

        public override void Draw( Context context )
        {
            if( Highlight && !Quoted )
            {
                GUI.Box( Rect, string.Empty, GUI.skin.GetStyle( "blockcode" ) );
            }
            else if( IsTableHeader )
            {
                GUI.Box( Rect, string.Empty, GUI.skin.GetStyle( "th" ) );
            }
            else if( IsTableRow )
            {
                var parentBlock = Parent as BlockContainer;
                if( parentBlock == null )
                {
                    GUI.Box( Rect, string.Empty, GUI.skin.GetStyle( "tr" ) );
                }
                else
                {
                    var idx = parentBlock.mBlocks.IndexOf(this);
                    GUI.Box( Rect, string.Empty, GUI.skin.GetStyle( idx % 2 == 0 ? "tr" : "trl" ) );
                }
            }

            mBlocks.ForEach( block => block.Draw( context ) );

            if( Highlight && Quoted )
            {
                GUI.Box( Rect, string.Empty, GUI.skin.GetStyle( "blockquote" ) );
            }
        }

        public void RemoveTrailingSpace()
        {
            if( mBlocks.Count > 0 && mBlocks[ mBlocks.Count - 1 ] is BlockSpace )
            {
                mBlocks.RemoveAt( mBlocks.Count - 1 );
            }
        }
    }
}

