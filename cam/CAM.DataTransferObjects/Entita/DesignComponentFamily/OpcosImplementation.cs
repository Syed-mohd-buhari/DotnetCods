using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.DesignComponentFamily
{
    public class DCFImplementation
    {
        public string DCFName { get; set; }
        public List<OpcosImplementation> OpcosImplementations { get; set; }
    }
    public class OpcosImplementation
    {
        public string OpcoName { get; set; }
        public string DCName { get; set; }
        public bool ImplementationFlag { get; set; }
        public int ProductionNodesCount { get; set; }
    }
    public class OpcosImplementationDetails
    {
        public string OpcoName { get; set; }
        public string DCName { get; set; }

        public string NodeCountApproach { get; set; }

        public int NumberofNodes { get; set; }

        public string NetworkELement { get; set; }
        public string Location { get; set; }

        public string Enviroment { get; set; }

    }
}
