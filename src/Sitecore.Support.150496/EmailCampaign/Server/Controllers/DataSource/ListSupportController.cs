using Sitecore.ListManagement.Services;
using Sitecore.ListManagement.Services.Model;
using Sitecore.ListManagement.Services.Services.List;
using Sitecore.Modules.EmailCampaign.Services;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Sitecore.Support.EmailCampaign.Server.Controllers.DataSource
{
  [ServicesController("EXM.ListSupport")]
  [AnalyticsDisabledFilter, AccessDeniedExceptionFilter, UnauthorizedAccessExceptionFilter, SitecoreAuthorize(Roles = @"sitecore\List Manager Editors")]
  public class ListSupportController : ServicesApiController
  {
    private readonly IListService _listService;
    private readonly IExmCampaignService _exmCampaignService;

    public ListSupportController(IListService listService, IExmCampaignService exmCampaignService)
    {
      _listService = listService;
      _exmCampaignService = exmCampaignService;
    }

    [ActionName("DefaultAction")]
    public FetchResult<ListModel> GetAllLists(string messageId, int pageIndex = 1, int pageSize = 20)
    {
      var results = this._listService.GetAllLists(String.Empty, pageIndex, pageSize);
      List<ListModel> listModelCollection = new List<ListModel>(results.Items);
      var messageGuid = Guid.Parse(messageId);
      var message = _exmCampaignService.GetMessageItem(messageGuid);
      var commonOptOutListID = Guid.Parse(message.ManagerRoot.Settings.GlobalOptOutList).ToString("D").ToLowerInvariant();

      foreach (var currentList in listModelCollection)
      {
        Guid current;
        bool isParsed = Guid.TryParse(currentList.itemId, out current);

        if (!isParsed)
        {
          continue;
        }

        if (current.ToString("D").ToLowerInvariant() == commonOptOutListID)
        {
          listModelCollection.Remove(currentList);
          break;
        }
      }

      FetchResult<ListModel> updatedResult = new FetchResult<ListModel>(listModelCollection, listModelCollection.Count);

      return updatedResult;
    }
  }
}