using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    /// <summary>
    /// Determines whether to Union or Intersect the results of an aggregated search.
    /// Union = OR, Intersect = AND
    /// </summary>
    public enum SearchMethod { OR = 0, AND = 1 };
    public enum SortOrder { Descending = 0, Ascending = 1 };

    /// <summary>
    /// Defines a proxy by which to search the Data Access Layer
    /// </summary>
    public interface ISearchProxy
    {
        void Dispose();
        /// <summary>
        /// Searches by the default included fields, 
        /// which are Title, Description, and Keywords.
        /// </summary>
        /// <param name="term">The term to search for.</param>
        /// <returns>
        /// An IEnumerable containing all the content objects 
        /// said to match the search term.
        /// </returns>
        IEnumerable<ContentObject> QuickSearch(string term);


        string[] GetMostPopularTags();
        string[] GetMostPopularDevelopers();
        /// <summary>
        /// Searches by the default included fields, 
        /// which are Title, Description, and Keywords.
        /// </summary>
        /// <param name="term">A list of terms to search for.</param>
        /// <param name="method">Specifies whether you want require results to match all field-term pairs or just at least one.</param>
        /// <returns>
        /// An IEnumerable containing all the content objects 
        /// said to match the search terms.
        /// </returns>
        IEnumerable<ContentObject> QuickSearch(IEnumerable<string> terms, SearchMethod method);

        /// <summary>
        /// Searches for term in the specified field over all ContentObjects.
        /// </summary>
        /// <param name="field">The field to search in.</param>
        /// <param name="term">The term to search for</param>
        /// <returns>
        /// An IEnumerable containing all the content objects 
        /// said to match the search term.
        /// </returns>
        IEnumerable<ContentObject> SearchByField(string field, string term);

        /// <summary>
        /// Searches through each field-term pair individually.
        /// </summary>
        /// <param name="fields">The field-term collection to search for</param>
        /// <param name="method">Specifies whether you want require results to match all field-term pairs or just at least one.</param>
        /// <returns>
        /// An IEnumerable containing all the content objects
        /// matching the conditions specified by the parameter combination.
        /// </returns>
        IEnumerable<ContentObject> SearchByFields(NameValueCollection fields, SearchMethod method);

        /// <summary>
        /// Searches through all available fields for a particular term.
        /// </summary>
        /// <param name="term">The term to search for</param>
        /// <returns>
        /// An IEnumerable containing all the content objects 
        /// said to match the search term.
        /// </returns>
        IEnumerable<ContentObject> DeepSearch(string term);

        /// <summary>
        ///  Searches through all available fields for a set of terms.
        /// </summary>
        /// <param name="term">A list of terms to search for.</param>
        /// <param name="method">Specifies whether you want require results to match all field-term pairs or just at least one.</param>
        /// <returns>
        /// An IEnumerable containing all the content objects 
        /// said to match the search terms.
        /// </returns>
        IEnumerable<ContentObject> DeepSearch(IEnumerable<string> terms, SearchMethod method);

        /// <summary>
        /// Gets a range of ContentObjects, ordered by the last time someone accessed them.
        /// </summary>
        /// <param name="count">The number of objects to return</param>
        /// <param name="start">The index of resulting set from which to start choosing objects</param>
        /// <returns>IEnumerable containing <i>count</i> most recently viewed ContentObjects, starting at <i>start</i></returns>
        IEnumerable<ContentObject> GetByLastViewed(int count, int start = 0, SortOrder order = SortOrder.Descending);

        /// <summary>
        /// Gets a range of ContentObjects, ordered by the last time the content was uploaded or edited.
        /// </summary>
        /// <param name="count">The number of objects to return</param>
        /// <param name="start">The index of resulting set from which to start choosing objects</param>
        /// <returns>IEnumerable containing <i>count</i> of the most viewed ContentObjects, starting at <i>start</i></returns>
        IEnumerable<ContentObject> GetByLastUpdated(int count, int start = 0, SortOrder order = SortOrder.Descending);

        /// <summary>
        /// Gets a range of ContentObjects, ordered by the average user rating.
        /// </summary>
        /// <param name="count">The number of objects to return</param>
        /// <param name="start">The index of resulting set from which to start choosing objects</param>
        /// <returns>IEnumerable containing <i>count</i> of the highest rated ContentObjects, starting at <i>start</i></returns>
        IEnumerable<ContentObject> GetByRating(int count, int start = 0, SortOrder order = SortOrder.Descending);

        /// <summary>
        /// Gets a range of ContentObjects, ordered by the number of views.
        /// </summary>
        /// <param name="count">The number of objects to return</param>
        /// <param name="start">The index of resulting set from which to start choosing objects</param>
        /// <returns>IEnumerable containing <i>count</i> of the most viewed ContentObjects, starting at <i>start</i></returns>
        IEnumerable<ContentObject> GetByViews(int count, int start = 0, SortOrder order = SortOrder.Descending);

        /// <summary>
        /// Gets a range of ContentObjects randomly.
        /// </summary>
        /// <param name="count">The number of objects to return</param>
        /// <param name="start">The index of resulting set from which to start choosing objects</param>
        /// <returns>IEnumerable containing <i>count</i> of the most viewed ContentObjects, starting at <i>start</i></returns>
        IEnumerable<ContentObject> GetByRandom(int count, int start = 0);

        /// <summary>
        /// Gets a list of ContentObjects that have a DeveloperName matching the input parameter
        /// </summary>
        /// <param name="developerName">The name of the developer to search for</param>
        /// <returns>An IEnumerable containing ContentObjects that match the DeveloperName</returns>
        IEnumerable<ContentObject> GetContentObjectsByDeveloperName(string developerName);

        /// <summary>
        /// Gets a list of ContentObjects that have a SponsorName matching the input parameter
        /// </summary>
        /// <param name="sponsorName">The name of the sponsor to search for</param>
        /// <returns>An IEnumerable containing ContentObjects that match the SponsorName</returns>
        IEnumerable<ContentObject> GetContentObjectsBySponsorName(string sponsorName);

        /// <summary>
        /// Gets a list of ContentObjects that have a ArtistName matching the input parameter.
        /// </summary>
        /// <param name="artistName">The name of the artist to search for</param>
        /// <returns>An IEnumerable containing ContentObjects that match the ArtistName</returns>
        IEnumerable<ContentObject> GetContentObjectsByArtistName(string artistName);

        /// <summary>
        /// Gets a list of ContentObjects that have a keyword matching the input parameter list
        /// </summary>
        /// <param name="keywords">A comma-delimited list of terms to search for</param>
        /// <returns>An IEnumerable containing ContentObjects that match one or more of the keywords</returns>
        IEnumerable<ContentObject> GetContentObjectsByKeyWords(string keywords);

        /// <summary>
        /// Gets a list of ContentObjects that have a Description matching the input parameter.
        /// </summary>
        /// <param name="description">The description you are searching for</param>
        /// <returns>An IEnumerable containing ContentObjects that match the ArtistName</returns>
        IEnumerable<ContentObject> GetContentObjectsByDescription(string description);
        
        /// <summary>
        /// Gets a list of ContentObjects that have a Title matching the input parameter.
        /// </summary>
        /// <param name="title">The title being searched for.</param>
        /// <returns>An IEnumerable containing ContentObjects that match the Title.</returns>
        IEnumerable<ContentObject> GetContentObjectsByTitle(string title);

        /// <summary>
        /// Gets a list of ContentObjects that were uploaded by a particular user.
        /// </summary>
        /// <param name="email">The email address of the uploader</param>
        /// <returns>All ContentObjects that were uploaded by the user specified in input</returns>
        IEnumerable<ContentObject> GetContentObjectsBySubmitterEmail(string email);

        int GetContentObjectCount();
    }
}
