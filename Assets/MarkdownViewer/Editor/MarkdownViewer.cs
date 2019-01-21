////////////////////////////////////////////////////////////////////////////////

using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace MG.MDV
{
    public interface IActionHandlers
    {
        Texture FetchImage( string url );
        void    SelectPage( string url );
    }


    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownViewer : Editor, IActionHandlers
    {
        public GUISkin      Skin;
        public StyleConfig  StyleConfig;
        public Texture      Placeholder;
        public Texture      IconMarkdown;
        public Texture      IconRaw;

        //private static Stack<string> History = new Stack<string>();

        //------------------------------------------------------------------------------

        public void SelectPage( string url )
        {
            if( string.IsNullOrEmpty( url ) )
            {
                return;
            }

            var assetPath = string.Empty;

            if( url.StartsWith( "Assets/", StringComparison.OrdinalIgnoreCase ) )
            {
                assetPath = url;
            }
            else
            {
                var targetPath = AssetDatabase.GetAssetPath( target );
                var targetDir  = Path.GetDirectoryName( targetPath );

                assetPath = Utils.PathCombine( targetDir, url );
            }

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>( assetPath );

            if( asset != null )
            {
                Selection.activeObject = asset;
            }
            else
            {
                Debug.LogError( $"Could not find asset {assetPath}" );
            }
        }

        //------------------------------------------------------------------------------

        class ImageRequest
        {
            public string           URL; // original url
            public UnityWebRequest  Request;

            public ImageRequest( string url )
            {
                URL     = url;
                Request = UnityWebRequestTexture.GetTexture( url );

                // TODO: images with redirect - i.e. loremflickr
                //                 Request.
                // 
                //                 Debug.Log( "Location: " + www.responseHeaders[ "Location" ] );
                //                 Debug.Log( "Cookie: " + www.responseHeaders[ "Set-Cookie" ] );

                Request.SendWebRequest();
            }

            public Texture Texture
            {
                get
                {
                    var handler = Request.downloadHandler as DownloadHandlerTexture;
                    return handler?.texture;
                }
            }
        }

        List<ImageRequest> mActiveRequests = new List<ImageRequest>();
        Dictionary<string,Texture> mTextureCache = new Dictionary<string, Texture>();

        private void OnEnable()
        {
            EditorApplication.update += UpdateRequests;
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateRequests;
        }

        private string RemapURL( string url )
        {
            if( Regex.IsMatch( url, @"^\w+:", RegexOptions.Singleline ) )
            {
                return url;
            }

            var projectDir = Path.GetDirectoryName( Application.dataPath );

            if( url.StartsWith( "/" ) )
            {
                return $"file:///{projectDir}{url}";
            }

            var assetDir = Path.GetDirectoryName( AssetDatabase.GetAssetPath( target ) );
            return Utils.PathNormalise( $"file:///{projectDir}/{assetDir}/{url}" );
        }

        public Texture FetchImage( string url )
        {
            url = RemapURL( url );

            Texture tex;

            if( mTextureCache.TryGetValue( url, out tex ) )
            {
                return tex;
            }

            mActiveRequests.Add( new ImageRequest( url ) );
            mTextureCache[ url ] = Placeholder;

            return Placeholder;
        }

        void UpdateRequests()
        {
            if( mActiveRequests.Count == 0 )
            {
                return;
            }

            var req = mActiveRequests.Find( r => r.Request.isDone );

            if( req == null )
            {
                return;
            }
            
            if( req.Request.isHttpError )
            {
                Debug.LogError( $"HTTP Error: {req.URL} - {req.Request.responseCode} {req.Request.error}" );
                mTextureCache[ req.URL ] = null;
            }
            else if( req.Request.isNetworkError )
            {
                Debug.LogError( $"Network Error: {req.URL} - {req.Request.error}" );
                mTextureCache[ req.URL ] = null;
            }
            else
            {
                mTextureCache[ req.URL ] = req.Texture;
            }

            mActiveRequests.Remove( req );
            Repaint();
        }


        //------------------------------------------------------------------------------
        // TOOD: add preview

        public override bool UseDefaultMargins()
        {
            return false;
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();

            if( Event.current.type == EventType.Repaint )
            {
                mHeaderHeight = GUILayoutUtility.GetLastRect().height;
            }
        }

        private Editor mDefaultEditor;

        public override void OnInspectorGUI()
        {
            // file extension is .md

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) )
            {
                ParseDocument();
                DrawIMGUI();
            }
            else
            {
                mDefaultEditor = mDefaultEditor ?? CreateEditor( target, Type.GetType( "UnityEditor.TextAssetInspector, UnityEditor" ) );
                mDefaultEditor?.OnInspectorGUI();
            }
        }


        //------------------------------------------------------------------------------

        void ParseDocument()
        {
            if( mLayout != null )
            {
                return;
            }

            mLayout = new Layout( new StyleCache( Skin, StyleConfig ), this );

            var renderer = new RendererMarkdown( mLayout );

            // TODO: look at pipeline options ...
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            pipeline.Setup( renderer );

            var doc = Markdown.Parse( ( target as TextAsset ).text, pipeline );
            renderer.Render( doc );
        }


        //------------------------------------------------------------------------------

        Vector2 mScrollPos;
        Layout  mLayout       = null;
        bool    mRaw          = false;
        float   mHeaderHeight = 0.0f;

        void DrawIMGUI()
        {
            GUI.skin    = Skin;
            GUI.enabled = true;
            var padding = 8.0f;


            // clear background

            GUI.DrawTexture( new Rect( 0.0f, mHeaderHeight, Screen.width, Screen.height ), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, false );

            // TODO: recalc scroll pos when swapping between modes .. ?


            // draw buttons
            {
                var style      = GUI.skin.button;
                var size       = style.fixedHeight;
                var rectButton = new Rect( Screen.width - padding - size - Skin.verticalScrollbar.fixedWidth, mHeaderHeight + padding, size, size );

                if( GUI.Button( rectButton, mRaw ? IconRaw : IconMarkdown, Skin.button ) )
                {
                    mRaw = !mRaw;
                }

                // TODO: add a back button (and browsing history?)
            }


            // draw content

            if( mRaw )
            {
                var style   = Skin.GetStyle( "pre" );
                var content = new GUIContent( ( target as TextAsset ).text );
                var height  = style.CalcHeight( content, Screen.width );

                mScrollPos = GUILayout.BeginScrollView( mScrollPos );
                EditorGUILayout.SelectableLabel( content.text, style, GUILayout.Height( height ) );
                GUILayout.EndScrollView();
            }
            else
            {
                //GetControlRect
                var rectContainer = new Rect( 0.0f, mHeaderHeight, Screen.width, Screen.height - mHeaderHeight * 3.0f );

                var width         = rectContainer.width - padding * 2.0f;
                var hasScrollBar  = mLayout.Height > rectContainer.height;
                var widthAdjust   = hasScrollBar ? 0.0f : Skin.verticalScrollbar.fixedWidth;
                var rectScroll    = new Rect( -padding, -padding, width - widthAdjust, mLayout.Height );

                mScrollPos = GUI.BeginScrollView( rectContainer, mScrollPos, rectScroll );
                
                
                //Debug.Log( Screen.height + "x" + mLayout.Height );
                //EditorGUIUtility.currentViewWidth

                switch( Event.current.type )
                {
                    case EventType.Ignore:
                        break;

                    case EventType.ContextClick:
                        // TODO: EventType.ContextClick

                        var menu = new GenericMenu();
                        menu.AddItem( new GUIContent( "View Source" ), false, () => mRaw = !mRaw );
                        menu.ShowAsContext();

                        break;

                    case EventType.Layout:
                        mLayout.Arrange( rectScroll.width );
                        break;

                    default:
                        mLayout.Draw();
                        break;
                }

                GUI.EndScrollView();
            }
        }
    }
}
