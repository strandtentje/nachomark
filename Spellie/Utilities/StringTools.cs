using System;
using System.Collections.Generic;
using System.Text;

namespace NachoMark
{
    public static class StringTools
    {

        public static string deprecated__cutAndReturn(ref string consumption, int position)
        {
            string food = consumption.Remove(position);
            consumption = consumption.Remove(0, position);
            return food;
        }

        public static string deprecated__consumeUpTo(ref string consumption, char upto)
        {
            return deprecated__cutAndReturn(ref consumption, consumption.IndexOf(upto));
        }

        public static string deprecated__consumeNest(ref string consumption, char opening, char closing)
        {
            int depth = 0;
            int i = 0;
            do
            {
                if (consumption[i] == opening) depth++;
                if (consumption[i] == closing) depth--;
                i++;
            } while (depth > 0);

            return deprecated__cutAndReturn(ref consumption, i);
        }

        public static string takeIdentifier(string data, ref int strPos)
        {
            StringBuilder srbIdentifier = new StringBuilder();

            char c = data[strPos];

            while((c >= 'a') && (c <= 'z'))
            {
                srbIdentifier.Append(c); 
                strPos++;
                c = data[strPos];
            }

            return srbIdentifier.ToString();
        }

        public static string takeText(string data, ref int strPos)
        {
            StringBuilder srbIdentifier = new StringBuilder();

            char c = data[strPos];

            while ((c >= ' ') && (c <= '~') && (c != '"'))
            {
                srbIdentifier.Append(c);
                strPos++;
                c = data[strPos];
            }

            return srbIdentifier.ToString();
        }

        public static string takeNumbers(string data, ref int strPos)
        {
            StringBuilder srbNumber = new StringBuilder(); //

            char c = data[strPos];

            while ((c >= '0') && (c <= '9'))
            {
                srbNumber.Append(c);
                strPos++;
                c = data[strPos];
            }

            return srbNumber.ToString();
        }

        public static bool takeCharacter(string data, ref int strPos, char pChar)
        {
            if (data[strPos] == pChar)
            {
                strPos++;
                return true;
            }

            return false;
        }

        public static void blockifyTo(string data, string preword, int linelenght, int leftSpace, StringBuilder target)
        {
            string[] words = data.Split(' ');
            
            int cLineLength = 0;

            if (preword.Length > 0)
            {
                cLineLength += (leftSpace > (preword.Length + 1) ? leftSpace : preword.Length + 1);
                target.Append(preword + ':');
                target.Append(new string(' ', cLineLength - preword.Length - 1));
            }
                
            foreach (string word in words)
            {
                cLineLength += word.Length + 1;

                if (cLineLength > linelenght)
                {
                    cLineLength = leftSpace;
                    target.AppendLine();
                    target.Append(new string(' ', leftSpace));
                }

                target.Append(word + ' ');
            }

            target.AppendLine();
        }

        public static void captifyTo(string capture, string data, StringBuilder target)
        {
            target.AppendLine(capture.Replace("[t]", data));
        }

    }
}
