using System;
using System.Collections.Generic;

// https://gist.github.com/mjs3339/9b2cfe7f872c58c41435d6adfe1a9913

namespace MG.MDV
{
    public static class LZW
    {
        private static byte[] Add( byte[] left, byte[] right )
        {
            var l1  = left.Length;
            var l2  = right.Length;
            var ret = new byte[ l1 + l2 ];

            Buffer.BlockCopy( left, 0, ret, 0, l1 );
            Buffer.BlockCopy( right, 0, ret, l1, l2 );

            return ret;
        }

        private static int[] ToIntArray( byte[] ba )
        {
            var bal        = ba.Length;
            var int32Count = bal / 4 + (bal % 4 == 0 ? 0 : 1);
            var arr        = new int[ int32Count ];

            Buffer.BlockCopy( ba, 0, arr, 0, bal );

            return arr;
        }

        public static byte[] Decompress( this byte[] inBufferBytes )
        {
            if( inBufferBytes == null || inBufferBytes.Length == 0 )
            {
                throw new Exception( "Input buffer is invalid" );
            }

            var inBufferInts = ToIntArray( inBufferBytes );
            var iBuf  = new List<int>( inBufferInts );

            var dictionary = new Dictionary<int, List<byte>>();

            for( var i = 0; i < 256; i++ )
            {
                var e = new List<byte> { (byte) i };
                dictionary.Add( i, e );
            }

            var window = dictionary[ iBuf[0] ];
            iBuf.RemoveAt( 0 );
            var outBuffer = new List<byte>( window );

            foreach( var k in iBuf )
            {
                var entry = new List<byte>();

                if( dictionary.ContainsKey( k ) )
                {
                    entry.AddRange( dictionary[k] );
                }
                else if( k == dictionary.Count )
                {
                    entry.AddRange( Add( window.ToArray(), new[] { window.ToArray()[0] } ) );
                }

                if( entry.Count > 0 )
                {
                    outBuffer.AddRange( entry );
                    dictionary.Add( dictionary.Count, new List<byte>( Add( window.ToArray(), new[] { entry.ToArray()[0] } ) ) );
                    window = entry;
                }
            }

            return outBuffer.ToArray();
        }
    }
}