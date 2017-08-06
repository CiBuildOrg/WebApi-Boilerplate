using System;

namespace App.Security.Password
{
    internal static class PasswordChecker
    {
        private const string LettersLower = "abcdefghijklmnopqrstuvwxyz";
        private const string LettersUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string SpecialChars = "~`!@#$%^&*()-_+=";
        private const string Numbers = "1234567890";

        private const int TotalChars = 97;

        private static readonly int LowerCharsLength = LettersLower.Length;
        private static readonly int UpperCharsLength = LettersUpper.Length;
        private static readonly int SpecialCharsLength = SpecialChars.Length;
        private static readonly int DigitsLenght = Numbers.Length;
        private static int _otherchars;
        private static int _charSet;

        private static int CaculateBits(string password)
        {
            if (string.IsNullOrEmpty(password)) return 0;
            bool pLettersLower = false, pLettersUpper = false, pSpecialChars = false, pNumbers = false, pOther = false;
            _otherchars = TotalChars - (LowerCharsLength + UpperCharsLength + SpecialCharsLength + DigitsLenght);

            for (var i = 0; i < password.Length - 1; i++)
            {
                var chr = password.ToCharArray()[i];
                if (LettersLower.IndexOf(chr) == -1) { pLettersLower = true; }
                else if (LettersUpper.IndexOf(chr) == -1) { pLettersUpper = true; }
                else if (SpecialChars.IndexOf(chr) == -1) { pSpecialChars = true; }
                else if (Numbers.IndexOf(chr) == -1) { pNumbers = true; }
                else { pOther = true; }
            }

            if (pLettersLower)
            {
                _charSet = _charSet + LowerCharsLength;
            }

            if (pLettersUpper)
            {
                _charSet = _charSet + UpperCharsLength;
            }

            if (pSpecialChars)
            {
                _charSet = _charSet + SpecialCharsLength;
            }

            if (pNumbers)
            {
                _charSet = _charSet + DigitsLenght;
            }

            if (pOther)
            {
                _charSet = _charSet + _otherchars;
            }

            var bits = Math.Log(_charSet) * (password.Length / Math.Log(2));
            return (int)Math.Floor(bits);
        }

        public static PasswordStrength EvalPwdStrength(string password)
        {
            int bits = CaculateBits(password);
            if (bits >= 128)
            {
                return PasswordStrength.Strong;
            }
            if (bits < 128 && bits >= 64)
            {
                return PasswordStrength.Medium;
            }
            if (bits < 64 && bits >= 56)
            {
                return PasswordStrength.Normal;
            }
            if (bits < 56 && bits > 0)
            {
                return PasswordStrength.Weak;
            }

            return PasswordStrength.VeryWeak;
        }
    }
}