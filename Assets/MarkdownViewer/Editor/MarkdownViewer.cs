////////////////////////////////////////////////////////////////////////////////

using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public Texture      TexturePlaceholder;


        //------------------------------------------------------------------------------

        // proper path combine

        char[] mSeparators = new char[] { '/', '\\' };

        private string PathCombine( string a, string b )
        {
            var combined = 
                a.Split( mSeparators, StringSplitOptions.RemoveEmptyEntries )
                .ToList()
                .Concat( b.Split( mSeparators, StringSplitOptions.RemoveEmptyEntries ) );

            var path = new List<string>();

            foreach( var str in combined )
            {
                if( str == "." )
                {
                    continue;
                }
                else if( str == ".." )
                {
                    if( path.Count > 0 )
                    {
                        path.RemoveAt( path.Count - 1 );
                    }
                }
                else
                {
                    path.Add( str );
                }
            }

            return String.Join( "/", path.ToArray() );
        }

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

                assetPath = PathCombine( targetDir, url );
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

        public Texture FetchImage( string url )
        {
            Texture tex;

            if( mTextureCache.TryGetValue( url, out tex ) )
            {
                return tex;
            }

            mActiveRequests.Add( new ImageRequest( url ) );
            mTextureCache[ url ] = TexturePlaceholder;

            return TexturePlaceholder;
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

            if( req.Request.isNetworkError )
            {
                Debug.LogError( $"Error fetching '{req.URL}' - {req.Request.error}" );
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

        private Editor mDefaultEditor;

        private void DrawDefaultEditor()
        {
            if( mDefaultEditor == null )
            {
                mDefaultEditor = CreateEditor( target, Type.GetType( "UnityEditor.TextAssetInspector, UnityEditor" ) );
            }

            if( mDefaultEditor != null )
            {
                GUI.skin = null;
                mDefaultEditor.OnInspectorGUI();
            }
        }

        //------------------------------------------------------------------------------

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


        //------------------------------------------------------------------------------

        bool    mRaw = false;
        float   mHeaderHeight = 0.0f;

        public override void OnInspectorGUI()
        {
            // has target?

            var asset = target as TextAsset;

            if( asset == null )
            {
                DrawDefaultEditor();
                return;
            }

            // TODO: configurable extensions?
            // file extension is .md

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) == false )
            {
                DrawDefaultEditor();
                return;
            }

            ParseDocument();
            DrawIMGUI();
        }


        //------------------------------------------------------------------------------

        Layout mDoc = null;

        void ParseDocument()
        {
            if( mDoc != null )
            {
                return;
            }

            mDoc = new Layout( new StyleCache( Skin.label, StyleConfig ), this );

            var renderer = new RendererMarkdown( mDoc );

            // TODO: look at pipeline options ...
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            pipeline.Setup( renderer );

            var doc = Markdown.Parse( ( target as TextAsset ).text, pipeline );
            renderer.Render( doc );
        }


        //------------------------------------------------------------------------------

        Vector2 mScrollPos;

        void DrawIMGUI()
        {
            GUI.skin    = Skin;
            GUI.enabled = true;

            var padding    = 8.0f;
            var buttonSize = 20.0f;

            // clear background

            GUI.DrawTexture( new Rect( 0.0f, mHeaderHeight, Screen.width, Screen.height ), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, false );

            // TODO: recalc scroll pos when swapping between modes ...

            // draw buttons

            mRaw = GUI.Toggle( new Rect( Screen.width - padding - buttonSize, mHeaderHeight + padding, buttonSize, buttonSize ), mRaw, "" );


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
                var contentRect = new Rect( padding, mHeaderHeight + padding, Screen.width - padding * 2.0f, Screen.height - mHeaderHeight - padding * 2.0f );
                
                GUI.BeginGroup( contentRect ); // clipping ...
                                               //GUI.BeginScrollView( Rect, position, Rect ); // TODO: scroll view


                if( Event.current.type == EventType.Layout )
                {
                    mDoc.Arrange( contentRect.width );
                }
                else
                {
                    mDoc.Draw();
                }

                GUI.EndGroup();
            }
        }
    }
}
