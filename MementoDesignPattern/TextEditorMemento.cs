using System;

namespace MementoDesignPattern
{
    [Serializable()]
    public class TextEditorMemento
    {
        private string Text { get; set; }
        public TextEditorMemento(string text)
        {
            Text = text;
        }

        public string GetText()
        {
            return Text;
        }
    }
}
