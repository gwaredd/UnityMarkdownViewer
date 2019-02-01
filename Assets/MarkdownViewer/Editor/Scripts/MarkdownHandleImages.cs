using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace MG.MDV
{
    public class HandlerImages
    {
        public string  CurrentPath;

        Texture                     mPlaceholder    = null;
        List<ImageRequest>          mActiveRequests = new List<ImageRequest>();
        Dictionary<string,Texture>  mTextureCache   = new Dictionary<string, Texture>();

        class ImageRequest
        {
            public string           URL; // original url
            public UnityWebRequest  Request;

            public ImageRequest( string url )
            {
                URL = url;
                Request = UnityWebRequestTexture.GetTexture( url );
                Request.SendWebRequest();
            }

            public Texture Texture
            {
                get
                {
                    var handler = Request.downloadHandler as DownloadHandlerTexture;
                    return handler != null ? handler.texture : null;
                }
            }
        }


        //------------------------------------------------------------------------------

        private string RemapURL( string url )
        {
            if( Regex.IsMatch( url, @"^\w+:", RegexOptions.Singleline ) )
            {
                return url;
            }

            var projectDir = Path.GetDirectoryName( Application.dataPath );

            if( url.StartsWith( "/" ) )
            {
                return string.Format( "file:///{0}{1}", projectDir, url );
            }

            var assetDir = Path.GetDirectoryName( CurrentPath );
            return "file:///" + Utils.PathNormalise( string.Format( "{0}/{1}/{2}", projectDir, assetDir, url ) );
        }

        //------------------------------------------------------------------------------

        public Texture FetchImage( string url )
        {
            url = RemapURL( url );

            Texture tex;

            if( mTextureCache.TryGetValue( url, out tex ) )
            {
                return tex;
            }

            if( mPlaceholder == null )
            {
                var style = GUI.skin.GetStyle( "btnPlaceholder" );
                mPlaceholder = style != null ? style.normal.background : null;
            }

            mActiveRequests.Add( new ImageRequest( url ) );
            mTextureCache[ url ] = mPlaceholder;

            return mPlaceholder;
        }

        //------------------------------------------------------------------------------

        public bool UpdateRequests()
        {
            var req = mActiveRequests.Find( r => r.Request.isDone );

            if( req == null )
            {
                return false;
            }

            if( req.Request.isHttpError )
            {
                Debug.LogError( string.Format( "HTTP Error: {0} - {1} {2}", req.URL, req.Request.responseCode, req.Request.error ) );
                mTextureCache[ req.URL ] = null;
            }
            else if( req.Request.isNetworkError )
            {
                Debug.LogError( string.Format( "Network Error: {0} - {1}", req.URL, req.Request.error ) );
                mTextureCache[ req.URL ] = null;
            }
            else
            {
                mTextureCache[ req.URL ] = req.Texture;
            }

            mActiveRequests.Remove( req );
            return true;
        }
    }
}
