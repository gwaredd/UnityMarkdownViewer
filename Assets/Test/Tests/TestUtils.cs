////////////////////////////////////////////////////////////////////////////////

using NUnit.Framework;

namespace MG.MDV
{
    public class TestUtils
    {
        [Test]
        public void TestUtilsPathCombine()
        {
            Assert.AreEqual( "a/b", Utils.PathCombine( "a", "b" ) );
            Assert.AreEqual( "a/b", Utils.PathCombine( "", "a/b" ) );
            Assert.AreEqual( "a/b", Utils.PathCombine( "a/b", "" ) );
            Assert.AreEqual( "a/b", Utils.PathCombine( "a/b", null ) );
            Assert.AreEqual( "a/b", Utils.PathCombine( null, "a/b" ) );
            Assert.AreEqual( "", Utils.PathCombine( null, null ) );

            Assert.AreEqual( "a/b/c/d/e/f/g", Utils.PathCombine( "a\\b/c\\d", "e\\f/g" ) );

            Assert.AreEqual( "a/b/c", Utils.PathCombine( "a//b", "c" ) );

            Assert.AreEqual( "a/b/c", Utils.PathCombine( "a/./b", "c" ) );
            Assert.AreEqual( "a/b/c", Utils.PathCombine( "a/.", "b/c" ) );
            Assert.AreEqual( "a/b/c", Utils.PathCombine( "./a/./././b", "c" ) );
            Assert.AreEqual( "a/b/c", Utils.PathCombine( "a/b", "./c/./." ) );

            Assert.AreEqual( "a/b/c", Utils.PathCombine( "a/b/d/../c", null ) );
            Assert.AreEqual( "a/b/c", Utils.PathCombine( "a/b/d/e/../../c", null ) );
            Assert.AreEqual( "a/b/c", Utils.PathCombine( "../a/b/c", null ) );
            Assert.AreEqual( "a/b/c", Utils.PathCombine( "../a/b/c/d/..", null ) );
            Assert.AreEqual( "a/b/c", Utils.PathCombine( "../a/d/../b/f/../c", null ) );
        }
    }
}

