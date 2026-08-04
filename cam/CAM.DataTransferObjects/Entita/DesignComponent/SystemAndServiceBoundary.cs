using System;
using System.Text;

namespace CAM.DataTransferObjects.Entita.DesignComponent
{
    public class SystemAndSubNetworkBoundary : IEquatable<SystemAndSubNetworkBoundary>
    {
        public long SystemTypeId { get; set; }
        public string SystemSolutionName { get; set; }
        public long SubNetworkBoundaryId { get; set; }
        public string SubNetworkBoundaryName { get; set; }

        public string SubNetworkBoundaryAlias { get; set; }

      public bool Equals(SystemAndSubNetworkBoundary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SystemTypeId == other.SystemTypeId && SystemSolutionName == other.SystemSolutionName && SubNetworkBoundaryId == other.SubNetworkBoundaryId && SubNetworkBoundaryName == other.SubNetworkBoundaryName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SystemAndSubNetworkBoundary)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SystemTypeId, SystemSolutionName, SubNetworkBoundaryId, SubNetworkBoundaryName);
        }
    }
}
