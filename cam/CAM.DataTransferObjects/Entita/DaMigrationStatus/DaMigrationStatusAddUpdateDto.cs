using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.DaMigrationStatus
{
    public class DaMigrationStatusAddUpdateDto
    {
        public DaMigrationStatusAddUpdateDto()
        {
            statusKeyPairValue = new Dictionary<int, string>();
            daMigrationStatusDtoGrids = new List<DaMigrationStatusDtoGrid>();
        }
        public Dictionary<int, string> statusKeyPairValue { get; set; }

        public List<DaMigrationStatusDtoGrid> daMigrationStatusDtoGrids { get; set; }

        public Dictionary<short,string> LocationResource { get; set; }
    }
}
