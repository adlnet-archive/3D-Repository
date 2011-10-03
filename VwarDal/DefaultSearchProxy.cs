using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    class DefaultSearchProxy : ISearchProxy
    {
        private string _connectionString;
        private IMetadataStore _dataStore;
        private string _identity;
        private ContentObjectEqualityComparer _compare = new ContentObjectEqualityComparer();

        public DefaultSearchProxy(string connectionString, string identity)
        {
            _connectionString = connectionString;
            _dataStore = new MySqlMetadataStore(_connectionString);
            _identity = identity;
        }

        public IEnumerable<ContentObject> QuickSearch(string term)
        {
            IEnumerable<ContentObject> results = null;

            results = combineResults(results, GetContentObjectsByTitle(term), SearchMethod.OR);
            results = combineResults(results, GetContentObjectsByDescription(term), SearchMethod.OR);
            results = combineResults(results, GetContentObjectsByKeyWords(term), SearchMethod.OR);

            return results;
        }

        public IEnumerable<ContentObject> QuickSearch(IEnumerable<string> terms, SearchMethod method = SearchMethod.OR)
        {
            if (terms.Count() < 1)
                return null;

            IEnumerable<ContentObject> results = null;
            foreach (string term in terms)
                results = combineResults(results, QuickSearch(term), method);

            return results;
        }

        public IEnumerable<ContentObject> SearchByField(string field, string term)
        {
            IEnumerable<ContentObject> results = null;
            field = field.ToLowerInvariant();

            switch (field)
            {
                case "title":
                case "description":
                case "developername":
                case "sponsorname":
                case "artistname":
                    results = _dataStore.GetContentObjectsByField(field, term, _identity);
                    break;

                case "submitteremail":
                    results = _dataStore.GetContentObjectsByField("submitter", term, _identity);
                    break;

                case "keywords":
                    results = this.GetContentObjectsByKeyWords(field);
                    break;

                default:
                    throw new Exception("Referenced field not searchable in MySQL metadata");
            }

            return results;
        }

        public IEnumerable<ContentObject> SearchByFields(NameValueCollection fields, SearchMethod method = SearchMethod.OR)
        {
            if (fields.Count < 1)
                return null;

            IEnumerable<ContentObject> results = null;
            for (int i = 0; i < fields.Count; i++)
                results = combineResults(results, SearchByField(fields.Keys[i], fields[fields.Keys[i]]), method);

            return results;
        }

        public IEnumerable<ContentObject> DeepSearch(string term)
        {
            NameValueCollection nvc = new NameValueCollection();
            string[] fields = { "title", "description", "keywords", "developername", "sponsorname", "artistname" };
            foreach (string field in fields)
                nvc[field] = term;

            return SearchByFields(nvc);
        }

        //This is horribly inefficient. Should only be used when absolutely necessary, like finding a troll
        public IEnumerable<ContentObject> DeepSearch(IEnumerable<string> terms, SearchMethod method = SearchMethod.OR)
        {
            IEnumerable<ContentObject> results = null;
            foreach (string term in terms)
                results = combineResults(results, DeepSearch(term), method);

            return results;
        }

        public IEnumerable<ContentObject> GetRecentlyViewed(int count, int start = 0)
        {
            return _dataStore.GetObjectsWithRange("{CALL GetMostRecentlyViewed(?,?,?)}", count, start, _identity);
        }

        public IEnumerable<ContentObject> GetHighestRated(int count, int start = 0)
        {
            return _dataStore.GetObjectsWithRange("{CALL GetHighestRated(?,?,?)}", count, start, _identity);
        }

        public IEnumerable<ContentObject> GetMostPopular(int count, int start = 0)
        {
            return _dataStore.GetObjectsWithRange("{CALL GetMostPopular(?,?,?)}", count, start, _identity);
        }

        public IEnumerable<ContentObject> GetRecentlyUpdated(int count, int start = 0)
        {
            return _dataStore.GetObjectsWithRange("{CALL GetMostRecentlyUpdated(?,?,?)}", count, start, _identity);
        }

        public IEnumerable<ContentObject> GetContentObjectsByDeveloperName(string developerName)
        {
            return SearchByField("developername", developerName);
        }

        public IEnumerable<ContentObject> GetContentObjectsBySponsorName(string sponsorName)
        {
            return SearchByField("sponsorname", sponsorName);
        }

        public IEnumerable<ContentObject> GetContentObjectsByArtistName(string artistName)
        {
            return SearchByField("artistname", artistName);
        }

        public IEnumerable<ContentObject> GetContentObjectsByKeyWords(string keywordsList)
        {
            return _dataStore.GetContentObjectsByKeywords(keywordsList, _identity);
        }

        public IEnumerable<ContentObject> GetContentObjectsByDescription(string description)
        {
            return SearchByField("description", description);
        }

        public IEnumerable<ContentObject> GetContentObjectsByTitle(string title)
        {
            return SearchByField("title", title);
        }

        public IEnumerable<ContentObject> GetContentObjectsBySubmitterEmail(string email)
        {
            var results = from obj in _dataStore.GetAllContentObjects()
                          where obj.SubmitterEmail.Equals(email, StringComparison.InvariantCultureIgnoreCase)
                          select obj;

            return results;
        }

        /// <summary>
        /// Cleanly combines two IEnumerables (possibly null) 
        /// </summary>
        /// <param name="co1">The existing ContentObjects result</param>
        /// <param name="co2">The new ContentObjects you would like to add to the result</param>
        /// <param name="method">The method by which to join the results (union or intersect)</param>
        /// <returns>An IEnumerable containing the combined inputs</returns>
        private IEnumerable<ContentObject> combineResults(IEnumerable<ContentObject> existing, IEnumerable<ContentObject> toAdd, SearchMethod method)
        {
            if (toAdd.Count() > 0)
            {
                if (existing == null)
                    existing = toAdd;
                else
                {
                    existing = (method == SearchMethod.OR)
                        ? existing.Union(toAdd, _compare)
                        : existing.Intersect(toAdd, _compare);
                }
            }
            return existing;
        }
    }
}
