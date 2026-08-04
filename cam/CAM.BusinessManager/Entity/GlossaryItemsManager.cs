using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class GlossaryItemsManager : BaseManager
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _manager;
        public GlossaryItemsManager(
            IEnumerable<IRepositoryWrapper> wrappers,
            IMapper mapper,
            GridCustomColumnManager manager,
            IRepositoryWrapper repositoryWrapper,
            IHttpContextAccessor contextAccessor
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }

        public async Task<IEnumerable<GlossaryItemsGridDto>> FindAll()
        {
            var data = _repositoryWrapper.GlossaryItemsRepository.FindAll().OrderBy(e => e.Header);
            var mappedModelsData = data.AsEnumerable().Select(p => GlossaryItemsMapper.GetGlossaryItemMapper(p));
            var gridData = _mapper.Map<IEnumerable<GlossaryItemsGridDto>>(mappedModelsData);

            return gridData;
        }

        public async Task<ResultDto> Add(GlossaryItemsDtoCreate dto, bool? forced = false)
        {
            var entity = _mapper.Map<GlossaryItems>(dto);
            _repositoryWrapper.GlossaryItemsRepository.Create(GlossaryItemsMapper.SetGlossaryItemMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<GlossaryItems> EntityExists(GlossaryItemsDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.GlossaryItemsRepository.FindByCondition(e => e.Glossaryitemsid == dto.GlossaryItemsId)
                .FirstOrDefaultAsync();
            return GlossaryItemsMapper.GetGlossaryItemMapper(entityExists);
        }

        public async Task<ResultDto> Update(GlossaryItemsDtoUpdate dto, bool? forced)
        {
            try
            {
                var originalEntityWithSameNaturalKey = await _repositoryWrapper.GlossaryItemsRepository
                    .FindByCondition(e => e.Glossaryitemsid == dto.GlossaryItemsId)
                    .SingleOrDefaultAsync();

                if (originalEntityWithSameNaturalKey == null)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateNotExists,
                        Data = dto.GlossaryItemsId
                    };
                }


                if (forced == true)
                {
                    originalEntityWithSameNaturalKey.Deleted = false;
                    originalEntityWithSameNaturalKey.Deletiondate = null;
                }

                GlossaryItems entity = new GlossaryItems()
                {
                    GlossaryItemsId = dto.GlossaryItemsId,
                    Description = dto.Description,
                    Header = dto.Header
                };

                var item = GlossaryItemsMapper.SetGlossaryItemMapper(entity);

                _repositoryWrapper.GlossaryItemsRepository.Update(item);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = dto.GlossaryItemsId
                };

            }
            catch (Exception ex)
            {
                throw;
            }

        }




        public async Task<ResultDto> Delete(long id)
        {

            try
            {
                var entity = await _repositoryWrapper.GlossaryItemsRepository.FindByCondition(x => x.Glossaryitemsid == id).SingleAsync();
                _repositoryWrapper.GlossaryItemsRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Glossaryitemsid
                };

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<ResultDto> Restore(long id)
        {

            var entity = await _repositoryWrapper.GlossaryItemsRepository.FindByCondition(x => x.Glossaryitemsid == id, true).SingleAsync();

            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.GlossaryItemsRepository.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Glossaryitemsid
            };
        }

        public GlossaryItemsDto Get(long id)
        {
            var entity = _repositoryWrapper.GlossaryItemsRepository.FindByCondition(x => x.Glossaryitemsid == id).Single();
            return _mapper.Map<GlossaryItemsDto>(entity);
        }

    }
}