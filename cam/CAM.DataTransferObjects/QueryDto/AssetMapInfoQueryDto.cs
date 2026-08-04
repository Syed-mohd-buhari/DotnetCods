using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AssetMapInfoQueryDto:QueryObject
    {
        public List<long> AssetMapInfoId { get; set; }
        public List<string> OmcAssetName { get; set; }
        public List<string> TemsAssetName { get; set; }
        public List<string> EnmAssetName { get; set; }
        public List<string> Site { get; set; }
        public List<string> DataSourceName { get; set; }
    }
}
