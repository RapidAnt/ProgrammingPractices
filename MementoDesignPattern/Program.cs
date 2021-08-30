using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MementoDesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            TextEditorOriginator textEditor = new TextEditorOriginator();
            Caretaker caretaker = new Caretaker();
            
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.AddText("Hello");
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.AddText( " world!");
            caretaker.AddMemento(textEditor.CreateMemento());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Undo());
            textEditor.ShowOriginatorStatus();

            textEditor.SetMemento(caretaker.Redo());
            textEditor.ShowOriginatorStatus();

            caretaker.SaveToFile("caretaker.dat");
            caretaker.LoadFromLile("caretaker.dat");

            Console.ReadLine();
        }
    }
}
