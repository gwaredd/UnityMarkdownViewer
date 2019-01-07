////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using MG.MDV;
using NUnit.Framework;

namespace Tests
{
    public class TestProjectGeneration
    {
        [Test]
        public void ProjectGeneration()
        {
            try
            {
//                 var project = File.ReadAllText( "Assembly-CSharp-Editor.csproj" );
// 
//                 Assert.IsNotEmpty( project );
// 
//                 var newProject = MarkdownVisualStudio.ModifyProjectFile( "Test", project );
// 
//                 File.WriteAllText( "test.csproj", newProject );
            }
            catch( Exception e )
            {
                Assert.Fail( e.Message );
            }
        }
    }
}
