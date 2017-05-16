/* Copyright(C) 2016  Teambrella, Inc.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License(version 3) as published
 * by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see<http://www.gnu.org/licenses/>.
 */
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using NBitcoin;
using Teambrella.Client.DomainModel;
using Teambrella.Client.ServerApiModels;

namespace Teambrella.Client.Services
{
    public class Server
    {
        private static string _siteUrl = "https://teambrella.com";
        private RestClient _restClient = new RestClient(_siteUrl);
        private long _timestamp;

        public const decimal NoAutoApproval = 1000000;

        public static string GetSiteUrl()
        {
            return _siteUrl;
        }

        public LoginQuery CreateBrowseOpenModel(Key bitcoinPrivateKey)
        {
            var model = new LoginQuery();
            model.AddSignature(_timestamp, bitcoinPrivateKey);
            return model;
        }

        public bool TryInitTimestamp()
        {
            var request = new RestRequest("me/GetTimestamp", Method.POST);
            request.RequestFormat = DataFormat.Json;
            var responseTimestamp = _restClient.Execute<ApiResult>(request);
            if (responseTimestamp.Data == null)
            {
                return false;
            }

            var status = responseTimestamp.Data.Status;
            if (status.ResultCode != 0)
            {
                return false;
            }

            _timestamp = status.Timestamp;
            return true;
        }

        public ApiResult InitClient(Key bitcoinPrivateKey)
        {
            if (TryInitTimestamp())
            {
                var modelIn = new ApiQuery();
                modelIn.AddSignature(_timestamp, bitcoinPrivateKey);
                var request = new RestRequest("me/InitClient", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(modelIn);
                var responseInit = _restClient.Execute<ApiResult>(request);

                ApiResult result = responseInit.Data;
                if (result != null && result.Status != null && result.Status.ResultCode == 0)
                {
                    _timestamp = result.Status.Timestamp;
                    return result;
                }
            }

            return null;
        }

        public GetUpdatesResultData GetUpdates(Key bitcoinPrivateKey, long since, IEnumerable<Tx> txsToUpdate, IEnumerable<TxSignature> txSignaturesToUpdate)
        {
            var modelIn = new GetUpdatesApiQuery();
            modelIn.AddSignature(_timestamp, bitcoinPrivateKey);
            modelIn.Since = since;
            modelIn.TxInfos = txsToUpdate.Select(x => new TxClientInfoApi
            {
                Id = x.Id,
                ResolutionTime = x.ClientResolutionTime,
                Resolution = x.Resolution
            }).ToList();
            modelIn.TxSignatures = txSignaturesToUpdate.Select(x => new TxSignatureClientInfoApi
            {
                Signature = Convert.ToBase64String(x.Signature),
                TeammateId = x.Teammate.Id,
                TxInputId = x.TxInput.Id
            }).ToList();

            var request = new RestRequest("me/GetUpdates", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(modelIn);
            var response = _restClient.Execute<ApiResult<GetUpdatesResultData>>(request);
            return response.Data.Data;
        }
    }
}
