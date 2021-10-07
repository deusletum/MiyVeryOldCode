using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace MS.CommandLine
{
	/// <summary>
	///    Utility for formatting help text.
	/// </summary>
	internal sealed class FormatUtility
	{
        private FormatUtility()
        {
        }

        /// <summary>
        ///    This is a utility to aid in reformatting text so that it fits on a
        ///    console screen of a given width.  It tries to break the line at the
        ///    most appropriate place.
        /// </summary>
        /// <param name="toFormat">
        ///    This is the text to format.
        /// </param>
        /// <param name="offset">
        ///    Offset from the left all lines should start at.
        /// </param>
        /// <param name="hangingIndent">
        ///    Hanging indent.  Subsequent lines will start this many
        ///    characters to the right of the first line.
        /// </param>
        /// <param name="width">
        ///    The width of the screen.
        /// </param>
        /// <returns>
        ///    Formatted string.
        /// </returns>
        public static string FormatStringForWidth(string toFormat, int offset, int hangingIndent, int width)
        {
            string[] words = MakeWords(toFormat);
            StringBuilder outputString = new StringBuilder();

            string hangingIndentString = new string(' ', offset + hangingIndent);

            int lineCount = 0;
            int startWord = 0;
            int endWord = 0;
            while (startWord < words.Length)
            {
                // first calculate how many words to write on this line.
                //
                endWord = startWord;
                int lineLength = offset;
                if (lineCount > 0)
                {
                    lineLength += hangingIndent;
                }

                lineLength += words[startWord].Length;
                if(words[startWord].EndsWith("."))
                { 
                    lineLength += 2; // two spaces after a period.
                }
                else
                {
                    lineLength++;
                }

                while (endWord+1 < words.Length && 
                       (lineLength + words[endWord+1].Length) < width )
                {
                    // the next word can be included on the line.
                    //
                    endWord++;
                    lineLength += words[endWord].Length;
                    if(words[endWord].EndsWith("."))
                    { 
                        lineLength += 2; // two spaces after a period.
                    }
                    else
                    {
                        lineLength++;
                    }
                }
                //
                // Now we know the words that should be on the line.
                //
                if(lineCount > 0)
                {
                    outputString.Append(hangingIndentString);
                }
                else
                {
                    outputString.Append(new string(' ', offset));
                }

                for (int i=startWord ; i <= endWord ; i++)
                {
                    if (i > startWord)
                    {
                        if(words[i-1].EndsWith("."))
                        { 
                            outputString.Append("  ") ; // two spaces after a period.
                        }
                        else
                        {
                            outputString.Append(" ");
                        }
                    }
                    outputString.Append(words[i]);
                }
                outputString.Append(Environment.NewLine);
                lineCount++;

                startWord = endWord+1;
            }

            Debug.Assert(startWord == words.Length, "startWord == words.Length+1");
            Debug.Assert(words.Length == 0 || endWord == words.Length-1, "words.Length == 0 || endWord == words.Length-1");
            return outputString.ToString();
        }

        /// <summary>
        ///    Divides a string up into words.
        /// </summary>
        /// <param name="toParse">
        ///    The string to break into words.
        /// </param>
        /// <returns>
        ///    An array of all of the words in the string.
        /// </returns>
        public static string[] MakeWords(string toParse)
        {
            char[] chars = toParse.ToCharArray();
            StringBuilder word = new StringBuilder();
            ArrayList wordList = new ArrayList();
            for(int i=0 ; i < chars.Length ; i++)
            {
                if(Char.IsWhiteSpace(chars[i]))
                {
                    if(word.Length > 0)
                    {
                        // Word boundary.  Add the word to the word list.
                        //
                        wordList.Add(word.ToString());
                        word.Length = 0;
                    }
                }
                else
                {
                    word.Append(chars[i]);
                }
            }
            //
            // don't forget about the last word!
            //
            if (word.Length > 0)
            {
                wordList.Add(word.ToString());
            }

            return (string[])wordList.ToArray(typeof(string));
        }

        private enum ParseState { Start, StartOfLine, ReadNext, EndOfLine, EndOfString };
	}
}
