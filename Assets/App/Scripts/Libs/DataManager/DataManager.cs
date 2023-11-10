using System;

namespace App.Scripts.Libs.DataManager
{
    public class DataChangedEventArgs : EventArgs
    {
        public string TextID { get; }
        public object NewData { get; }

        public DataChangedEventArgs(string textID, object newData)
        {
            TextID = textID;
            NewData = newData;
        }
    }

    public class DataManager
    {
        public event EventHandler<DataChangedEventArgs> DataChanged;

        public void ModifyData<T>(string textID, T newData)
        {
            OnDataChanged(textID, newData);
        }

        protected virtual void OnDataChanged<T>(string textID, T newData)
        {
            DataChanged?.Invoke(this, new DataChangedEventArgs(textID, newData));
        }
    }
}