using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAM.DataTransferObjects.VIA;

namespace CAM.BusinessManager.Entity.Vai
{
   public static class ViaExportExtension
    {
        public static IQueryable<T> ApplyPagingViaExport<T>(this IQueryable<T> query, ViaExportQuery queryObj,bool getOnlyOne)
        {
            queryObj.SkipManager ??= new List<SkipManager>();

            var skipRecord = queryObj.SkipManager.OrderBy(x=>x.Page).Where(x=>x.Page <= queryObj.Page).Sum(x =>x.SkipAmount) + (getOnlyOne ? queryObj.PageSize : 0 );
            if (queryObj.Page <= 0)
                queryObj.Page = 1;

            if (queryObj.PageSize <= 0)
                queryObj.PageSize = query.Count();
            if (queryObj.PageSize == 0) return query;
            return query.Skip(((queryObj.Page - 1) * queryObj.PageSize) + skipRecord).Take(getOnlyOne ? 1 :queryObj.PageSize);
        }

        public static IQueryable<T> ApplyPagingViaExportStart<T>(this IQueryable<T> query, ViaExportQuery queryObj)
        {
            queryObj.SkipManager ??= new List<SkipManager>();


            if (queryObj.Page <= 0)
                queryObj.Page = 1;

            var skipRecord = queryObj.Page == 1 ?  0 : queryObj.SkipManager.OrderBy(x => x.Page).Where(x => x.Page <= queryObj.Page).Sum(x => x.SkipAmount);
            if (queryObj.PageSize <= 0)
                queryObj.PageSize = query.Count();
            if (queryObj.PageSize == 0) return query;
            return query.Skip(((queryObj.Page - 1) * queryObj.PageSize) + skipRecord).Take(queryObj.PageSize);
        }

    }
}
