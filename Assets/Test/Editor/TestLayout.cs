////////////////////////////////////////////////////////////////////////////////

using NUnit.Framework;
//using Moq;

namespace MG.MDV
{
    public class TestLayout
    {
        [Test]
        public void TestStyle_Flags()
        {
            var style = new LayoutStyle();

            Assert.IsFalse( style.Bold );
            Assert.IsFalse( style.Italic );
            Assert.IsFalse( style.Fixed );

            style.Bold = true;

            Assert.IsTrue( style.Bold );
            Assert.IsFalse( style.Italic );
            Assert.IsFalse( style.Fixed );

            style.Bold = false;
            style.Italic = true;

            Assert.IsFalse( style.Bold );
            Assert.IsTrue( style.Italic );
            Assert.IsFalse( style.Fixed );

            style.Italic = false;
            style.Fixed = true;

            Assert.IsFalse( style.Bold );
            Assert.IsFalse( style.Italic );
            Assert.IsTrue( style.Fixed );
        }

        [Test]
        public void TestStyle_Size()
        {
            var style = new LayoutStyle();

            Assert.AreEqual( 0, style.Size );

            style.Bold = true;
            Assert.IsTrue( style.Bold );

            for( var i = 0; i <= 6; i++ )
            {
                style.Size = i;
                Assert.AreEqual( i, style.Size );
                Assert.IsTrue( style.Bold );
            }
        }


        ////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void Test_SegmentBuilder()
        {
//#warning TODO: come back to moq mocking library
            //             var mock = new Mock<IRenderContext>();
            // 
            //             char outChar;
            //             float outWidth;
            // 
            //             mock
            //                 .Setup( m => m.GetCharacter( It.IsAny<char>(), out outChar, out outWidth ) )
            //                 .Returns( (char ch ) => { return true; } );

            //bool GetCharacter( char inChar, out char outChar, out float outWidth );
            //    void Apply( LayoutStyle style );

            //RenderContext


            //var builder = new SegmentBuilder( mock.Object );
        }
    }
}

