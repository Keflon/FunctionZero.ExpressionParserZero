using System;
using System.ComponentModel;
using System.Reflection;

namespace FunctionZero.ExpressionParserZero.Binding
{
    /*
    TODO:
    Ensure no memory leaks when a nested property changes value, including to null.
    Test with a path that doesn't repeat the same property names.
    Wrap inside an IBackingStore and feed it to an ExpressionParser
    Create an update strategy for the EP ... Immediate, deferred or manual.
    */
    public class PathBind
    {
        private static char[] _dot = new[] { '.' };

        private readonly PropertyInfo _propertyInfo;
        private PropertyInfo _hostPropertyInfo;

        public Type PropertyType { get; private set; }

        private readonly PathBind _bindingRoot;
        private readonly bool _isLeaf;
        private readonly object _host;
        private readonly string[] _bits;
        private readonly int _currentIndex;
        private readonly string _propertyName;
        private PathBind _child;
        private object _partValue;
        private object _value;
        private readonly Action<object> _valueChanged;
        private string _hostPropertyName;

        public object Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    DoRootValueChanged(value);
                }
            }
        }

        private void DoRootValueChanged(object value)
        {
            if (this != _bindingRoot)
                throw new InvalidOperationException("Something has gone very wrong!");

            if (_hostPropertyInfo != null)
                _hostPropertyInfo.SetValue(_host, value);

            _valueChanged(value);
        }

        public PathBind(object host, string qualifiedName, Action<object> valueChanged = null)
            : this(null, valueChanged ?? ((o) => { }), host, qualifiedName.Split(_dot), 0)
        {
        }

        protected PathBind(PathBind bindingRoot, Action<object> valueChanged, object host, string[] bits, int currentIndex)
        {
            _bindingRoot = bindingRoot ?? this;
            if (_bindingRoot == this)
                _valueChanged = valueChanged;

            _host = host;
            _bits = bits;
            _currentIndex = currentIndex;
            _propertyName = _bits[currentIndex];
            _isLeaf = _currentIndex >= _bits.Length - 1;

            // Get info for the property
            _propertyInfo = host.GetType().GetProperty(_propertyName, BindingFlags.Public | BindingFlags.Instance);

            // Bail out if the property doesn't exist or cannot be read.
            if (_propertyInfo == null || _propertyInfo.CanRead == false)
                return;

            // If the value changes, respond accordingly
            if (_host is INotifyPropertyChanged inpc)
                inpc.PropertyChanged += HostPropertyChanged;

            // Refresh the value of the property
            _partValue = _propertyInfo.GetValue(_host);

            if (_isLeaf == true)
            {
                _bindingRoot.Value = _partValue;
                _bindingRoot.PropertyType = _propertyInfo.PropertyType;
            }
            else if (_partValue != null)
                _child = new PathBind(_bindingRoot, null, _partValue, _bits, _currentIndex + 1);
        }

        /// <summary>
        /// Called by parent when a non-leaf value on the path changes or by the consumer when the instance is no longer wanted.
        /// </summary>
        public void DetachFromProperty()
        {
            if (_child != null)
                _child.DetachFromProperty();

            if (_host is INotifyPropertyChanged inpc)
                inpc.PropertyChanged -= HostPropertyChanged;
        }

        private void HostPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _propertyName)
            {
                // Our property has changed
                if (_child != null)
                    _child.DetachFromProperty();

                // Refresh the value of the property
                _partValue = _propertyInfo.GetValue(_host);

                if ((_isLeaf == true) || (_partValue == null))
                    _bindingRoot.Value = _partValue;
                else
                    _child = new PathBind(_bindingRoot, null, _partValue, _bits, _currentIndex + 1);
            }
            // If 'this' is the root PathBind, and the property we are interested in has changed ...
            else if ((this == _bindingRoot) && (e.PropertyName == _hostPropertyName))
            {
                var newval = _hostPropertyInfo.GetValue(_host);
                SetValue(newval);
            }
        }

        // TODO: PERFORMANCE: Track the leaf node so we can hit it directly.
        protected void SetValue(object newValue)
        {
            if (_isLeaf)
                this._propertyInfo.SetValue(_host, (int)newValue);
            else if (_child != null)
                _child.SetValue(newValue);
        }

        public PathBind BindTo(string hostPropertyName)
        {
            _hostPropertyName = hostPropertyName;

            if (!string.IsNullOrEmpty(hostPropertyName))
            {
                _hostPropertyInfo = _host.GetType().GetProperty(_hostPropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (_hostPropertyInfo?.CanWrite == false)
                    _hostPropertyInfo = null;
            }
            return this;
        }
    }
}
