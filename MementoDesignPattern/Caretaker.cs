using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MementoDesignPattern
{
    [Serializable()]
    public class Caretaker
    {
        private List<TextEditorMemento> MementosForUndo { get; set; } = new List<TextEditorMemento>();
        private List<TextEditorMemento> MementosForRedo { get; set; } = new List<TextEditorMemento>();

        public void AddMemento(TextEditorMemento memento)
        {
            MementosForUndo.Add(memento);
            MementosForRedo = new List<TextEditorMemento>();
        }

        public TextEditorMemento Undo()
        {
            TextEditorMemento memento = null;

            if (MementosForUndo.Count > 1)
            {
                MementosForRedo.Add(MementosForUndo[MementosForUndo.Count - 1]);
                MementosForUndo.RemoveAt(MementosForUndo.Count - 1);

                memento = MementosForUndo[MementosForUndo.Count - 1];
            }

            return memento;
        }

        public TextEditorMemento Redo()
        {
            TextEditorMemento memento = null;

            if (MementosForRedo.Count > 0)
            {
                MementosForUndo.Add(MementosForRedo[MementosForRedo.Count - 1]);
                MementosForRedo.RemoveAt(MementosForRedo.Count - 1);

                memento = MementosForUndo[MementosForUndo.Count - 1];
            }

            return memento;
        }

        public void SaveToFile(string fileName)
        {
            Stream SaveFileStream = File.Create(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, this);
            SaveFileStream.Close();
        }

        public void LoadFromLile(string fileName)
        {
            Caretaker caretaker = null;

            if (File.Exists(fileName))
            {
                Stream openFileStream = File.OpenRead(fileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                caretaker = (Caretaker)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
            }

            if (caretaker != null)
            {
                MementosForRedo = caretaker.MementosForRedo;
                MementosForUndo = caretaker.MementosForUndo;
            }
        }
    }
}
