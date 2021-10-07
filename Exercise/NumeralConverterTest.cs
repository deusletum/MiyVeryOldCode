using System;
using MbUnit.Framework;
using roman_numeral.roman;

// Because I converted the methods to .NET 4.0, I was forced to convert the Unit test Framewark to MbUnit
// The great thing is that MbUnit is compatible with Nunit, so I only add a using statement for MbUnit.Framework
// You can download MbUnit from http://gallio.org/Downloads.aspx, I am using the most current build of 3.3.1 x64
// MbUnit is part of the Gallio automation plateform and intigrates with Visual Studio
// I have added tests for both of the new methods
namespace roman_numeral.roman_test
{
    [TestFixture]
    public class NewNumeralConverterTest
    {
        [Test]
        public void shouldBeCreatable()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.IsNotNull(NewNumeralConverter);
        }

        [Test]
        public void shouldBeAbleToConvertTo1()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(1, NewNumeralConverter.ConvertToDecimal("I"));
            Assert.AreEqual(1, NewNumeralConverter.ConvertToDecimalLinq("I"));
        }

        [Test]
        public void shouldBeAbleToConvertTo2()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(2, NewNumeralConverter.ConvertToDecimal("II"));
            Assert.AreEqual(2, NewNumeralConverter.ConvertToDecimalLinq("II"));
        }

        [Test]
        public void shouldBeAbleToConvertTo3()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(3, NewNumeralConverter.ConvertToDecimal("III"));
            Assert.AreEqual(3, NewNumeralConverter.ConvertToDecimalLinq("III"));
        }

        [Test]
        public void shouldBeAbleToConvertTo4()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(4, NewNumeralConverter.ConvertToDecimal("IV"));
            Assert.AreEqual(4, NewNumeralConverter.ConvertToDecimalLinq("IV"));
        }

        [Test]
        public void shouldBeAbleToConvertTo5()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(5, NewNumeralConverter.ConvertToDecimal("V"));
            Assert.AreEqual(5, NewNumeralConverter.ConvertToDecimalLinq("V"));
        }

        [Test]
        public void shouldBeAbleToConvertTo6()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(6, NewNumeralConverter.ConvertToDecimal("VI"));
            Assert.AreEqual(6, NewNumeralConverter.ConvertToDecimalLinq("VI"));
        }

        [Test]
        public void shouldBeAbleToConvertTo7()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(7, NewNumeralConverter.ConvertToDecimal("VII"));
            Assert.AreEqual(7, NewNumeralConverter.ConvertToDecimalLinq("VII"));
        }

        [Test]
        public void shouldBeAbleToConvertTo8()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(8, NewNumeralConverter.ConvertToDecimal("VIII"));
            Assert.AreEqual(8, NewNumeralConverter.ConvertToDecimalLinq("VIII"));
        }

        [Test]
        public void shouldBeAbleToConvertTo10()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(10, NewNumeralConverter.ConvertToDecimal("X"));
            Assert.AreEqual(10, NewNumeralConverter.ConvertToDecimalLinq("X"));

        }

        [Test]
        public void shouldBeAbleToConvertTo19()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(19, NewNumeralConverter.ConvertToDecimal("XVIIII"));
            Assert.AreEqual(19, NewNumeralConverter.ConvertToDecimal("XIX"));
            Assert.AreEqual(19, NewNumeralConverter.ConvertToDecimalLinq("XVIIII"));
            Assert.AreEqual(19, NewNumeralConverter.ConvertToDecimalLinq("XIX"));
        }

        [Test]
        public void shouldBeAbleToConvertTo20()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(20, NewNumeralConverter.ConvertToDecimal("XX"));
            Assert.AreEqual(20, NewNumeralConverter.ConvertToDecimalLinq("XX"));
        }
        
        [Test]
        public void shouldBeAbleToConvertTo25()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(25, NewNumeralConverter.ConvertToDecimal("XXV"));
            Assert.AreEqual(25, NewNumeralConverter.ConvertToDecimalLinq("XXV"));
        }

        [Test]
        public void shouldBeAbleToConvertTo30()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(30, NewNumeralConverter.ConvertToDecimal("XXX"));
            Assert.AreEqual(30, NewNumeralConverter.ConvertToDecimalLinq("XXX"));
        }

        [Test]
        public void shouldBeAbleToConvertTo44()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimal("XXXXIV"));
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimal("XXXXIIII"));
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimal("XLIIII"));
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimal("XLIV"));
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimalLinq("XXXXIV"));
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimalLinq("XXXXIIII"));
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimalLinq("XLIIII"));
            Assert.AreEqual(44, NewNumeralConverter.ConvertToDecimalLinq("XLIV"));
        }

        [Test]
        public void shouldBeAbleToConvertTo50()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            //Fixed this test
            Assert.AreEqual(50, NewNumeralConverter.ConvertToDecimal("L"));
            Assert.AreEqual(50, NewNumeralConverter.ConvertToDecimal("XXXXX"));
            Assert.AreEqual(50, NewNumeralConverter.ConvertToDecimalLinq("L"));
            Assert.AreEqual(50, NewNumeralConverter.ConvertToDecimalLinq("XXXXX"));
        }
        [Test]
        public void shouldBeAbleToConvertTo100()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(100, NewNumeralConverter.ConvertToDecimal("C"));
            Assert.AreEqual(100, NewNumeralConverter.ConvertToDecimal("LL"));
            Assert.AreEqual(100, NewNumeralConverter.ConvertToDecimalLinq("C"));
            Assert.AreEqual(100, NewNumeralConverter.ConvertToDecimalLinq("LL"));
        }
        [Test]
        public void shouldBeAbleToConvertTo500()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(500, NewNumeralConverter.ConvertToDecimal("D"));
            Assert.AreEqual(500, NewNumeralConverter.ConvertToDecimal("CCCCC"));
            Assert.AreEqual(500, NewNumeralConverter.ConvertToDecimalLinq("D"));
            Assert.AreEqual(500, NewNumeralConverter.ConvertToDecimalLinq("CCCCC"));
        }
        [Test]
        public void shouldBeAbleToConvertTo1000()
        {
            NewNumeralConverter NewNumeralConverter = new NewNumeralConverter();
            Assert.AreEqual(1000, NewNumeralConverter.ConvertToDecimal("M"));
            Assert.AreEqual(1000, NewNumeralConverter.ConvertToDecimal("DD"));
            Assert.AreEqual(1000, NewNumeralConverter.ConvertToDecimalLinq("M"));
            Assert.AreEqual(1000, NewNumeralConverter.ConvertToDecimalLinq("DD"));
        }
    }
}
