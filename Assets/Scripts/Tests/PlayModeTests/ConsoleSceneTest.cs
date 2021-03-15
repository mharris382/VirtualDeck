using System.Collections;

namespace Tests.PlayModeTests
{
    public class ConsoleSceneTest
    {
        [NUnit.Framework.Test]
        public void ConsoleSceneTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityEngine.TestTools.UnityTest]
        public IEnumerator ConsoleSceneTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}