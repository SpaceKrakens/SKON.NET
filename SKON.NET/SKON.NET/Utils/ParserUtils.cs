#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserUtils.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SKON.Internal.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ParserUtils
    {
        private static UnicodeEncoding encoding = new UnicodeEncoding(true, false);

        public static string ConvertToUnicode(string input)
        {
            int length = input.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            }
            
            return encoding.GetString(bytes);
        }
        
        public static string EscapeString(string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return txt;
            }
            StringBuilder retval = new StringBuilder(txt.Length);
            for (int anchor = 0; anchor <= txt.Length;)
            {
                int escapeIndex = txt.IndexOf('\\', anchor);

                if (escapeIndex < 0 || escapeIndex >= txt.Length - 1)
                    escapeIndex = txt.Length;

                retval.Append(txt, anchor, escapeIndex - anchor);

                if (escapeIndex >= txt.Length)
                    break;

                switch (txt[++escapeIndex])
                {
                    case 'b': retval.Append('\b'); break;
                    case 'n': retval.Append('\n'); break;
                    case 'f': retval.Append('\f'); break;
                    case 'r': retval.Append('\r'); break;
                    case 't': retval.Append('\t'); break;
                    case '"': retval.Append('"'); break;
                    case '\\': retval.Append('\\'); break;
                    case 'u':
                        if (escapeIndex + 4 >= txt.Length)
                            goto default;

                        retval.Append(ConvertToUnicode(txt.Substring(escapeIndex + 1, 4)));
                        escapeIndex += 4;
                        break;
                    default:
                        throw new FormatException("Invalid character escape!");
                }
                anchor = escapeIndex + 1;
            }
            return retval.ToString();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
