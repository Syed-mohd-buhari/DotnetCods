import { DesignComponentDtoCreate } from "./DesignComponent";
import { MajorHardwareBuildDtoUpdate } from "./MajorHardwareBuild";
import { MajorSoftwareBuildDtoCreate } from "./MajorSoftwareBuild";
import { SystemTypeDtoCreate } from "./SystemTypeModel";

/**
 *
 * @export
 * @interface InizializeNewProductCreateDto
 */
export interface InizializeNewProductCreateDto {
  /**
   *
   * @type {DesignComponentDtoCreate}
   * @memberof InizializeNewProductCreateDto
   */
  designComponentDto: DesignComponentDtoCreate;
  /**
   *
   * @type {SystemTypeDtoCreate}
   * @memberof InizializeNewProductCreateDto
   */
  systemTypeDto: SystemTypeDtoCreate;
  /**
   *
   * @type {MajorSoftwareBuildDtoCreate}
   * @memberof InizializeNewProductCreateDto
   */
  majorSoftwareBuildDto: MajorSoftwareBuildDtoCreate;
  /**
   *
   * @type {MajorHardwareBuildDtoUpdate}
   * @memberof InizializeNewProductCreateDto
   */
  majorHardwareBuildDto: MajorHardwareBuildDtoUpdate;
}
