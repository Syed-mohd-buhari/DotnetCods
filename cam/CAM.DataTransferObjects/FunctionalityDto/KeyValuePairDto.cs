using System;

namespace CAM.DataTransferObjects.FunctionalityDto
{
    public class KeyValuePairDto : IEquatable<KeyValuePairDto>
    {

         public long  Key { get; set; }
        public string Text { get; set; }

        public KeyValuePairDto()
        {

        }
      
        public KeyValuePairDto(short val, string text)
        {
            Key = val;
            Text = text;
        }

        public bool Equals(KeyValuePairDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key == other.Key && Text == other.Text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeyValuePairDto)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Text);
        }
    }
}
