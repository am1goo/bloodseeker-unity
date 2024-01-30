using System;
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public class AndroidJavaArray : IDisposable
    {
        private string _javaClass;
        private int _length;
        public int length => _length;

        private AndroidJavaClass _arrayClass;
        private AndroidJavaObject _arrayObject;
        private AndroidJavaObject[] _arrayValues;

        public AndroidJavaArray(int length, string javaClass)
        {
            _length = length;
            _javaClass = javaClass;

            
            _arrayClass = new AndroidJavaClass("java.lang.reflect.Array");
            _arrayObject = _arrayClass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass(javaClass), length);
            _arrayValues = new AndroidJavaObject[length];
        }
        
        public AndroidJavaObject this[int index]
        {
            set
            {
                _arrayClass.CallStatic("set", _arrayObject, index, value);
                _arrayValues[index] = value;
            }
        }

        public AndroidJavaObject AsJavaObject()
        {
            return _arrayObject;
        }

        public void Dispose()
        {
            if (_arrayValues != null)
            {
                for (int i = 0; i < _arrayValues.Length; ++i)
                {
                    var wrapper = _arrayValues[i];
                    if (wrapper == null)
                        continue;

                    wrapper.Dispose();
                }
                _arrayValues = null;
            }
            
            if (_arrayClass != null)
            {
                _arrayClass.Dispose();
                _arrayClass = null;
            }

            if (_arrayObject != null)
            {
                _arrayObject.Dispose();
                _arrayObject = null;
            }

            _javaClass = null;
            _length = 0;
        }

        public static implicit operator AndroidJavaObject(AndroidJavaArray array)
        {
            return array._arrayObject;
        }
    }
}
