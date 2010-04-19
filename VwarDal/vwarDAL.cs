using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace vwarDAL
{
    public class vwarDAL
    {
        private string _ConnectionString;
        public vwarDAL(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection String must be a non empty string");
            }
            _ConnectionString = connectionString;
        }
        public IEnumerable<ContentObject> GetAllContentObjects()
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObjects = from co in entities.ContentObject
                                     select co;
                foreach (var co in contentObjects)
                {
                    co.Reviews.Load();
                }
                return contentObjects.ToList();
            }
        }
        public IEnumerable<ContentObject> GetContentObjectsByCollectionName(string collectionName)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObjects = from co in entities.ContentObject
                                     where co.CollectionName == collectionName
                                     select co;
                return contentObjects.ToList();
            }
        }
        public IEnumerable<ContentObject> GetHighestRated(int count)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObjects = from co in entities.ContentObject
                                     orderby co.Reviews.Average((r) => r.Rating) descending
                                     select co;
                return contentObjects.Take(count).ToList();

            }
        }
        public IEnumerable<ContentObject> GetMostPopular(int count)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObjects = from co in entities.ContentObject
                                     orderby co.Views descending
                                     select co;
                return contentObjects.Take(count).ToList();

            }
        }
        public IEnumerable<ContentObject> GetRecentlyUpdated(int count)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObjects = from co in entities.ContentObject
                                     orderby co.LastModified descending
                                     select co;
                return contentObjects.Take(count).ToList();

            }
        }
        public void InsertReview(int rating, string text, string submitterEmail, int contentObjectId)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var review = new Reviews()
                {
                    ReviewText = text,
                    SubmittedBy = submitterEmail,
                    Rating = rating
                };
                review.ContentObject = (from co in entities.ContentObject
                                        where co.Id == contentObjectId
                                        select co).First();
                review.ContentObject.Reviews.Add(review);
                entities.SaveChanges(true);
            }
        }
        public void UpdateContentObject(ContentObject co)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var oldCo = entities.ContentObject.First((x) => x.Id == co.Id);
                oldCo.Title = co.Title;
                oldCo.DescriptionWebsiteURL = co.DescriptionWebsiteURL;
                oldCo.Keywords = co.Keywords;
                oldCo.Location = String.IsNullOrEmpty(co.Location) ? oldCo.Location : co.Location;
                oldCo.ScreenShot = String.IsNullOrEmpty(co.ScreenShot) ? oldCo.ScreenShot : co.ScreenShot;
                oldCo.SubmitterLogoImageFilePath = String.IsNullOrEmpty(co.SubmitterLogoImageFilePath) ? oldCo.SubmitterLogoImageFilePath : co.SubmitterLogoImageFilePath;
                entities.SaveChanges();
            }
        }
        public IEnumerable<ContentObject> GetRecentlyViewed(int count)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObjects = from co in entities.ContentObject
                                     orderby co.LastViewed descending
                                     select co;
                return contentObjects.Take(count).ToList();

            }
        }
        public IEnumerable<ContentObject> SearchContentObjects(string searchTerm)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {

                var contentObjects = from co in entities.ContentObject
                                     where co.Title.Contains(searchTerm) ||
                                     co.Description.Contains(searchTerm) ||
                                     co.Keywords.Contains(searchTerm)
                                     select co;
                foreach (var co in contentObjects)
                {
                    co.Reviews.Load();
                }
                return contentObjects.ToList() ;

            }
        }
        public IEnumerable<ContentObject> GetContentObjectsBySubmitterEmail(string email)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObjects = from co in entities.ContentObject
                                     where co.SubmitterEmail == email
                                     select co;
                return contentObjects.ToList();

            }
        }
        public ContentObject GetContentObjectById(int id, bool updateViews)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var contentObject = (from co in entities.ContentObject
                                     where co.Id == id
                                     select co).First();
                contentObject.Reviews.Load();
                if (updateViews)
                {
                    contentObject.Views++;
                    contentObject.LastViewed = DateTime.Now;
                    entities.SaveChanges(true);
                }
                return contentObject;
            }
        }
        public void DeleteContentObject(int id)
        {
            using (vwarEntities entitiles = new vwarEntities(_ConnectionString))
            {
                var contentObejct = (from co in entitiles.ContentObject
                                     where co.Id == id
                                     select co).First();
                entitiles.DeleteObject(contentObejct);
                entitiles.SaveChanges(true);
            }
        }
        public void InsertContentObject(ContentObject co)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var count = (from c in entities.ContentObject
                             where c.Title == co.Title
                             select c).Count();
                if (count > 0)
                {
                    throw new ArgumentException("A content object with that title already exists");
                }
                entities.AddToContentObject(co);
                entities.SaveChanges(true);
                co.SubmitterLogoImageFilePath = co.Id + co.SubmitterLogoImageFilePath.Substring(co.SubmitterLogoImageFilePath.LastIndexOf("."));
                entities.SaveChanges(true);
            }
        }
        public void IncrementDownloads(int id)
        {
            using (vwarEntities entities = new vwarEntities(_ConnectionString))
            {
                var co = (from c in entities.ContentObject
                          where c.Id == id
                          select c).First();
                co.Downloads++;
                entities.SaveChanges(true);
            }
        }
    }
}
