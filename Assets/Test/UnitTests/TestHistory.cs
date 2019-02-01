////////////////////////////////////////////////////////////////////////////////


//using NUnit.Framework;

//namespace MG.MDV
//{
//    public class TestHistory
//    {
//        //------------------------------------------------------------------------------

//        [Test]
//        public void Navigation()
//        {
//            var history = new History();

//            Assert.IsTrue( history.IsEmpty );
//            Assert.IsNull( history.Current );
//            Assert.AreEqual( "", history.Join() );

//            history.Add( "a" );
//            history.Add( "b" );
//            history.Add( "c" );

//            Assert.AreEqual( 3, history.Count );
//            Assert.AreEqual( "c", history.Current );
//            Assert.AreEqual( "a;b;c", history.Join() );

//            Assert.AreEqual( "b", history.Back() );
//            Assert.AreEqual( "a", history.Back() );
//            Assert.AreEqual( "a", history.Back() );
//            Assert.AreEqual( "a", history.Current );

//            Assert.AreEqual( "b", history.Forward() );
//            Assert.AreEqual( "c", history.Forward() );
//            Assert.AreEqual( "c", history.Forward() );
//        }

//        //------------------------------------------------------------------------------

//        [Test]
//        public void AddToMiddle()
//        {
//            var history = new History();

//            history.Add( "a" );
//            history.Add( "b" );
//            history.Add( "c" );
//            history.Add( "d" );
//            history.Add( "e" );
//            history.Add( "f" );
//            Assert.AreEqual( "a;b;c;d;e;f", history.Join() );

//            history.Back();
//            history.Back();
//            history.Back();
//            Assert.AreEqual( "a;b;c", history.Join() );

//            history.Add( "z" );
//            Assert.AreEqual( "a;b;c;z", history.Join() );
//            Assert.AreEqual( 4, history.Count );
//        }

//        //------------------------------------------------------------------------------

//        [Test]
//        public void AddCurrent()
//        {
//            var history = new History();

//            history.Add( "a" );
//            history.Add( "b" );
//            history.Add( "c" );
//            Assert.AreEqual( "a;b;c", history.Join() );

//            history.Add( "c" );
//            Assert.AreEqual( "a;b;c", history.Join() );
//            Assert.AreEqual( 3, history.Count );

//            history.Back();
//            Assert.AreEqual( "a;b", history.Join() );
//            Assert.AreEqual( 3, history.Count );

//            history.Add( "b" );
//            Assert.AreEqual( "a;b", history.Join() );
//            Assert.AreEqual( 3, history.Count );
//        }

//        //------------------------------------------------------------------------------

//        [Test]
//        public void OnOpen()
//        {
//            var history = new History();

//            history.Add( "a" );
//            history.Add( "b" );
//            history.Add( "c" );

//            Assert.AreEqual( "a;b;c", history.Join() );
//            Assert.AreEqual( 3, history.Count );

//            history.OnOpen( "c" );
//            Assert.AreEqual( "a;b;c", history.Join() );
//            Assert.AreEqual( 3, history.Count );

//            history.OnOpen( "a" );
//            Assert.AreEqual( "a", history.Join() );
//            Assert.AreEqual( 1, history.Count );
//        }
//    }
//}

