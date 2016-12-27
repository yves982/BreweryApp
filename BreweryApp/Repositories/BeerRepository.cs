using BreweryApp.Models;
using BreweryApp.Utils;
using ResotelApp.Utils;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

namespace BreweryApp.Repositories
{
    /// <summary>
    /// Retrieves Beers from REST webservice (see app.config for URL)
    /// </summary>
    class BeerRepository
    {
        #region Privates
        private async Task<Beer> _fillIngredients(Beer beer)
        {
            string[] segments = { "beer", beer.Id, "ingredients" };
            NameValueCollection extraParams = new NameValueCollection();
            if (beer.Available != null)
            {
                extraParams.Add("availableId", beer.Available.Id.ToString());
            }
            HttpClient client = HttpClientProvider.Client;
            Uri requestUri = HttpClientProvider.BuildRequest(segments, extraParams);
            HttpResponseMessage response = await client.GetAsync(requestUri);

            if(response.IsSuccessStatusCode)
            {
                IngredientsHolder ingredientsHolder = await response.Content.ReadAsAsync<IngredientsHolder>();
                beer.Ingredients.Clear();
                if (ingredientsHolder.Ingredients != null)
                {
                    beer.Ingredients.AddRange(ingredientsHolder.Ingredients);
                }
            }
            return beer;
        }

        private static string _toLcFirst(string str)
        {
            string firstCharString = str[0].ToString();
            string buildString = str.Replace(firstCharString, firstCharString.ToLowerInvariant());
            return buildString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Retrieves all Beers with given availability from configured web service
        /// </summary>
        /// <param name="availableId">Identifier of the availability</param>
        /// <param name="page">Page number to retrieve (web service handles pagination)</param>
        /// <param name="orderField">Field to sort Beers by</param>
        /// <param name="sortDir">Sort direction</param>
        /// <returns>A set of beers (BeerSet) if any still exists, an empty BeerSet otherwise</returns>
        /// <exception cref="ApplicationException">Thrown when REST service does not give an OK status code in [200;400[</exception>
        public async Task<BeerSet> GetBeersAsync(int availableId, int page=1, EOrderField orderField=EOrderField.Name, ESortDir sortDir=ESortDir.Asc)
        {
            BeerSet beerSet = null;

            string[] segments = { "beers" };
            NameValueCollection extraParams = new NameValueCollection();
            string orderFieldLcFirst = _toLcFirst(orderField.ToString());

            extraParams.Add("availableId", availableId.ToString());
            extraParams.Add("order", orderFieldLcFirst);
            extraParams.Add("sort", sortDir.ToString().ToUpperInvariant());
            extraParams.Add("p", page.ToString());

            HttpClient client = HttpClientProvider.Client;
            Uri requestUri = HttpClientProvider.BuildRequest(segments, extraParams);
            Logger.Log($"REQUEST: requesting: {requestUri}");
            HttpResponseMessage response = await client.GetAsync(requestUri.AbsoluteUri);

            if (response.IsSuccessStatusCode)
            {
                Logger.Log($"REQUEST: loaded: {requestUri}");
                beerSet = await response.Content.ReadAsAsync<BeerSet>();
            }
            else
            {
                Logger.Log($"REQUEST: failed: {requestUri}");
                throw new ApplicationException($"{(int)response.StatusCode} - {response.ReasonPhrase} : {await response.Content.ReadAsStringAsync()}.");
            }
            return beerSet;
        }

        /// <summary>
        /// Retrieves a single Beer 
        /// </summary>
        /// <param name="id">Identifier of seeked Beer</param>
        /// <param name="withIngredients">If true, will get ingredients when they're available</param>
        /// <returns>Seeked Beer if it still exists, null otherwise</returns>
        public async Task<Beer> GetBeerAsync(string id, bool withIngredients = true)
        {
            Beer beer = null;
            string[] segments = { "beer", id };
            HttpClient client = HttpClientProvider.Client;

            Uri requestUri = HttpClientProvider.BuildRequest(segments);
            Logger.Log($"REQUEST: requesting: {requestUri}");

            HttpResponseMessage response = await client.GetAsync(requestUri.AbsoluteUri);
            
            if (response.IsSuccessStatusCode)
            {
                Logger.Log($"REQUEST: loaded: {requestUri}");
                BeerHolder beerHolder = await response.Content.ReadAsAsync<BeerHolder>();
                
                beer = beerHolder.Beer;
                if (withIngredients)
                {
                    await _fillIngredients(beer);
                }
            } else
            {
                Logger.Log($"REQUEST: failed: {requestUri}");
                throw new ApplicationException($"{(int)response.StatusCode} - {response.ReasonPhrase} : {await response.Content.ReadAsStringAsync()}.");
            }

            
            return beer;
        }

        /// <summary>
        /// Search Beers by keyword
        /// </summary>
        /// <param name="availableId">Identifier of the availability</param>
        /// <param name="query">Seeked keyword</param>
        /// <param name="page">Page number to retrieve (web service handles pagination)</param>
        /// <param name="orderField">Field to sort Beers by</param>
        /// <param name="sortDir">Sort direction</param>
        /// <returns>A set of beers (BeerSet) if any matches keyword, an empty BeerSet otherwise</returns>
        public async Task<BeerSet> SearchBeersAsync(int availableId, string query, int page=1, EOrderField orderField = EOrderField.Name, ESortDir sortDir = ESortDir.Asc)
        {
            BeerSet beerSet = null;

            string[] segments = { "search" };
            NameValueCollection extraParams = new NameValueCollection();
            extraParams.Add("availableId", availableId.ToString());
            extraParams.Add("q", query);
            extraParams.Add("type", "beer");
            extraParams.Add("order", orderField.ToString().ToLowerInvariant());
            extraParams.Add("sort", sortDir.ToString().ToUpperInvariant());
            extraParams.Add("p", page.ToString());

            HttpClient client = HttpClientProvider.Client;
            Uri requestUri = HttpClientProvider.BuildRequest(segments, extraParams);
            Logger.Log($"REQUEST: requesting: {requestUri}");
            HttpResponseMessage response = await client.GetAsync(requestUri.AbsoluteUri);
            
            if (response.IsSuccessStatusCode)
            {
                beerSet = await response.Content.ReadAsAsync<BeerSet>();
                Logger.Log($"REQUEST: loaded: {requestUri}");
            }
            else
            {
                Logger.Log($"REQUEST: failed: {requestUri}");
                throw new ApplicationException($"{(int)response.StatusCode} - {response.ReasonPhrase} : {await response.Content.ReadAsStringAsync()}.");
            }
            return beerSet;
        }
        #endregion
    }
}
