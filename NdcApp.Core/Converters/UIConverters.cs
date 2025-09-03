using System;
using System.Globalization;

namespace NdcApp.Core.Converters
{
    public class InverseBoolConverter
    {
        public bool Convert(bool value)
        {
            return !value;
        }
    }

    public class SelectedTextConverter
    {
        public string Convert(bool selected)
        {
            return selected ? "Selected" : "Select";
        }
    }

    public class SelectedColorConverter
    {
        public string Convert(bool selected)
        {
            return selected ? "#FFB400" : "#0A2342";
        }
    }
}