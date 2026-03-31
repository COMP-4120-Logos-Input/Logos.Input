using System.IO;
using NUnit.Framework.Interfaces;

namespace Logos.Input.Tests
{
    public abstract class MeasurableTestFixture
    {
        protected const string KeyboardCategory = "Keyboard";
        protected const string MouseCategory = "Mouse";

        private static int s_keyboardTestCount;
        private static int s_mouseTestCount;
        private static int s_keyboardSuccessCount;
        private static int s_mouseSuccessCount;

        [TearDown]
        public static void LogResults()
        {
            TestContext context = TestContext.CurrentContext;
            bool isKeyboardTest = false;
            bool isMouseTest = false;

            foreach (string category in context.Test.AllCategories())
            {
                switch (category)
                {
                    case KeyboardCategory:
                        s_keyboardTestCount++;
                        isKeyboardTest = true;
                        continue;
                    case MouseCategory:
                        s_mouseTestCount++;
                        isMouseTest = true;
                        continue;
                    default:
                        continue;
                }
            }

            if (context.Result.Outcome.Status == TestStatus.Passed)
            {
                if (isKeyboardTest)
                {
                    s_keyboardSuccessCount++;
                }

                if (isMouseTest)
                {
                    s_mouseSuccessCount++;
                }
            }
        }

        [OneTimeTearDown]
        public static void SaveResultsToFile()
        {
            string contents = $"Keyboard Success Rate,Mouse Success Rate\n" +
                              $"{(double)s_keyboardSuccessCount / s_keyboardTestCount}," +
                              $"{(double)s_mouseSuccessCount / s_mouseTestCount}";
            File.WriteAllText("Results.csv", contents);
        }
    }
}
