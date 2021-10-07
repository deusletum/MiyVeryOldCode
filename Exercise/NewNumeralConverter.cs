using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

// Please note this code was complied in .NET 4.0 using Visual Studio 2010
// I have created a full implementation to the method that converts Roman Numerals to decimals (Added C,D,M)
// It is now one main method "ConvertToDecimal" and a supporting method "ConvertRomanNumToNumber"
// I feel is is much simplier to read and support
// I have also included a method "ConvertToDecimalLinq" that uses LINQ
// This new method does all the work in one method and only uses 15 lines of code
// It is however advanced code and not very easy to understand (Aggregate expression method can be confusing)

namespace roman_numeral.roman
{   
    /// <summary>
    /// Public class containing the new methods for converting Roman Numerals to decimal values
    /// </summary>
    public class NewNumeralConverter
    {
        /// <summary>
        /// Method to convert roman numerals to their corresponding decimal value
        /// </summary>
        /// <param name="RomanNum">String containing the roman numeral</param>
        /// <returns>int containing roman numeral decimal value</returns>
        public int ConvertRomanNumToNumber(string RomanNum)
        {
            int num = 0;

            switch (RomanNum)
            {
                case "I":
                    num = 1;
                    break;
                case "V":
                    num = 5;
                    break;
                case "X":
                    num = 10;
                    break;
                case "L":
                    num = 50;
                    break;
                case "C":
                    num = 100;
                    break;
                case "D":
                    num = 500;
                    break;
                case "M":
                    num = 1000;
                    break;
                default:
                    break;
            }
            return num;
        }

        /// <summary>
        /// Method to convert roman numerals to decimal value
        /// </summary>
        /// <param name="RomanNumerals">string of roman numerals</param>
        /// <returns>int with corresponding decimal value</returns>
        public int ConvertToDecimal(string RomanNumerals)
        {
            List<int> Rnums = new List<int>();
            //Reverse the string and load into a List<char>
            List<char> ReverseRomanNumerals = RomanNumerals.ToCharArray().Reverse().ToList<char>();

            //Get Decimal value for roman nums
            foreach (char c in ReverseRomanNumerals)
            {
                Rnums.Add(this.ConvertRomanNumToNumber(c.ToString()));
            }

            int total = 0, maxvalue = 0;

            // Keep a running total in total
            // keep current max value in maxvalue
            // Chech each char one by one
            // if number is greater than maxvalue, add it to the running total
            // if number is less than maxvalue, subtract it
            // if number is less than maxvalue and maxvale equal total, subtract number from maxvalue and assign it to total
            foreach (int n in Rnums)
            {
                maxvalue = Math.Max(maxvalue, n);
                if (n > maxvalue)
                {
                    total += maxvalue;
                }
                else if (n < maxvalue && total == maxvalue)
                {
                    total = (maxvalue - n);
                }
                else if (n < maxvalue)
                {
                    total -= n;
                }
                else
                {
                    total += maxvalue;
                }
            }

            return total;
        }

        /// <summary>
        /// Method to convert roman numerals to decimal value using LINQ
        /// </summary>
        /// <param name="RomanNumerals">string of roman numerals</param>
        /// <returns>int with corresponding decimal value</returns>
        public int ConvertToDecimalLinq(string RomanNumerals)
        {
            var result = RomanNumerals
                .ToCharArray()
                .Reverse()
                .Select(n => this.ConvertRomanNumToNumber(n.ToString()))
                .Aggregate(
                    new { CurMaxValue = 0, CurTotal = 0 },
                    (state, num) => new
                    {
                        CurMaxValue = Math.Max(state.CurMaxValue, num),
                        CurTotal = num >= state.CurMaxValue ? state.CurTotal + num
                                                        : state.CurTotal - num
                    },
                    aggregate => aggregate.CurTotal);

            return result;
        }
    }
}
