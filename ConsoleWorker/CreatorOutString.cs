using System.Text;

namespace ConsoleWorker
{
    public static class CreatorOutString
    {
        public static string GetLoadString(string beginStr, int num, int all)
        {
            int numAllChar = 70;
            num++;
            double valueDivision = (double)numAllChar / (double)all;
            int numLoadChar = (int)Math.Round(num * valueDivision);

            StringBuilder loadStr = new StringBuilder();
            loadStr.Append('\r');
            loadStr.Append(beginStr);
            loadStr.Append(": [");

            for (int i = 0; i < numLoadChar; i++)
            {
                loadStr.Append('=');
            }
            for (int i = numLoadChar; i < numAllChar; i++)
            {
                loadStr.Append(' ');
            }

            loadStr.Append(String.Format("] - {0}/{1}", num, all));

            return loadStr.ToString();
        }
    }
}
