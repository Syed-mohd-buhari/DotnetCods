using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.InterVMType;
using CAM.DataTransferObjects.LookUp.VNFName;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Vbom;
using CAM.Entities.Models.VBom;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Drawing.Charts;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.LookUp
{
    public class VnfNameManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DropdownDataServiceManager _dropDownManager;


        public VnfNameManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, DropdownDataServiceManager dropownManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _dropDownManager = dropownManager;
        }
        #region UI Member function
        public ExpressionStarter<Vnfname> ApplyFilterForOracleModel(VnfNameQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Vnfname>();

            if (request.Vnfnameid != null && request.Vnfnameid.Any())
            {
                var predicateInner = PredicateBuilder.New<Vnfname>();
                foreach (var item in request.Vnfnameid)
                    predicateInner.Or(x => x.Vnfnameid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Vnfdescription != null && request.Vnfdescription.Any())
            {
                var predicateInner = PredicateBuilder.New<Vnfname>();
                foreach (var item in request.Vnfdescription)
                    predicateInner.Or(x => x.Vnfdescription == item);
                predicateResult.And(predicateInner);
            }
            if (request.Productid != null && request.Productid.Any())
            {
                var predicateInner = PredicateBuilder.New<Vnfname>();
                foreach (var item in request.Productid)
                    predicateInner.Or(x => x.Productid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Vnfname>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Vnfname>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(VnfNameQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<VnfNameDtoGrid>(new GenerateRenderForGrid<VnfNameDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Vnfname>(true);
            var fullQuery = await Task.Run(() => PrepareQuery(predicateResult));
            var allItems = MappingDto(fullQuery.ToList());

            return new ResultDto
            {
                Data = new
                {
                    rtn.GridRender,
                    rtn.Items,
                    rtn.TotalItems,
                    allItems
                }
            };
        }

        public List<VnfNameDtoGrid> MappingDto(List<VnfName> query)
        {
            var result = new List<VnfNameDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new VnfNameDtoGrid();

                    grid.VnfNameId = x.VnfNameId;
                    grid.VnfDescription = x.VnfDescription;
                    grid.Product = x.Product.Description;
                    grid.LastModified = x.ModificationDate;
                    grid.LastModifiedBy = x.ModificationUserEntity.Email;
                    return grid;
                }).ToList();
            }
            catch
            {
                return result;
            }

        }


        public Dictionary<string, Expression<Func<VnfName, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VnfName, object>>[]>
            {
                ["vnfNameId"] = new Expression<Func<VnfName, object>>[] { p => p.VnfNameId },
                ["vnfDescription"] = new Expression<Func<VnfName, object>>[] { p => p.VnfDescription },
                ["productId"] = new Expression<Func<VnfName, object>>[] { p => p.ProductId },
                ["modificationUser"] = new Expression<Func<VnfName, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, VnfNameQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "vnfNameId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VnfNameId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.VnfNameId))
                    .Distinct()
                    .ToList(),

                "vnfDescription" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VnfDescription.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.VnfDescription))
                    .Distinct()
                    .ToList(),

                "product" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ProductId.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto { Text = x.Product.Description, Value = x.Product.Id.ToString()})
                    .Distinct()
                    .ToList(),

                "lastModifiedBy" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationUserEntity.Email.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.ModificationUserEntity.Email))
                    .Distinct()
                    .ToList(),

                _ => new List<FilterValueDto>()
            };
            return result;
        }

        public IQueryable<VnfName> PrepareQuery(ExpressionStarter<Vnfname> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.VnfNameRepository.FindByCondition(predicateResult)
                .Include(m => m.Product)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.VnfNameRepository.FindAll()
                 .Include(m => m.Product)
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => VnfNameMapper.GetVnfName(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(VnfNameCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.VnfNameRepository.FindByCondition(
               x => x.Vnfdescription.ToLower().Trim().Replace(" ", "") == dto.VnfDescription.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnfnameid
                };
            }

            Vnfname entity = new Vnfname()
            {
                Vnfnameid = dto.VnfNameId,
                Vnfdescription = dto.VnfDescription,
                Productid = dto.ProductId,
            };

            _repositoryWrapper.VnfNameRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<VnfNameCreateDto> GetCreatePage()
        {
            
            var dto = new VnfNameCreateDto();
            dto.ProductResource =  await Task.Run(()=>_dropDownManager.GetProductNameDropDown());
            return dto;

        }

        public async Task<VnfNameUpdateDto> GetUpdatedPage(long id)
        {

            var vnfNameModel = await _repositoryWrapper.VnfNameRepository.FindByCondition(x => x.Vnfnameid == id).Include(x => x.Product).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new VnfNameUpdateDto();
            if (vnfNameModel != null)
            {
                var vnfNameEntity = VnfNameMapper.GetVnfName(vnfNameModel);
                if (vnfNameEntity != null)
                {
                    dto.VnfNameId = vnfNameEntity.VnfNameId;
                    dto.VnfDescription = vnfNameEntity.VnfDescription;
                    dto.ProductId = vnfNameEntity.ProductId.Value;
                    dto.LastModified = vnfNameEntity.ModificationDate;
                    dto.LastModifiedBy = vnfNameEntity.ModificationUserEntity.Email;
                    dto.ProductResource = await Task.Run(() => _dropDownManager.GetProductNameDropDown());
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(VnfNameUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.VnfNameRepository.FindByCondition(
                   x => x.Vnfnameid != dto.VnfNameId
                   && x.Vnfdescription.ToLower().Trim().Replace(" ", "") == dto.VnfDescription.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnfnameid
                };
            }
            Vnfname model = new Vnfname()
            {
                Vnfdescription = dto.VnfDescription,
                Productid = dto.ProductId,
                Vnfnameid = dto.VnfNameId,
                Deleted = false
            };
            _repositoryWrapper.VnfNameRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.VnfNameRepository.FindByCondition(x => x.Vnfnameid == id).SingleAsync();
            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();
            var relatedVmTypeName = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfnameid != 0))
                {

                    return new ResultDto
                    {
                        Info = $"{entity.Vnfnameid}, This Id has Linked to VNF Info ",
                        Data = entity.Vnfnameid
                    };
                }
            }
            if (relatedVmTypeName != null && relatedVmTypeName.Count > 0)
            {
                if (relatedVmTypeName.Any(x => x.Vnfnameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"{entity.Vnfnameid}, This Id has Linked to VM Type Name ",
                        Data = entity.Vnfnameid
                    };
                }
            }
            _repositoryWrapper.VnfNameRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vnfnameid
            };

        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.VnfNameRepository
            .FindByCondition(x => x.Vnfnameid == id)
            .SingleAsync();

            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();
            var relatedVmTypeName = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfnameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"VNF Name '{entity.Vnfdescription}', is Linked with VNF Info ",
                        Data = entity.Vnfnameid,
                        Warning = true
                    };
                }
            }
            if (relatedVmTypeName != null && relatedVmTypeName.Count > 0)
            {
                if (relatedVmTypeName.Any(x => x.Vnfnameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"{entity.Vnfnameid}, This Id has Linked to VM Type Name ",
                        Data = entity.Vnfnameid
                    };
                }
            }

            _repositoryWrapper.VnfNameRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vnfnameid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            var rm = new List<ResultMessageDto>();
            var entity = await _repositoryWrapper.VnfNameRepository.FindByCondition(x => x.Vnfnameid == id).SingleAsync();
            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();
            var relatedVmTypeName = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfnameid != 0))
                {
                    rm.Add(new ResultMessageDto { Table = "VNF Info", Values = relatedVnfInfo.Select(x=>x.Vnfnameid.ToString()).ToArray() });                  
                }
            }
            if (relatedVmTypeName != null && relatedVmTypeName.Count > 0)
            {
                if (relatedVmTypeName.Any(x => x.Vnfnameid != 0))
                {
                    rm.Add(new ResultMessageDto { Table = "VM Type Name", Values = relatedVmTypeName.Select(x => x.Vnfnameid.ToString()).ToArray() });

                }
            }
            return new ResultDto
            {
                Warning = true,
                Info = ResultMessages.EntryDeleteNotOrphan,
                Data = new RelatedRecordsResultDto
                {
                    EntityName = "VNF Name",
                    RecordName = entity.Vnfdescription,
                    DataRelatedList = rm
                }
            };
        }
        #endregion

    }
}
