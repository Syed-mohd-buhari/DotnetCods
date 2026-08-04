using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{

    public class IdentityAsIsDtoQuery : QueryObject
    {
        public List<int> Id { get; set; }
        public List<string> Value { get; set; }
        public List<string> ResourceKey { get; set; }
        public List<string> PreviousResourceKey { get; set; }
        public List<string> CategoryDescription { get; set; }
        public List<string> ClassDescription { get; set; }
        public List<string> TypeDescription { get; set; }
        public List<long> AssetId { get; set; }
        public DateFilter LastModifiedValue { get; set; }
        public List<string> AssetName { get; set; }
        public List<int> DesignComponentFamily { get; set; }

        public List<string> InterfaceType { get; set; }

        public List<string> InterfaceName { get; set; }

        public List<int> VerticalId { get; set; }

        public List<string> VerticalName { get; set; }

        public List<short> OpCoId { get; set; }

        public List<string> OpCo { get; set; }
        public List<long> SupportedService { get; set; }
    }
}
