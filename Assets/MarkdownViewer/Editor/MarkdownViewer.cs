////////////////////////////////////////////////////////////////////////////////

using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Markdig.Extensions.JiraLinks;

namespace MG.MDV
{
    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownViewer : Editor, IActions
    {
        public GUISkin      Skin;
        public Texture      Placeholder;
        public Texture      IconMarkdown;
        public Texture      IconRaw;
        public Texture      IconBack;
        public Texture      IconForward;

        //------------------------------------------------------------------------------

        private static History mHistory = new History();

        public void SelectPage( string url )
        {
            if( string.IsNullOrEmpty( url ) )
            {
                return;
            }

            // internal link

            if( url.StartsWith( "#" ) )
            {
                var block = mLayout.Find( url.ToLower() );

                if( block != null )
                {
                    mScrollPos.y = block.Rect.y;
                }
                else
                {
                    Debug.LogWarning( $"Unable to find section header {url}" );
                }

                return;
            }

            // relative or absolute link ...

            var newPath = string.Empty;

            if( url.StartsWith( "/" ) )
            {
                newPath = url.Substring( 1 );
            }
            else
            {
                var currentPath = AssetDatabase.GetAssetPath( target );
                newPath = Utils.PathCombine( Path.GetDirectoryName( currentPath ), url );
            }

            // load file

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>( newPath );

            if( asset != null )
            {
                mHistory.Add( newPath );
                Selection.activeObject = asset;
            }
            else
            {
                Debug.LogError( $"Could not find asset {newPath}" );
            }
        }

        //------------------------------------------------------------------------------

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

        private Editor mDefaultEditor;

        public override void OnInspectorGUI()
        {
            // file extension is .md

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) )
            {
                ParseDocument( path );
                DrawIMGUI();
            }
            else
            {
                mDefaultEditor = mDefaultEditor ?? CreateEditor( target, Type.GetType( "UnityEditor.TextAssetInspector, UnityEditor" ) );
                mDefaultEditor?.OnInspectorGUI();
            }
        }


#if MDV_DEV
        //------------------------------------------------------------------------------

        void Dump()
        {
            var renderer = new DebugRenderer();

            var builder = new MarkdownPipelineBuilder()
                .UseAutoLinks()
                .DisableHtml()
            ;

            if( !string.IsNullOrEmpty( Preferences.JIRA ) )
            {
                builder.UseJiraLinks( new JiraLinkOptions( Preferences.JIRA ) );
            }

            var pipeline = builder.Build();
            pipeline.Setup( renderer );

            var doc = Markdown.Parse( ( target as TextAsset ).text, pipeline );
            renderer.Render( doc );
        }

#endif

        void ParseDocument( string filename )
        {
            if( mLayout != null )
            {
                return;
            }

            mHistory.OnOpen( filename );

            var context  = new Context( Skin, this );
            var builder  = new LayoutBuilder( context );
            var renderer = new RendererMarkdown( builder );

            var pipelineBuilder = new MarkdownPipelineBuilder()
                .UseAutoLinks()
                // TODO: .UsePipeTables()
                // TODO: .UseGridTables()
                // TODO: .UseEmphasisExtras()
                // TODO: .UseEmojiAndSmiley()
                // TODO: .UseListExtras()
                // TODO: .UseTaskLists()
                .DisableHtml()
            ;

            if( !string.IsNullOrEmpty( Preferences.JIRA ) )
            {
                pipelineBuilder.UseJiraLinks( new JiraLinkOptions( Preferences.JIRA ) );
            }

            var pipeline = pipelineBuilder.Build();
            pipeline.Setup( renderer );

            var doc = Markdown.Parse( ( target as TextAsset ).text, pipeline );
            renderer.Render( doc );

            mLayout = builder.GetLayout();
        }


        //------------------------------------------------------------------------------

        Vector2     mScrollPos;
        Layout      mLayout = null;
        bool        mRaw    = false;
        TextAsset   mText   = null;

        void DrawIMGUI()
        {
            GUI.skin = Skin;
            GUI.enabled = true;

            mText = target as TextAsset;

            // content rect

            GUILayout.FlexibleSpace();
            var rectContainer = GUILayoutUtility.GetLastRect();
            rectContainer.width = Screen.width;


            // clear background

            var rectFullScreen = new Rect( 0.0f, rectContainer.yMin - 4.0f, Screen.width, Screen.height );
            GUI.DrawTexture( rectFullScreen, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, false );


            // scroll window

            var padLeft     = 8.0f;
            var padRight    = 4.0f;
            var padHoriz    = padLeft + padRight;
            var scrollWidth = Skin.verticalScrollbar.fixedWidth;
            var minWidth    = rectContainer.width - scrollWidth - padHoriz;
            var maxHeight   = ContentHeight( minWidth );

            var hasScrollbar =  maxHeight >= rectContainer.height;
            var contentWidth = hasScrollbar ? minWidth : rectContainer.width - padHoriz;
            var rectContent  = new Rect( -padLeft, 0.0f, contentWidth, maxHeight );

            // draw content

            using( var scroll = new GUI.ScrollViewScope( rectContainer, mScrollPos, rectContent ) )
            {
                mScrollPos = scroll.scrollPosition;

                if( mRaw )
                {
                    rectContent.width = minWidth - GUI.skin.button.fixedWidth;
                    DrawRaw( rectContent );
                }
                else
                {
                    DrawMarkdown( rectContent );
                }
            }

            DrawToolbar( rectContainer, hasScrollbar ? scrollWidth + padRight : padRight );

        }

        //------------------------------------------------------------------------------

        float ContentHeight( float width )
        {
            return mRaw ? Skin.GetStyle( "raw" ).CalcHeight( new GUIContent( mText.text ), width ) : mLayout.Height;
        }

        //------------------------------------------------------------------------------

        void DrawToolbar( Rect rect, float marginRight )
        {
            var style  = GUI.skin.button;
            var size   = style.fixedHeight;
            var btn    = new Rect( rect.xMax - size - marginRight, rect.yMin, size, size );

            if( GUI.Button( btn, mRaw ? IconRaw : IconMarkdown, Skin.button ) )
            {
                mRaw = !mRaw;
            }

            if( mRaw == false )
            {
                if( mHistory.CanForward )
                {
                    btn.x -= size;

                    if( GUI.Button( btn, IconForward, Skin.button ) )
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>( mHistory.Forward() );
                    }
                }

                if( mHistory.CanBack )
                {
                    btn.x -= size;

                    if( GUI.Button( btn, IconBack, Skin.button ) )
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>( mHistory.Back() );
                    }
                }
            }
        }

        void DrawRaw( Rect rect )
        {
            //EditorGUI.TextArea( rect, mText.text, Skin.GetStyle( "raw" ) );
            EditorGUI.SelectableLabel( rect, mText.text, Skin.GetStyle( "raw" ) );
        }

        void DrawMarkdown( Rect rect )
        {
            switch( Event.current.type )
            {
                case EventType.Ignore:
                    break;

                case EventType.ContextClick:
                    var menu = new GenericMenu();
                    menu.AddItem( new GUIContent( "View Source" ), false, () => mRaw = !mRaw );
                    menu.ShowAsContext();

                    break;

                case EventType.Layout:
                    mLayout.Arrange( rect.width );
                    break;

                default:
                    mLayout.Draw();
                    break;
            }
        }
    }
}
