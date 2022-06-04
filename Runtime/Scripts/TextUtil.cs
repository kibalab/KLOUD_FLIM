
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KLOUD
{
    public static class TextUtil
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        
        public static bool GetWordRectInText(this Text textUI, out Rect rect, string word)
        {
            rect = new Rect();
            if (string.IsNullOrEmpty(textUI.text) || string.IsNullOrEmpty(word) || !textUI.text.Contains(word))
            {
                return false;
            }

            Canvas.ForceUpdateCanvases();

            TextGenerator textGenerator = textUI.cachedTextGenerator;
            if (textGenerator.characterCount == 0)
            {
                textGenerator = textUI.cachedTextGeneratorForLayout;
            }

            if (textGenerator.characterCount == 0 || textGenerator.lineCount == 0)
            {
                return false;
            }
            
            List<UILineInfo> lines = textGenerator.lines as List<UILineInfo>;
            List<UICharInfo> characters = textGenerator.characters as List<UICharInfo>;

            int startIndex = textUI.text.IndexOf(word);
            UILineInfo lineInfo = new UILineInfo();
            for (int i = textGenerator.lineCount - 1; i >= 0; i--)
            {
                if (lines != null && lines[i].startCharIdx <= startIndex)
                {
                    lineInfo = lines[i];
                    break;
                }
            }

            if (lines != null && characters != null)
            {
                var anchoredPosition = textUI.rectTransform.anchoredPosition;

                var screenRatio = Screen.width / Screen.height; // 기준해상도

                rect.x = characters[startIndex].cursorPos.x * screenRatio;

                var temp = anchoredPosition.y / screenRatio;
                var yRatio = (float)Math.Round(temp, MidpointRounding.AwayFromZero); // 반올림 해줘야함
                rect.y = lineInfo.topY;

                for (var index = startIndex; index < startIndex + word.Length; index++)
                {
                    var info = characters[index];
                    rect.width += info.charWidth * screenRatio;
                }

                rect.height = lineInfo.height * screenRatio;
            }

            return true;
        }
    }
    
    // Get by https://nickname.tistory.com/28
}