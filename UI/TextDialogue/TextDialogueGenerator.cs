using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public static class TextDialogueGenerator
    {
        public static string[] GetLinesFromSingle(string text, IntPoint _textBufferSize)
        {
            var output = new string[_textBufferSize.Y];

            if (text.Length < _textBufferSize.Product)
            {
                text += new string(' ', _textBufferSize.Product - text.Length);
            }

            for (int i = 0; i < _textBufferSize.Product; i += _textBufferSize.X)
            {
                var section = text.Substring(i, _textBufferSize.X);
                output[i / _textBufferSize.X] = StringUtils.SetStringToSize(section,_textBufferSize.X);
            }

            return output;
        }

        public static string[] GetLines_KeepWordsIntact(IntPoint _textBufferSize,string sentence)
        {
            var words = sentence.Split(' ');
            var lines = new List<string>();

            string currentString = "";
            foreach (var word in words)
            {
                if (word.Length > _textBufferSize.X)
                {
                    throw new Exception("Can't keep word intact");
                }

                if (word.Length + currentString.Length > _textBufferSize.X)
                {
                    lines.Add(currentString + " ");
                    currentString = word + " ";
                }
                else
                {
                    currentString += word + " ";
                }
            }

            lines.Add(currentString);

            return GetLines(_textBufferSize,lines);
        }

        public static string[] GetLines(IntPoint _textBufferSize,params string[] lines) => GetLines(_textBufferSize,lines.ToList());
        public static string[] GetLines(IntPoint _textBufferSize,List<string> lines)
        {
            var output = new string[_textBufferSize.Y];

            if (lines.Count > _textBufferSize.Y)
            {
                lines = lines.GetRange(0, _textBufferSize.Y);
            }
            else if (lines.Count < _textBufferSize.Y)
            {
                lines.AddRange(StringUtils.GetEmptyStrings(_textBufferSize.Y - lines.Count));
            }

            for (int i = 0; i < _textBufferSize.Y; i++)
            {
                output[i] = StringUtils.SetStringToSize(lines[i],_textBufferSize.X);
            }

            return output;
        }

    }

}
