using System;

namespace MementoDesignPattern
{
    public class TextEditorOriginator
    {
        private string Text { get; set; }

        public void AddText(string text)
        {
            Text += text;
        }

        public void ShowOriginatorStatus()
        {
            Console.WriteLine("Current state of the Originator: " + Text);
        }

        public TextEditorMemento CreateMemento()
        {
            return new TextEditorMemento(Text);
        }

        public void SetMemento(TextEditorMemento memento)
        {
            if (memento != null)
            {
                Text = memento.GetText();
            }
        }
    }
}
