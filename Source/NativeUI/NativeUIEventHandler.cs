using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NativeUI {
    /// <summary>
    /// For internal use only.
    /// </summary>
    public class NativeUIEventHandler {

        public delegate void IClickEventHandler(UI.BaseElement sender);
        public delegate void MSelectedIndexChangedEventHandler(UIMenu sender, int newIndex);
        public delegate void ICheckedChangedEventHandler(UI.BaseElement sender, bool newValue);
        public delegate void ISelectedTextChangedEventHandler(UI.BaseElement sender, string newSelectedText);
        public delegate void ISelectedIndexChangedEventHandler(UI.BaseElement sender, int newIndex);

    }
}
