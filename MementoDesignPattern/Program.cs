using System;

namespace MementoDesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Caretaker caretaker = new Caretaker();
            TextEditorOriginator textEditor = new TextEditorOriginator();

            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.AddText("Hello");
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.AddText( " world!");
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.AddText(" This is an example for:");
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.AddText(" a design pattern.");
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Redo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            textEditor.AddText(" The Memento design pattern.");
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Redo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            Console.ReadLine();
        }
    }
}
