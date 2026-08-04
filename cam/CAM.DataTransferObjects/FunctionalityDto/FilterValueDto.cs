using System;

namespace CAM.DataTransferObjects.FunctionalityDto
{
    public class FilterValueDto : IEquatable<FilterValueDto>
    {

        public string Value { get; set; }
        public string Text { get; set; }

        public FilterValueDto()
        {

        }
        public FilterValueDto(string val)
        {
            Value = val;
            Text = val;
        }
        public FilterValueDto(string val, string text)
        {
            Value = val;
            Text = text;
        }
        /// <summary>
        /// casta automaticamente object in string
        /// </summary>
        /// <param name="val"></param>
        /// <param name="text"></param>
        public FilterValueDto(object val, object text)
        {
            Value = val?.ToString();
            Text = text?.ToString();
        }

        /// <summary>
        /// casta automaticamente object in string
        /// </summary>
        /// <param name="val"></param>
        /// <param name="text"></param>
        public FilterValueDto(object val)
        {
            Value = val?.ToString();
            Text = val?.ToString();
        }

     
        public bool Equals(FilterValueDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value && Text == other.Text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FilterValueDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Text);
        }
    }
}
