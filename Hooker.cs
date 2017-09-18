using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Clipboard = System.Windows.Clipboard;

namespace ClipboardRing
{
    public class StringWrap
    {
        public string Value { get; set; }

        public StringWrap()
        {
        }

        public StringWrap(string x)
        {
            Value = x;
        }

        public static implicit operator string(StringWrap x)
        {
            return x.Value ?? "";
        }

        public static implicit operator StringWrap(string x)
        {
            return new StringWrap(x);
        }
    }

    class Hooker : INotifyPropertyChanged
    {
        public bool IsCtrlDown = false;
        private int _index = 0;
        private ObservableCollection<StringWrap> _strings = new ObservableCollection<StringWrap>() {""};

        public int index
        {
            get { return _index; }
            set
            {
                if (value < 0)
                    return;
                _index = value < strings.Count ? value : strings.Count - 1;
                OnPropertyChanged();
                Clipboard.SetText(strings[_index]);
            }
        }
        public ObservableCollection<StringWrap> strings
        {
            get { return _strings; }
            set
            {
                _strings = value;
                index = value.Count - 1;
            }
        }


        private IKeyboardMouseEvents m_GlobalHook;

        public Hooker()
        {
            index = 0;
            Clipboard.SetText(strings[index]);
        }

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            //m_GlobalHook.KeyPress += GlobalHookKeyPress;
            m_GlobalHook.KeyDown += GlobalHookKeyDown;
            m_GlobalHook.KeyUp += GlobalHookKeyUp;
        }

        private void GlobalHookKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey)
                IsCtrlDown = false;
            if (e.KeyCode == Keys.V && IsCtrlDown)
            {
                index = (index + 1) % strings.Count;
            }
        }

        private void GlobalHookKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey)
                IsCtrlDown = true;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("KeyPress: \t{0}", e.KeyChar);
        }

        public void Unsubscribe()
        {
            m_GlobalHook.KeyDown -= GlobalHookKeyDown;
            m_GlobalHook.KeyUp -= GlobalHookKeyUp;
            //m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
