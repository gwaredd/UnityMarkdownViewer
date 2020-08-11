using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

namespace MG.MDV
{
    public class ImageGif
    {
        public struct BlockData
        {
            public byte   Size;
            public byte[] Data;
        }

        public struct ImageBlock
        {
            public ushort   Left;
            public ushort   Top;
            public ushort   Width;
            public ushort   Height;
            public bool     InterlaceFlag;
            public bool     SortFlag;
            public byte     LzwMinimumCodeSize;

            public List<byte[]>     ColourTable;
            public List<BlockData>  ImageData;
        }

        public struct GraphicControlExtension
        {
            public byte     BlockSize;
            public ushort   DisposalMethod;
            public bool     TransparentColour;
            public ushort   Delay;
            public byte     TransparentColourIndex;
            public byte     BlockTerminator;
        }

        public ushort   Width              { get; private set; }
        public ushort   Height             { get; private set; }
        public int      BitDepth           { get; private set; }
        public bool     SortFlag           { get; private set; }
        public byte     BackgroundIndex    { get; private set; }
        public byte     AspectRatio        { get; private set; }

        public List<byte[]> ColourTable;

        public List<ImageBlock> Images;
        public List<GraphicControlExtension> GraphicsExt;


        //------------------------------------------------------------------------------

        public static ImageGif Create( byte[] data )
        {
            try
            {
                return new ImageGif().Decode( data );
            }
            catch( Exception e )
            {
                Debug.Log( e.Message );
                return null;
            }
        }

        //------------------------------------------------------------------------------

        public ImageGif Decode( byte[] data )
        {
            if( data == null || data.Length <= 12 )
            {
                throw new Exception( "Invalid data" );
            }

            using( var r = new BinaryReader( new MemoryStream( data ) ) )
            {
                ReadHeader( r );
                ReadBlocks( r );
            }

            foreach( var img in Images )
            {
                // TODO: create texture
            }

            return this;
        }

        //------------------------------------------------------------------------------

        protected void ReadHeader( BinaryReader r )
        {
            // signature

            if( new string( r.ReadChars( 3 ) ) != "GIF" )
            {
                throw new Exception( "No GIF signature at start of data" );
            }

            // version

            var version = new string( r.ReadChars( 3 ) );

            if( version != "87a" && version != "89a" )
            {
                throw new Exception( "Unsupported GIF version" );
            }

            // read header

            Width           = r.ReadUInt16();
            Height          = r.ReadUInt16();
            var flags       = r.ReadByte();
            BackgroundIndex = r.ReadByte();
            AspectRatio     = r.ReadByte();
            SortFlag        = ( flags & 0x08 ) == 0x08;
            BitDepth        = ( flags & 0x70 ) >> 4 + 1;

            // colour table?

            var hasColourTable = ( flags & 0x80 ) == 0x80;

            if( hasColourTable )
            {
                var tableSize = (int) Math.Pow( 2, ( flags & 0x07 ) + 1 );

                ColourTable = new List<byte[]>( tableSize );

                for( var i=0; i < tableSize; i++ )
                {
                    ColourTable.Add( r.ReadBytes( 3 ) );
                }
            }
        }

        //------------------------------------------------------------------------------

        protected void ReadBlocks( BinaryReader r )
        {
            while( true )
            {
                switch( r.ReadByte() )
                {
                    case 0x2C:
                        ReadImageBlock( r );
                        break;

                    case 0x21:

                        switch( r.ReadByte() )
                        {
                            case 0xf9:
                                ReadGraphicControlExtension( r );
                                break;

                            // comments
                            case 0xfe:
                                SkipBlockData( r );
                                break;

                            // plain text
                            case 0x01:
                                SkipPlainText( r );
                                break;

                            case 0xff:
                                SkipApplicationExt( r );
                                break;

                            default:
                                break;
                        }

                        break;

                    case 0x3B:
                        return;

                    default:
                        throw new Exception( "Unexpected block type" );
                }
            }
        }


        //------------------------------------------------------------------------------

        protected void ReadImageBlock( BinaryReader r )
        {
            var ib = new ImageBlock();

            ib.Left   = r.ReadUInt16();
            ib.Top    = r.ReadUInt16();
            ib.Width  = r.ReadUInt16();
            ib.Height = r.ReadUInt16();

            var flags = r.ReadByte();

            ib.InterlaceFlag = ( flags & 64 ) == 64;
            ib.SortFlag = ( flags & 32 ) == 32;

            var hasColourTable = ( flags & 0x80 ) == 0x80;

            if( hasColourTable )
            {
                var tableSize = (int) Math.Pow( 2, ( flags & 0x07 ) + 1 );

                ib.ColourTable = new List<byte[]>( tableSize );

                for( var i=0; i < tableSize; i++ )
                {
                    ib.ColourTable.Add( r.ReadBytes( 3 ) );
                }
            }

            ib.LzwMinimumCodeSize = r.ReadByte();

            ib.ImageData = ReadBlockData( r );

            if( Images == null )
            {
                Images = new List<ImageBlock>();
            }

            Images.Add( ib );
        }


        //------------------------------------------------------------------------------

        private void ReadGraphicControlExtension( BinaryReader r )
        {
            var ext = new GraphicControlExtension();

            ext.BlockSize = r.ReadByte();

            var flags = r.ReadByte();

            switch( flags & 28 )
            {
                case 4:
                    ext.DisposalMethod = 1; // do not dispose
                    break;
                case 8:
                    ext.DisposalMethod = 2; // restore background colour
                    break;
                case 12:
                    ext.DisposalMethod = 3; // return to previous
                    break;
                default:
                    ext.DisposalMethod = 0; // none
                    break;
            }

            ext.TransparentColour       = ( flags & 1 ) == 1;
            ext.Delay                   = r.ReadUInt16();
            ext.TransparentColourIndex  = r.ReadByte();
            ext.BlockTerminator         = r.ReadByte();

            if( GraphicsExt == null )
            {
                GraphicsExt = new List<GraphicControlExtension>();
            }

            GraphicsExt.Add( ext );
        }


        //------------------------------------------------------------------------------

        private List<BlockData> ReadBlockData( BinaryReader r )
        {
            var blocks = new List<BlockData>();

            byte blockSize = r.ReadByte();

            while( blockSize != 0x00 )
            {
                var data = new BlockData();
                data.Size = blockSize;
                data.Data = r.ReadBytes( blockSize );
                blocks.Add( data );

                blockSize = r.ReadByte();
            }

            return blocks;
        }

        private void SkipBlockData( BinaryReader r )
        {
            byte blockSize = r.ReadByte();

            while( blockSize != 0x00 )
            {
                r.ReadBytes( blockSize );
                blockSize = r.ReadByte();
            }
        }

        private void SkipPlainText( BinaryReader r )
        {
            byte blockSize = r.ReadByte();

            r.ReadBytes( 12 );
            SkipBlockData( r );
        }

        private void SkipApplicationExt( BinaryReader r )
        {
            byte blockSize = r.ReadByte();

            r.ReadBytes( 11 );
            SkipBlockData( r );
        }
    }
}
