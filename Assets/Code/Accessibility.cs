using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Fizz6.Code
{
    public enum Accessibility
    {
        Public,
        Private,
        Protected,
        Internal,
        ProtectedInternal,
        PrivateProtected
    }

    public static class AccessibilityExt
    {
        private const string Pattern = @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))";
        private const string Replacement = " $0";
        
        public static string ToAccessibilityString(this Accessibility accessibility) => 
            Regex.Replace(accessibility.ToString(), Pattern, Replacement)
                .ToLowerInvariant();
    }
}
