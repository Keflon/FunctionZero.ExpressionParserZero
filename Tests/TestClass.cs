using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zBindTests
{
    public class TestClass : INotifyPropertyChanged
    {
        public TestClass(TestClass child, int startValue)
        {
            Child = child;
            TestInt = startValue;
        }
        public TestClass(int startValue) : this(null, startValue) { }

        private int _testInt;

        public int TestInt
        {
            get
            {
                return _testInt;
            }
            set
            {
                if (value != _testInt)
                {
                    _testInt = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TestInt)));
                }
            }
        }

        private TestClass _child;

        public TestClass Child
        {
            get
            {
                return _child;
            }
            set
            {
                if (value != _child)
                {
                    _child = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Child)));
            }
        }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
